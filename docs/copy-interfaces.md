# Copy Interfaces

Arkane.Core provides two generic interfaces for explicitly declaring object-copy semantics: `IDeepCopy<T>` and `IShallowCopy<T>`. Both extend `ICloneable` to maintain compatibility with the standard .NET copy contract.

## Background: Deep Copy vs. Shallow Copy

| | Shallow Copy | Deep Copy |
|-|-------------|-----------|
| New instance created? | ✅ | ✅ |
| Fields/properties copied? | ✅ | ✅ |
| Referenced objects duplicated? | ❌ (same references) | ✅ (new copies) |

A **shallow copy** creates a new object but copies only the *references* to any nested objects — both the original and the copy point to the *same* inner objects. A **deep copy** recursively duplicates all nested objects so that the original and the copy are entirely independent.

---

## IShallowCopy\<T\>

**Namespace:** `ArkaneSystems.Arkane`

```csharp
public interface IShallowCopy<out T> : ICloneable
{
    T ShallowCopy();
}
```

Implement this interface on types that support shallow copying. The `Clone()` method inherited from `ICloneable` must delegate to `ShallowCopy()`.

### Example

```csharp
public class Tag : IShallowCopy<Tag>
{
    public string Name { get; set; } = string.Empty;
    public List<string> Aliases { get; set; } = [];

    public Tag ShallowCopy()
    {
        // MemberwiseClone copies the reference to Aliases, not the list itself
        return (Tag)MemberwiseClone();
    }

    object ICloneable.Clone() => ShallowCopy();
}
```

After a shallow copy, both instances share the **same** `Aliases` list — adding to one affects the other.

---

## IDeepCopy\<T\>

**Namespace:** `ArkaneSystems.Arkane`

```csharp
public interface IDeepCopy<out T> : ICloneable
{
    T DeepCopy();
}
```

Implement this interface on types that support deep copying. The `Clone()` method inherited from `ICloneable` must delegate to `DeepCopy()`.

### Example

```csharp
public class Tag : IDeepCopy<Tag>
{
    public string Name { get; set; } = string.Empty;
    public List<string> Aliases { get; set; } = [];

    public Tag DeepCopy()
    {
        return new Tag
        {
            Name = this.Name,
            Aliases = [..this.Aliases]  // new independent list
        };
    }

    object ICloneable.Clone() => DeepCopy();
}
```

After a deep copy, both instances have their **own** `Aliases` list and are fully independent.

---

## Rules

> ⚠️ **Do not implement both `IDeepCopy<T>` and `IShallowCopy<T>` on the same class.** The two interfaces represent mutually exclusive copy semantics; implementing both on a single type is ambiguous and misleading to consumers.

- `Clone()` **must** delegate to `DeepCopy()` on types implementing `IDeepCopy<T>`.
- `Clone()` **must** delegate to `ShallowCopy()` on types implementing `IShallowCopy<T>`.
