# Arkane.Core — Overview

Arkane.Core is the foundational library in the Arkane Systems base class library (BCL). Targeting .NET 10, it provides a small set of lightweight, reusable abstractions for other Arkane libraries and consumer applications to build upon.

## Namespace

All public types live under the `ArkaneSystems.Arkane` namespace, with the following sub-namespaces for specific areas:

| Sub-namespace | Contents |
|---------------|----------|
| `ArkaneSystems.Arkane.Annotations` | Annotation attributes (authorship, source language, static-analysis hints) |
| `ArkaneSystems.Arkane.Aspects` | Metalama aspect attributes (break, contract, diagnostics, resilience) |
| `ArkaneSystems.Arkane.Security.Cryptography` | Cryptographic key and hash helpers |
| `ArkaneSystems.Arkane.ComponentModel.Composition` | MEF support interfaces |

## What's Included

| Area | Types | Description |
|------|-------|-------------|
| [Copy interfaces](copy-interfaces.md) | `IDeepCopy<T>`, `IShallowCopy<T>`, `DeepCopiableObject`, `ShallowCopiableObject` | Strongly-typed contracts and base classes for deep and shallow copying |
| [Random numbers](random-provider.md) | `RandomProvider` | Thread-safe per-thread `System.Random` singleton |
| [Annotations](annotations.md) | `AuthorAttribute`, `SourceLanguageAttribute`, `ProgrammingLanguages`, `PublicAPIAttribute`, `UsedImplicitlyAttribute`, `MeansImplicitUseAttribute` | Authorship metadata and static analysis hints |
| [Aspects](aspects.md) | `BreakBeforeAttribute`, `BreakAfterAttribute`, `DivisibleByAttribute`, `LogMethodCallsAttribute`, `NoOpAttribute`, `StopwatchAttribute`, `RetryAttribute` | Metalama aspects for debugging, validation, diagnostics, and resilience |
| [Extension methods](extensions.md) | `ExtensionMethods` (partial) | Extensions for `string`, `bool`, `DateTime`, `Math`, `Action`, `Array`, `byte[]`, `Guid`, `IBinaryInteger<T>`, `INumber<T>`, `JsonElement`, `Type` |
| [Disposal patterns](disposal.md) | `IDisposable<T>`, `DisposerBase<T>`, `DisposerByDelegation<T>`, `DisposerByReflection<T>` | Wrappers for managing the lifecycle of objects that lack `IDisposable` |
| [Utilities](utilities.md) | `CannotHappenException`, `SubclassResponsibilityException`, `Epochs`, `Empty`, `EventArgs<T>`, `Memento`, `IFluent` | General-purpose helper types |
| [Security](security.md) | `CryptoHelpers`, `HashHelpers` | Cryptographic key generation and salting helpers |
| [Component model](component-model.md) | `IGenericMetadata` | MEF part metadata interface |

## Package

| Property | Value |
|----------|-------|
| Package ID | `Arkane.Core` |
| Version | 0.1.0 |
| License | MIT |
| Target framework | .NET 10 |
| Strong-named | Yes |
