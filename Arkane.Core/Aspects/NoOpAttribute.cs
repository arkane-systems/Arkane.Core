#region header

// Arkane.Core - NoOpAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 10:34 AM

#endregion

#region using

using JetBrains.Annotations;

using Metalama.Framework.Aspects;

#endregion

namespace ArkaneSystems.Arkane.Aspects;

/// <summary>
///   A Metalama aspect that replaces the target method body with a no-op, returning the default value for the
///   method's return type.
/// </summary>
[PublicAPI]
[CLSCompliant (false)]
public sealed class NoOpAttribute : OverrideMethodAspect
{
  /// <summary>
  ///   Replaces the target method's body with a no-op implementation that returns the default value for the
  ///   method's return type.
  /// </summary>
  /// <returns>
  ///   The default value for the method's return type, or <see langword="null" /> for reference types.
  /// </returns>
  public override dynamic? OverrideMethod ()
  {
    object? returnValue = meta.Target.Method.ReturnType.ToType ().GetDefault ();

    return returnValue;
  }
}
