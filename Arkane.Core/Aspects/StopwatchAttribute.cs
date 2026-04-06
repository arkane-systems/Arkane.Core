#region header

// Arkane.Core - StopwatchAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 7:04 PM

#endregion

#region using

using System.Diagnostics;

using JetBrains.Annotations;

using Metalama.Framework.Aspects;

#endregion

namespace ArkaneSystems.Arkane.Aspects;

[PublicAPI]
[CLSCompliant (false)]
public class StopwatchAttribute : OverrideMethodAspect
{
  #region Overrides of OverrideMethodAspect

  /// <inheritdoc />
  public override dynamic? OverrideMethod ()
  {
    var stopWatch = Stopwatch.StartNew ();

    dynamic? returnValue = meta.Proceed ();

    stopWatch.Stop ();

    // TODO: remove this argument naming suppression
    // ReSharper disable ArgumentsStyleNamedExpression
    Trace.WriteLine (string.Format (AspectResources.StopwatchAttribute_OverrideMethod_MethodExecutedIn,
                                    meta.Target.Method.Name,
                                    stopWatch.ElapsedMilliseconds));

    // ReSharper restore ArgumentsStyleNamedExpression

    return returnValue;
  }

  #endregion
}
