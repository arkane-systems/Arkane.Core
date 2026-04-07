#region header

// Arkane.Core - LogMethodCallsAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 9:23 AM

#endregion

#region using

using JetBrains.Annotations;

using Metalama.Framework.Aspects;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Diagnostics;

// TODO: Fill this out over the next while and integrate it with our forthcoming built-in logging service.
[PublicAPI]
[CLSCompliant (false)]
public class LogMethodCallsAttribute : OverrideMethodAspect
{
  #region Overrides of OverrideMethodAspect

  /// <inheritdoc />
  public override dynamic? OverrideMethod ()
  {
    Console.WriteLine ($"Entering {meta.Target.Method}");

    try
    {
      dynamic? result = meta.Proceed ();
      Console.WriteLine ($"Exiting {meta.Target.Method} with result {result}");

      return result;
    }
    catch (Exception ex)
    {
      Console.WriteLine ($"Exiting {meta.Target.Method} with exception {ex}");

      throw;
    }
  }

  #endregion
}
