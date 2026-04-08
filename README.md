# Arkane Libraries

**Arkane.Core** is the foundational base class library (BCL) for the Arkane Systems suite of .NET libraries. It provides lightweight, reusable interfaces and utilities for other Arkane libraries and consumer applications.

- **Author:** Alistair J. R. Young
- **Company:** Arkane Systems
- **License:** MIT
- **Target framework:** .NET 10

## Getting Started

Install via NuGet:

```dotnet add package Arkane.Core```

## Contents

| Area | Types |
|------|-------|
| Copy interfaces | `IDeepCopy<T>`, `IShallowCopy<T>`, `DeepCopiableObject`, `ShallowCopiableObject` |
| Random numbers | `RandomProvider` |
| Annotations | `AuthorAttribute`, `SourceLanguageAttribute`, `ProgrammingLanguages`, `PublicAPIAttribute`, `UsedImplicitlyAttribute`, `MeansImplicitUseAttribute` |
| Aspects | `BreakBeforeAttribute`, `BreakAfterAttribute`, `DivisibleByAttribute`, `LogMethodCallsAttribute`, `NoOpAttribute`, `StopwatchAttribute`, `RetryAttribute` |
| Extension methods | `string`, `bool`, `DateTime`, `Math`, `Action`, `Array`, `byte[]`, `Guid`, `IBinaryInteger<T>`, `INumber<T>`, `JsonElement`, `Type` |
| Disposal patterns | `IDisposable<T>`, `DisposerBase<T>`, `DisposerByDelegation<T>`, `DisposerByReflection<T>` |
| Utilities | `CannotHappenException`, `SubclassResponsibilityException`, `Epochs`, `Empty`, `EventArgs<T>`, `Memento`, `IFluent` |
| Security | `CryptoHelpers`, `HashHelpers` |
| Component model | `IGenericMetadata` |

## Documentation

- [Overview](docs/index.md)
- [Copy Interfaces](docs/copy-interfaces.md)
- [RandomProvider](docs/random-provider.md)
- [Annotations](docs/annotations.md)
- [Aspects](docs/aspects.md)
- [Extension Methods](docs/extensions.md)
- [Disposal Patterns](docs/disposal.md)
- [Utilities](docs/utilities.md)
- [Security](docs/security.md)
- [Component Model](docs/component-model.md)

