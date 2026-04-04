#region header

// Arkane.Core - ExtensionMethods-System-DateTime.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-03 3:34 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>

// This part of the extension methods class is reserved for extension methods on System.DateTime.
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  extension (DateTime @this)
  {
    [PublicAPI]
    public double ToLinuxTimeStamp ()
    {
      double dotnetMilliseconds = TimeSpan.FromTicks (@this.Ticks).TotalMilliseconds;
      double linuxMilliseconds  = dotnetMilliseconds - EpochMillisecondDifference;

      return Math.Round (value: linuxMilliseconds, digits: 0, mode: MidpointRounding.AwayFromZero);
    }
  }

  extension (DateTime)
  {
    [PublicAPI]
    public static DateTime FromLinuxTimeStamp (double timestamp) => Epochs.LinuxEpoch + TimeSpan.FromMilliseconds (timestamp);
  }

  #endregion

  #region Epochs

  // Used by the ToLinuxTimeStamp and FromLinuxTimeStamp methods.
  private static readonly double EpochMillisecondDifference =
    new TimeSpan (Epochs.LinuxEpoch.Ticks - Epochs.WindowsEpoch.Ticks).TotalMilliseconds;

  #endregion
}
