# Arkane.Core Copilot Instructions

## Project Overview

**Arkane.Core** is a foundational .NET 10 base class library (BCL) for the Arkane Systems suite. It provides lightweight, reusable interfaces and utilities including copy semantics, random number providers, disposal patterns, and annotations.

- **Author:** Alistair J. R. Young  
- **License:** MIT  
- **Target Framework:** .NET 10 with `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>`
- **Assembly Signing:** Uses strong-named assembly key (`arkane.snk`)
- **Strict Build:** `<TreatWarningsAsErrors>True</TreatWarningsAsErrors>` in both Debug and Release

## Architectural Principles

### Annotations and Metadata

- **JetBrains.Annotations** attributes should be used generously for tooling support and annotations.
	- Particularly important here is the use of `[PublicAPI]` to mark public API surface.
	- Since nullability is enabled, attributes like `[NotNull]`, `[CanBeNull]`, and `[MaybeNull]` should be used to clarify nullability contracts where necessary.
	- Since these attributes are critical for documentation and tooling, the `JETBRAINS_ANNOTATIONS` conditional should be defined in both Debug and Release configurations to ensure they are included in the build.
- Annotation attributes found in `Arkane.Core/Annotations/` should also be used when appropriate.

### Metalama Usage

- Metalama is available for code generation and aspect-oriented programming. It should be used where appropriate.
	 - This specifically includes the use of `Metalama.Patterns.Contracts` aspects to enforce interface contracts, preconditions, postconditions, and invariants.
	 - Metalama aspects can also be used to generate boilerplate code, such as property change notifications or equality members, or for any other appropriate use case.
	 - Metalama aspects defined in `Arkane.Core/Aspects/` should be used consistently across the project to ensure maintainability and readability.
	 - **Automatic null validation:** `GlobalFabric.cs` calls `amender.VerifyNotNullableDeclarations()`, which automatically adds `ArgumentNullException`-throwing contract aspects to all non-nullable parameter declarations in the project. As a result, **do not add manual null checks** for non-nullable parameters — the contract aspect handles this automatically.
	 - **`AspectResources` must be `public`:** The `Arkane.Core/Aspects/AspectResources.resx` file is generated with `PublicResXFileCodeGenerator` to produce a public `AspectResources` class. This is required because Metalama weaves aspect code into client assemblies at compile time, and that woven code needs to access these resources from the client's context. Do **not** change this generator to `ResXFileCodeGenerator`.

## Conventions

### Naming & Organization

- **Namespace:** `ArkaneSystems.Arkane` (configured in `.csproj` as `<RootNamespace>`)
- **File headers:** Every file includes a standard header block with author, copyright, and creation date (in `#region header`)
- **Using statements:** Organized in `#region using` before namespace declaration
- **Extension method files:** Named `ExtensionMethods-System-<TargetType>.cs`

### Code Style
- **Nullable reference types enabled** – for non-nullable parameters, null validation is handled automatically by the `VerifyNotNullableDeclarations` contract aspect (see Metalama Usage above); do not add manual null checks for non-nullable parameters
- **Implicit usings enabled** – no need for common `System.*` usings
- **Fluent API marker:** `IFluent` interface signals fluent-style methods
- **XML documentation:** All public and protected types and methods require `///` comments. Other types and methods should have them where necessary or useful for clarity.
- **Implicit keyword use:** Use `private` by default, escalate visibility only when needed

### Extension Methods for System Types

- Organized by namespace using partial classes (ExtensionMethods-System-String.cs, etc.)
- Use #pragma warning disable IDE0130 to allow static extension method classes outside their namespace
- See Arkane.Core/Extensions/

### Test Organization
- **Framework:** MSTest (`[TestClass]`, `[TestMethod]`)
- **Naming:** `{ComponentName}Tests` (e.g., `RandomProviderTests`)
- **Pattern:** AAA (Arrange-Act-Assert) with descriptive method names
- **Location:** `Arkane.Core.UnitTests/` mirrors main project structure
- Tests cover interfaces, base classes, extension methods, and utilities

## Development Workflow

### Build & Warnings
- Run `dotnet build` – all warnings are treated as errors
- Strong-named assembly: signing key from `\\arkane-systems.lan\Folders\home\arkane.snk`
- Debug and Release configurations both apply strict rules

