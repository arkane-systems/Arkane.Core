#region header

// Arkane.Core - GlobalFabric.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 10:31 AM

#endregion

#region using

using JetBrains.Annotations;

using Metalama.Framework.Fabrics;
using Metalama.Patterns.Contracts;

#endregion

namespace ArkaneSystems.Arkane;

[UsedImplicitly]
internal class GlobalFabric : ProjectFabric
{
  #region Overrides of ProjectFabric

  /// <inheritdoc />
  /// <remarks>
  ///   <para>
  ///     Configure the project to be more strict about nullability. This is important for catching potential issues early,
  ///     especially during development. In release builds, we can be more lenient and only check the public-facing API, as
  ///     internal code is less likely to be exposed to external consumers.
  ///   </para>
  /// </remarks>
  public override void AmendProject (IProjectAmender amender)
  {
#if DEBUG

    // When debugging, we want to be more strict about nullability, to catch potential issues early. In release builds,
    // we can be more lenient and only check the public-facing API.
    amender.VerifyNotNullableDeclarations (true);
#else
    amender.VerifyNotNullableDeclarations (false);
#endif
  }

  #endregion
}
