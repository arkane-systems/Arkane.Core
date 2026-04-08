# Utility Types

Arkane.Core provides a collection of small, general-purpose utility types. All live under the **`ArkaneSystems.Arkane`** namespace unless otherwise noted.

---

## CannotHappenException

A specialisation of `InvalidOperationException` for code paths that are logically unreachable.

Use it in branches that the program's invariants guarantee will never execute — for example, the `default` case of a `switch` that covers every member of an enum. Throwing `CannotHappenException` signals an internal bug rather than a user or caller error.

```csharp
switch (status)
{
    case Status.Active:   return HandleActive();
    case Status.Inactive: return HandleInactive();
    default:
        throw new CannotHappenException($"Unexpected status: {status}");
}
```

### Constructors

| Overload | Description |
|----------|-------------|
| `CannotHappenException()` | Uses a default message. |
| `CannotHappenException(string message)` | Appends `message` to a standard "this cannot happen" prefix. |
| `CannotHappenException(string message, Exception inner)` | Same, with an inner exception. |

> 💡 Before reaching for this exception, consider whether the enclosing type should be `abstract` instead and the branch should be a virtual/abstract method.

---

## SubclassResponsibilityException

A specialisation of `NotImplementedException` for base-class methods that subclasses are expected to override.

Throw it from a non-abstract method in a base class to communicate "this method must be implemented by a subclass". The preferred design is to make the base class or method `abstract` whenever possible; use `SubclassResponsibilityException` as a fallback when that is not practical.

```csharp
public class Animal
{
    public virtual string Speak()
        => throw new SubclassResponsibilityException();
}

public class Dog : Animal
{
    public override string Speak() => "Woof";
}
```

### Constructors

| Overload | Description |
|----------|-------------|
| `SubclassResponsibilityException()` | Uses a default message. |
| `SubclassResponsibilityException(string message)` | Uses the provided message. |
| `SubclassResponsibilityException(string message, Exception inner)` | With an inner exception. |

---

## Epochs

A static class providing well-known `DateTime` epoch reference points.

| Property | Value | Description |
|----------|-------|-------------|
| `Epochs.LinuxEpoch` | 1 January 1970 00:00:00 UTC | Unix/Linux epoch |
| `Epochs.MacEpoch` | 1 January 1904 00:00:00 | Classic Mac OS epoch |
| `Epochs.WindowsEpoch` | 1 January 0001 00:00:00 | Windows/.NET `DateTime.MinValue` epoch |

```csharp
TimeSpan sinceUnix = DateTime.UtcNow - Epochs.LinuxEpoch;
```

These values are also used internally by the `DateTime` extension methods `ToLinuxTimeStamp()` and `FromLinuxTimeStamp()`.

---

## Empty

A unit-like value struct with no state, analogous to `Unit` in functional languages or `void` as a generic type argument.

- All instances are considered equal to each other.
- `ToString()` returns `"<empty>"`.
- Has size 1 byte.

Common uses:

- As a type argument when a generic type requires a `T` but no data needs to be carried — for example, `Task<Empty>` or `EventArgs<Empty>`.
- As the payload of `EventArgs<T>` for events that carry no data.

```csharp
// An event that signals "something happened" with no additional data
public event EventHandler<EventArgs<Empty>>? SomethingHappened;
```

---

## EventArgs\<T\>

A generic subclass of `System.EventArgs` that carries a strongly-typed payload.

```csharp
public class EventArgs<T> : EventArgs
{
    public T Value { get; }
}
```

Use it instead of creating a bespoke `EventArgs` subclass for simple, single-value event data.

```csharp
public event EventHandler<EventArgs<string>>? UserNameChanged;

// Raise:
UserNameChanged?.Invoke(this, new EventArgs<string>("Alice"));

// Handle:
service.UserNameChanged += (sender, e) => Console.WriteLine(e.Value);
```

For events that carry no data, use `EventArgs<Empty>` (see [Empty](#empty) above).

---

## Memento

An `IDisposable` that executes a delegate when disposed. It implements the [Memento pattern](https://en.wikipedia.org/wiki/Memento_pattern) as a scope guard — a convenient way to schedule a cleanup or undo action that runs when execution leaves a `using` block.

```csharp
int savedValue = counter;
using (new Memento(() => counter = savedValue))
{
    counter = 0;
    DoWork(); // counter might be changed inside
} // counter restored to savedValue here
```

You can also create a `Memento` from an `Action` via the extension method `action.AsMemento()`:

```csharp
using (resetAction.AsMemento())
{
    // ...
}
```

The action is guaranteed to run exactly once; subsequent `Dispose()` calls are no-ops.

---

## IFluent

A marker interface for fluent-style builder APIs.

When implemented, `IFluent` hides `Equals`, `GetHashCode`, `ToString`, and `GetType` from IDE IntelliSense (via `[EditorBrowsable(EditorBrowsableState.Never)]`). This keeps fluent builder method chains clean by suppressing irrelevant object members from the completion list.

```csharp
public class QueryBuilder : IFluent
{
    public QueryBuilder Where(string condition) { ... return this; }
    public QueryBuilder OrderBy(string column)  { ... return this; }
    public string Build() { ... }
}

// IntelliSense only shows Where, OrderBy, Build — not Equals, GetHashCode, etc.
var sql = new QueryBuilder()
    .Where("active = 1")
    .OrderBy("name")
    .Build();
```
