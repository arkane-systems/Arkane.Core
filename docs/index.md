# Arkane.Core — Overview

Arkane.Core is the foundational library in the Arkane Systems base class library (BCL). Targeting .NET 10, it provides a small set of lightweight, reusable abstractions for other Arkane libraries and consumer applications to build upon.

## Namespace

All public types live under the `ArkaneSystems.Arkane` namespace. Annotation types live under `ArkaneSystems.Arkane.Annotations`.

## What's Included

| Area | Types | Description |
|------|-------|-------------|
| [Copy interfaces](copy-interfaces.md) | `IDeepCopy<T>`, `IShallowCopy<T>` | Strongly-typed contracts for deep and shallow copying |
| [Random numbers](random-provider.md) | `RandomProvider` | Thread-safe per-thread `System.Random` singleton |
| [Annotations](annotations.md) | `PublicAPIAttribute`, `UsedImplicitlyAttribute`, `MeansImplicitUseAttribute` | Static analysis hints |

## Package

| Property | Value |
|----------|-------|
| Package ID | `Arkane.Core` |
| Version | 0.1.0 |
| License | MIT |
| Target framework | .NET 10 |
| Strong-named | Yes |
