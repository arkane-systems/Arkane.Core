# Annotations

**Namespace:** `ArkaneSystems.Arkane.Annotations`

The `ArkaneSystems.Arkane.Annotations` namespace provides attributes that carry metadata about code authorship and assembly provenance.

---

## AuthorAttribute

```csharp
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class AuthorAttribute : Attribute, IEquatable<AuthorAttribute>
```

Records authorship information for an assembly or any smaller code artifact. Multiple instances may be applied to the same target.

### Constructor

```csharp
public AuthorAttribute(string name, string emailAddress)
```

| Parameter | Description |
|-----------|-------------|
| `name` | The author's display name. |
| `emailAddress` | The author's e-mail address (required; used as the identity key). |

Both parameters must be non-empty strings. `emailAddress` must be a valid e-mail address (validated by a Metalama `[Email]` contract).

### Equality

Two `AuthorAttribute` instances are considered equal when their `EmailAddress` values match (case-insensitive). The `Name` property is informational only and is not part of equality.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | The author's display name. |
| `EmailAddress` | `string` | The author's e-mail address. |

### Usage

```csharp
// Assembly-level authorship
[assembly: Author("Alistair Young", "alistair@example.com")]

// Per-file or per-class override
[Author("Jane Smith", "jane@example.com")]
public class ContributedFeature { }
```

---

## SourceLanguageAttribute

```csharp
[AttributeUsage(AttributeTargets.Assembly)]
public class SourceLanguageAttribute : Attribute
```

An assembly-level attribute that records the primary source language and post-compilation modification state of an assembly.

### Constructor

```csharp
public SourceLanguageAttribute(ProgrammingLanguages language = ProgrammingLanguages.CSharp)
```

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Language` | `ProgrammingLanguages` | The primary language in which the assembly was written. Defaults to `CSharp`. |
| `RewrittenByAspects` | `bool` | `true` if the assembly's IL has been post-processed by an aspect weaver (e.g., Metalama, PostSharp). |
| `Obfuscated` | `bool` | `true` if the assembly has been obfuscated after compilation. |
| `PostCompilationModifications` | `string` | Free-text description of other post-compilation modifications (e.g., ILMerge, NativeAOT trimming). |

### Usage

```csharp
[assembly: SourceLanguage(RewrittenByAspects = true)]
```

---

## ProgrammingLanguages

An enum that identifies the programming language in which source code is written. Used by `SourceLanguageAttribute`.

| Value | Description |
|-------|-------------|
| `CSharp` | C# |
| `VisualBasic` | Visual Basic .NET |
| `FSharp` | F# |
| `Il` | Common Intermediate Language (CIL/MSIL) |
| `Python` | Python |
| `PowerShell` | PowerShell |
| `JavaScript` | JavaScript |
