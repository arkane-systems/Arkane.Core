# Disposal Patterns

Arkane.Core provides a small framework for managing the lifecycle of objects that require a cleanup step but that do not implement `System.IDisposable`. All types live under the **`ArkaneSystems.Arkane`** namespace.

---

## IDisposable\<T\>

An extension of `System.IDisposable` that exposes the underlying wrapped value through a covariant `Value` property.

```csharp
public interface IDisposable<out T> : IDisposable
{
    T Value { get; }
}
```

The covariant `out T` means that an `IDisposable<DerivedType>` can be used where `IDisposable<BaseType>` is expected, mirroring the covariance of `IEnumerable<T>`.

```csharp
IDisposable<Stream> disposableStream = /* ... */;
using (disposableStream)
{
    disposableStream.Value.Write(data, 0, data.Length);
}
```

---

## DisposerBase\<T\>

An abstract base class that wraps an object `T` and provides a thread-safe implementation of both `IDisposable` and `IAsyncDisposable`. Derive from this class when you need a custom disposal strategy for an object you do not control.

```csharp
public abstract class DisposerBase<T> : IDisposable, IAsyncDisposable
{
    public T Object { get; protected set; }
    protected bool IsDisposed;

    public void Dispose();                              // sync entry point
    public ValueTask DisposeAsync();                    // async entry point
    protected void Dispose(bool disposing);             // cleanup dispatcher
    protected abstract void DisposeImplementation(bool disposing);
    protected virtual ValueTask DisposeAsyncCore();     // async override point
}
```

### Disposal lifecycle

**Synchronous path** (`using`):
1. `Dispose()` — suppresses the finalizer, atomically transitions to disposed state (throws `ObjectDisposedException` if already disposed), then calls `Dispose(true)` → `DisposeImplementation(true)`.

**Asynchronous path** (`await using`):
1. `DisposeAsync()` — suppresses the finalizer, atomically transitions to disposed state (throws if already disposed).
2. Calls `DisposeAsyncCore()` — the async override point. Awaits it.
3. Calls `Dispose(false)` — invokes `DisposeImplementation(false)` to suppress any unmanaged-only re-cleanup.

**Finalizer path:**
- If neither `Dispose()` nor `DisposeAsync()` was called before GC collection, the finalizer calls `Dispose(false)` directly. It checks `IsDisposed` without locking and never throws.

### What to override

| Method | When to override |
|--------|-----------------|
| `DisposeImplementation(bool disposing)` | **Always** — this is where your cleanup logic lives. When `disposing` is `true`, release managed resources. When `false` (finalizer path), release unmanaged resources only. |
| `DisposeAsyncCore()` | Only when you have true async cleanup (e.g., flushing a network stream). The default implementation calls `DisposeImplementation(true)` synchronously and returns `ValueTask.CompletedTask`. |

### Key points

- **Thread-safe single-disposal:** A `Lock` atomically transitions to the disposed state. A second call to either `Dispose()` or `DisposeAsync()` throws `ObjectDisposedException` — including cross-mode calls (e.g., calling `Dispose()` after `DisposeAsync()` has already run).
- **`GC.SuppressFinalize`** is called at the top of both `Dispose()` and `DisposeAsync()`, so the finalizer is always suppressed when disposal happens through either public entry point.
- **Sync is always sufficient:** `Dispose()` handles all cleanup. `DisposeAsync()` is an optional optimization; if you don't have async cleanup needs, there is no need to override `DisposeAsyncCore()`.

### Example — sync only

```csharp
public class MyDisposer : DisposerBase<SomeResource>
{
    public MyDisposer(SomeResource r) : base(r) { }

    protected override void DisposeImplementation(bool disposing)
    {
        if (!disposing) return;       // finalizer path — skip managed cleanup
        Object.Shutdown();
    }
}
```

### Example — with async override

```csharp
public class MyAsyncDisposer : DisposerBase<SomeResource>
{
    public MyAsyncDisposer(SomeResource r) : base(r) { }

    protected override void DisposeImplementation(bool disposing)
    {
        if (!disposing) return;
        Object.Shutdown();            // sync fallback (used by Dispose() and finalizer)
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await Object.ShutdownAsync(); // true async cleanup (used by DisposeAsync())
    }
}
```

> **Note:** `DisposeAsyncCore()` is only called on the managed-resource path. The subsequent `Dispose(false)` call after `DisposeAsyncCore()` completes is a no-op for managed resources — it exists solely to cover unmanaged resource cleanup if your override did not handle it.

---

## DisposerByDelegation\<T\>

A ready-to-use concrete `DisposerBase<T>` that accepts disposal delegates at construction time. This is the most convenient option when the cleanup logic is simple enough to express as a lambda.

```csharp
public class DisposerByDelegation<T> : DisposerBase<T>
{
    // Sync-only: DisposeAsync() falls back to disposeAction
    public DisposerByDelegation(T obj, Action<T> disposeAction);

    // With async path: DisposeAsync() uses asyncDisposeAction
    public DisposerByDelegation(T obj, Action<T> disposeAction, Func<T, ValueTask> asyncDisposeAction);
}
```

Both constructors throw `ArgumentNullException` at construction time if any delegate argument is `null`.

