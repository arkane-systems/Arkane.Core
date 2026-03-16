# Annotations

**Namespace:** `ArkaneSystems.Arkane.Annotations`

The `ArkaneSystems.Arkane.Annotations` namespace provides attributes that communicate intent to static analysis tools such as Roslyn analyzers, ReSharper, or Rider. They have **no effect at runtime**.

---

## PublicAPIAttribute

```csharp
[AttributeUsage(AttributeTargets.All, Inherited = false)]
public sealed class PublicAPIAttribute : Attribute
```

Marks a type or member as part of the library's intentional public API. Any symbol tagged `[PublicAPI]` will not be reported as unused by static analysis, even if no internal references to it exist within the same assembly.

### Usage

```csharp
[PublicAPI]
public class MyLibraryClass { ... }

[PublicAPI("Consumed by the external plugin system")]
public static void RegisterPlugin(IPlugin plugin) { ... }
```

An optional `Comment` string can explain *why* the member is considered public API.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Comment` | `string` | Optional explanation of why this is public API |

---

## UsedImplicitlyAttribute

```csharp
[AttributeUsage(AttributeTargets.All)]
public sealed class UsedImplicitlyAttribute : Attribute
```

Marks a symbol as being used implicitly — for example via reflection, a dependency injection container, or an external framework — so that static analysis tools do not report it as unused.

### Usage

```csharp
// A class instantiated by a DI container
[UsedImplicitly]
public class MyService { }

// A constructor called by a serialiser with a fixed signature
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
public MyService(ILogger logger) { }

// An interface whose implementors are all considered used
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IHandler { }
```

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `UseKindFlags` | `ImplicitUseKindFlags` | How the symbol is implicitly used |
| `TargetFlags` | `ImplicitUseTargetFlags` | Which parts of the symbol are considered used |
| `Reason` | `string` | Optional explanation |

---

## MeansImplicitUseAttribute

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.GenericParameter | AttributeTargets.Parameter)]
public sealed class MeansImplicitUseAttribute : Attribute
```

Applied to a *custom attribute class* to make that attribute behave like `[UsedImplicitly]`. Any symbol decorated with the custom attribute will be treated as implicitly used.

This is how `PublicAPIAttribute` itself works — it is decorated with `[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]`, which means every type or member tagged `[PublicAPI]` is automatically treated as used along with all of its members.

### Usage

```csharp
// All members of types decorated with [MyFrameworkComponent] will be treated as used
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
public sealed class MyFrameworkComponentAttribute : Attribute { }
```

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `UseKindFlags` | `ImplicitUseKindFlags` | How decorated symbols are considered used |
| `TargetFlags` | `ImplicitUseTargetFlags` | Which parts of decorated symbols are considered used |

---

## ImplicitUseKindFlags

Specifies *how* a symbol is considered to be implicitly used.

| Value | Description |
|-------|-------------|
| `Default` | Equivalent to `Access \| Assign \| InstantiatedWithFixedConstructorSignature` |
| `Access` | The symbol is read or accessed implicitly |
| `Assign` | The symbol is assigned implicitly |
| `InstantiatedWithFixedConstructorSignature` | The type is instantiated with a specific constructor; unused constructor parameters are suppressed |
| `InstantiatedNoFixedConstructorSignature` | The type is instantiated implicitly with no fixed constructor signature |

---

## ImplicitUseTargetFlags

Specifies *which parts* of a symbol are considered used.

| Value | Description |
|-------|-------------|
| `Default` | Equivalent to `Itself` |
| `Itself` | Only the symbol itself is considered used |
| `Members` | The symbol's members are considered used |
| `WithInheritors` | Inheriting or implementing types are considered used |
| `WithMembers` | The symbol and all its members are considered used (`Itself \| Members`) |
