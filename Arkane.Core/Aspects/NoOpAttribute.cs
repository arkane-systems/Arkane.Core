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

[PublicAPI]
[CLSCompliant (false)]
public sealed class NoOpAttribute : OverrideMethodAspect
{
  public override dynamic? OverrideMethod ()
  {
    object? returnValue = meta.Target.Method.ReturnType.ToType ().GetDefault ();

    return returnValue;
  }
}
