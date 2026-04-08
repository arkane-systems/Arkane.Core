#region header

// Arkane.Core - ReferenceFabric.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-08 7:35 AM

#endregion

#region using

using JetBrains.Annotations;

using Metalama.Framework.Fabrics;
using Metalama.Patterns.Contracts;

#endregion

namespace ArkaneSystems.Arkane;

[UsedImplicitly]
internal class ReferenceFabric : TransitiveProjectFabric
{
  #region Overrides of ProjectFabric

  /// <summary>
  ///   Configures solution-wide nullability contract verification for this assembly.
  /// </summary>
  /// <param name="amender">The Metalama project amender used to apply contract policies.</param>
  /// <remarks>
  ///   In <c>DEBUG</c> builds, verification is applied to all declarations to catch issues earlier during development.
  ///   In non-debug builds, verification is limited to the public-facing API surface.
  /// </remarks>
  public override void AmendProject (IProjectAmender amender)
  {
#if DEBUG

    // Apply strict validation across the full codebase during development.
    amender.VerifyNotNullableDeclarations (true);
#else
    // Limit validation scope to public API for release-oriented builds.
    amender.VerifyNotNullableDeclarations (false);
#endif
  }

  #endregion
}