### Versioning
- **Version Source:** `<VersionPrefix>0.1.0</VersionPrefix>` in `.csproj` (semantic versioning)
- **Development Suffix:** Debug builds append `-dev` to version via `<VersionSuffix>dev</VersionSuffix>`
- **Package Generation:** Enabled with `<GeneratePackageOnBuild>True</GeneratePackageOnBuild>` – produces NuGet package on each build
- Release builds use clean version (no suffix) for public distribution

### External Dependencies
- **JetBrains.Annotations:** Required dependency for annotation attributes
  - Critical for IDE tooling (ReSharper, Visual Studio) and API documentation
  - Included in package as a dependency
  - Define `JETBRAINS_ANNOTATIONS` conditional in both Debug/Release configurations
- **Metalama:** Dependency for code generation and AOP
  - Use where appropriate for contract enforcement and boilerplate reduction
  - Aspects should be defined in `Arkane.Core/Aspects/` and used consistently

### Testing
- Run `dotnet test` to execute MSTest suite in `Arkane.Core.UnitTests`
- Each test class targets a single public type or concern
- Use `TestMethod` attribute for individual test methods with clear behavior names

### Documentation
- Maintain Markdown docs in `docs/` directory
- Use separate agent role: `docs_agent` (see `.github/agents/docs.agent.md`)
- Keep docs in sync with code changes, and especially XML comments.

## Patterns & Practices

### When Adding Public APIs
1. Add `[PublicAPI]` attribute (from JetBrains.Annotations)
2. Include complete XML documentation (`/// <summary>`, `/// <remarks>`, `/// <exception>`, etc.)
3. Create corresponding unit tests before or immediately after
4. Update README.md if adding a new functional area

### When Adding Extension Methods
1. Create or add to the appropriate `ExtensionMethods-System-<Type>.cs` file
2. Use `public static partial class ExtensionMethods`
3. Include the `#pragma warning disable IDE0130` directive
4. Add XML documentation explaining the extension's purpose and usage

### Generic Advice
- Do not use catch blocks without rethrowing or logging
- Do not suppress compiler warnings except where documented (like IDE0130)
- Do not leave code without unit tests if adding public API
- Do not forget strong-name signing for release builds

## Key Files & References

- `Arkane.Core.csproj` – Project configuration, signing, and package metadata
- `Arkane.Core/` – Main library implementation
- `Arkane.Core.UnitTests/` – MSTest suite
- `README.md` – User-facing overview of library contents
- `docs/` – Markdown documentation for each component
- `.github/agents/docs.agent.md` – Role definition for documentation agent

## Constraints & Guardrails

- ✅ **Do:** Implement interfaces in namespace `ArkaneSystems.Arkane`
- ✅ **Do:** Use strict nullability and nullable reference types
- ✅ **Do:** Write XML docs for all public members
- ✅ **Do:** Test public APIs thoroughly
- 🚫 **Don't:** Modify strong-name key or assembly GUID
- 🚫 **Don't:** Violate the "no both copy interfaces" rule
- 🚫 **Don't:** Add public APIs without `[PublicAPI]` attribute
- 🚫 **Don't:** Leave warnings in a clean build

## Specific Components

### Copy Interfaces Pattern
- **`IDeepCopy<T>`** and **`IShallowCopy<T>`** define copy semantics
  - **Critical Rule:** Never implement both on the same class
  - See `Arkane.Core/IDeepCopy.cs` and `Arkane.Core/IShallowCopy.cs` for contract specs
  - Base implementations: `DeepCopiableObject` and `ShallowCopiableObject`

#### When Implementing Copy Semantics
- Choose **exactly one**: `IDeepCopy<T>` or `IShallowCopy<T>`
- Document the copy semantics clearly in remarks

### RandomProvider (Thread-Safe Singleton)
- **Design:** Per-thread `ThreadLocal<Random>` to avoid repeated values from synchronized seeding
- **Usage:** `RandomProvider.GetInstance()` returns thread-local instance
- **Key:** No locking required; safe on multiple threads
- See `Arkane.Core/RandomProvider.cs`

### Disposal Patterns
- **`DisposerBase<T>`** wraps objects lacking `IDisposable`, managing their lifecycle
- **`IDisposable<out T>`** extends `System.IDisposable` with a covariant `Value` property
- Proper finalizer + disposal state tracking via `IsDisposed` field
- See `Arkane.Core/DisposerBase.cs` and `Arkane.Core/IDisposable.cs`
