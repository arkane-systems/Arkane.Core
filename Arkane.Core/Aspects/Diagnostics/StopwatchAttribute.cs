#region header

// Arkane.Core - StopwatchAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 10:31 AM

#endregion

#region using

using System.Diagnostics;

using JetBrains.Annotations;

using Metalama.Framework.Aspects;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Diagnostics;

[PublicAPI]
[CLSCompliant (false)]
public class StopwatchAttribute : OverrideMethodAspect
{
  #region Overrides of OverrideMethodAspect

  /// <inheritdoc />
  public override dynamic? OverrideMethod ()
  {
    var stopWatch = Stopwatch.StartNew ();

    try { return meta.Proceed (); }
    finally
    {
      stopWatch.Stop ();

      Trace.WriteLine (string.Format (format: AspectResources.StopwatchAttribute_OverrideMethod_MethodExecutedIn,
                                      arg0: meta.Target.Method.Name,
                                      arg1: stopWatch.ElapsedMilliseconds));
    }
  }

  #endregion
}
