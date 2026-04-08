#region header

// Arkane.Core - RetryAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 9:27 AM

#endregion

#region using

using JetBrains.Annotations;

using Metalama.Framework.Aspects;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Resilience;

[PublicAPI]
[CLSCompliant (false)]
public class RetryAttribute : OverrideMethodAspect
{
  #pragma warning disable IDE0290
  public RetryAttribute (int maxRetries = 3, int delay = 100)
  {
    this.MaxRetries = maxRetries;
    this.Delay      = delay;
  }
  #pragma warning restore IDE0290

  public int MaxRetries { get; init; }

  public int Delay { get; init; }

  #region Overrides of OverrideMethodAspect

  /// <inheritdoc />
  public override dynamic? OverrideMethod ()
  {
    for (var attempt = 0;; attempt++)
    {
      try { return meta.Proceed (); }
      catch (Exception ex)
      {
        if (attempt >= this.MaxRetries)
          throw;

        Console.WriteLine (format: AspectResources.RetryAttribute_OverrideMethod_RetryingDueToException,
                           arg0: this.Delay,
                           arg1: ex.Message);
        Thread.Sleep (this.Delay);
      }
    }
  }

  #endregion
}