### Sync usage

```csharp
var connection = GetLegacyConnection();
using (new DisposerByDelegation<LegacyConnection>(connection, c => c.Close()))
{
    connection.Execute(query);
} // connection.Close() is called here
```

### Async usage

```csharp
var connection = GetLegacyConnection();
await using (new DisposerByDelegation<LegacyConnection>(
    connection,
    c => c.Close(),
    async c => await c.CloseAsync()))
{
    await connection.ExecuteAsync(query);
} // connection.CloseAsync() is called here
```

### Behaviour notes

- `disposeAction` is invoked only on the managed-disposal path (`disposing == true`). If the object is collected by the GC without `Dispose()` being called first, the delegate is **not** invoked.
- If `asyncDisposeAction` is supplied, `DisposeAsync()` uses it exclusively. If not, `DisposeAsync()` falls back to invoking `disposeAction` synchronously.
- **Cross-mode double-dispose:** Calling `Dispose()` after `DisposeAsync()` has already completed (or vice versa) throws `ObjectDisposedException`. The single-disposal guarantee applies across both entry points.

---

## DisposerByReflection\<T\>

A concrete `DisposerBase<T>` that discovers and invokes cleanup methods on the wrapped object via reflection. Use this when the cleanup method name is not known until runtime, or when you want to avoid capturing a method reference in a delegate closure.

```csharp
public class DisposerByReflection<T> : DisposerBase<T>
{
    public DisposerByReflection(T obj, string methodName = "Dispose", string? asyncMethodName = "DisposeAsync");
    public DisposerByReflection(T obj, MethodInfo method);

    public MethodInfo  Method      { get; protected set; }  // resolved sync method
    public MethodInfo? AsyncMethod { get; protected set; }  // resolved async method, or null
}
```

### Sync disposal

- Finds a public parameterless instance method named `methodName` on the wrapped object.
- Throws `ArgumentException` at construction time if the method is not found, is static, has parameters, or contains unassigned generic type parameters.
- `TargetInvocationException` is unwrapped at disposal time — the inner exception propagates directly.

```csharp
// Calls resource.Release() via reflection
using var d = new DisposerByReflection<LegacyResource>(resource, "Release");
```

### Async disposal precedence

When `DisposeAsync()` is called, `DisposeAsyncCore()` applies the following logic in order:

1. **`IAsyncDisposable` interface check** — if the wrapped object implements `IAsyncDisposable`, its `DisposeAsync()` is called via interface dispatch. This means the explicit interface implementation takes priority over any public `DisposeAsync()` method of the same name.
2. **Reflection for async method** — if `asyncMethodName` is not `null`, reflects for a public parameterless instance method with that name whose return type is exactly `ValueTask` or `Task`. Methods with other return types (e.g. `Task<int>`) are silently skipped.
3. **Sync fallback** — if neither of the above applies, falls back to `DisposeImplementation(true)` synchronously.

Setting `asyncMethodName: null` disables steps 1 and 2 entirely — async discovery is skipped and sync fallback is immediate. Note that step 1 (the `IAsyncDisposable` check) only applies when using the `string methodName` constructor; the `MethodInfo` constructor does not populate `AsyncMethod` or the interface flag.

```csharp
// Uses reflection to call obj.Dispose() sync; obj.DisposeAsync() async (if found)
using var d = new DisposerByReflection<LegacyResource>(resource);

// Calls obj.Release() sync; obj.ReleaseAsync() async (if found)
await using var d = new DisposerByReflection<LegacyResource>(resource, "Release", "ReleaseAsync");

// Sync-only — no async discovery attempted
using var d = new DisposerByReflection<LegacyResource>(resource, asyncMethodName: null);
```

The `AsyncMethod` property reflects what was resolved at construction time. If it is `null`, the async path will use either the `IAsyncDisposable` interface (if applicable) or sync fallback.

---

## Thread safety

All three disposal classes share the same guarantee from `DisposerBase<T>`:

- A `Lock` is used to atomically transition to the disposed state.
- **Only one call wins** — whichever of `Dispose()` or `DisposeAsync()` is called first completes normally.
- **All subsequent calls throw** — any later call to either `Dispose()` or `DisposeAsync()`, from any thread, throws `ObjectDisposedException`. This includes cross-mode calls (e.g., `DisposeAsync()` after `Dispose()`).
- The **finalizer** checks `IsDisposed` without locking and never throws — it only acts if disposal has not yet occurred.

---

## When to use which

| Scenario | Recommended type |
|----------|-----------------|
| Cleanup logic is a simple lambda | `DisposerByDelegation<T>` |
| Cleanup method name is known at compile time | `DisposerByDelegation<T>` (call the method in the lambda) |
| You need true async cleanup with a lambda | `DisposerByDelegation<T>` (three-argument constructor) |
| You only know the method name at runtime, or want to avoid capturing the method in a closure | `DisposerByReflection<T>` |
| Wrapped type implements `IAsyncDisposable` and you want interface dispatch | `DisposerByReflection<T>` (detects and uses `IAsyncDisposable` automatically) |
| Custom cleanup logic with derived-class structure | Subclass `DisposerBase<T>` directly |
