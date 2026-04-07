#region header

// Arkane.Core - BreakAttributes.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 11:54 AM

#endregion

#region using

using System.Diagnostics;

using JetBrains.Annotations;

using Metalama.Framework.Aspects;

#endregion

namespace ArkaneSystems.Arkane.Aspects;

/// <summary>
///   Aspect attribute that triggers a debugger break immediately before executing the target method body.
/// </summary>
/// <remarks>
///   This aspect is intended for diagnostic and troubleshooting scenarios where execution should pause
///   before method execution.
/// </remarks>
[PublicAPI]
[CLSCompliant (false)]
public class BreakBeforeAttribute : OverrideMethodAspect
{
  /// <summary>
  ///   Injects a debugger break before invoking the original target method implementation.
  /// </summary>
  /// <returns>
  ///   The value returned by the original target method.
  /// </returns>
  public override dynamic? OverrideMethod ()
  {
    Debugger.Break ();

    return meta.Proceed ();
  }
}

/// <summary>
///   Aspect attribute that triggers a debugger break immediately after executing the target method body.
/// </summary>
/// <remarks>
///   This aspect is intended for diagnostic and troubleshooting scenarios where execution should pause
///   after method completion and before returning to the caller.
/// </remarks>
[PublicAPI]
[CLSCompliant (false)]
public class BreakAfterAttribute : OverrideMethodAspect
{
  /// <summary>
  ///   Invokes the original target method implementation, then injects a debugger break before returning.
  /// </summary>
  /// <returns>
  ///   The value returned by the original target method.
  /// </returns>
  public override dynamic? OverrideMethod ()
  {
    try { return meta.Proceed (); }
    finally { Debugger.Break (); }
  }
}
