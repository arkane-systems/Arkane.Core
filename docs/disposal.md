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

An abstract base class that wraps an object `T` and provides a thread-safe `IDisposable` implementation around it. Derive from this class when you need a custom disposal strategy for an object you do not control.

```csharp
public abstract class DisposerBase<T> : IDisposable
{
    public T Object { get; protected set; }
    protected bool IsDisposed;

    public void Dispose();                              // calls Dispose(true), suppresses finalizer
    protected void Dispose(bool disposing);             // thread-safe; enforces single-dispose
    protected abstract void DisposeImplementation(bool disposing);
}
```

### Key points

- **Thread-safe single-disposal:** The base class uses a `Lock` to ensure `DisposeImplementation` is called exactly once. A second call to `Dispose()` throws `ObjectDisposedException`.
- **Finalizer safety:** A finalizer calls `Dispose(false)`, passing `disposing: false` to signal that managed resources should not be accessed. Your override should check this flag.
- **Override `DisposeImplementation`:** Implement your cleanup logic here. Respect the `disposing` parameter.

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

---

## DisposerByDelegation\<T\>

A ready-to-use concrete `DisposerBase<T>` that accepts a disposal delegate at construction time. This is the most convenient option when the cleanup logic is simple enough to express as a lambda.

```csharp
public class DisposerByDelegation<T> : DisposerBase<T>
{
    public DisposerByDelegation(T obj, Action<T> disposeAction);
}
```

```csharp
var connection = GetLegacyConnection();
using (new DisposerByDelegation<LegacyConnection>(connection, c => c.Close()))
{
    connection.Execute(query);
} // connection.Close() is called here
```

The `disposeAction` delegate is invoked only on the managed-disposal path (`disposing == true`). If the object is collected by the GC without `Dispose()` being called first, the delegate is **not** invoked.

---

## DisposerByReflection\<T\>

A concrete `DisposerBase<T>` that discovers and invokes a named public parameterless instance method on the wrapped object via reflection. The default method name is `"Dispose"`, but you can supply a different name (e.g., `"Close"`, `"Shutdown"`, `"Release"`).

```csharp
public class DisposerByReflection<T> : DisposerBase<T>
{
    public DisposerByReflection(T obj, string methodName = "Dispose");
    public DisposerByReflection(T obj, MethodInfo method);
}
```

```csharp
var resource = GetLegacyResource();
using (new DisposerByReflection<LegacyResource>(resource, "Release"))
{
    resource.DoWork();
} // resource.Release() is called via reflection
```

If the specified method cannot be found, or the method is static, has parameters, or contains unassigned generic type parameters, an `ArgumentException` is thrown at construction time (not at disposal time).

`TargetInvocationException` is unwrapped during disposal — if the invoked method throws, its inner exception propagates directly.

### When to use which

| Scenario | Recommended type |
|----------|-----------------|
| Cleanup logic is a simple lambda | `DisposerByDelegation<T>` |
| Cleanup method name is known at compile time | `DisposerByDelegation<T>` (call the method in the lambda) |
| You only know the method name at runtime, or you want to avoid capturing the method in a closure | `DisposerByReflection<T>` |
| Custom cleanup logic with derived-class structure | Subclass `DisposerBase<T>` directly |
