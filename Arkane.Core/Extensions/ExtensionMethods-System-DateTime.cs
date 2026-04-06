#region header

// Arkane.Core - ExtensionMethods-System-DateTime.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 5:53 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  /// <summary>
  ///   Extension methods for System.DateTime.
  /// </summary>
  /// <param name="this">The DateTime instance.</param>
  extension (DateTime @this)
  {
    /// <summary>
    ///   Converts the current <see cref="DateTime" /> to a Unix/Linux timestamp expressed as milliseconds
    ///   since the Linux epoch (1 January 1970).
    /// </summary>
    /// <returns>
    ///   The number of milliseconds elapsed since the Linux epoch, rounded to the nearest whole millisecond.
    /// </returns>
    [PublicAPI]
    public double ToLinuxTimeStamp ()
    {
      double dotnetMilliseconds = TimeSpan.FromTicks (@this.Ticks).TotalMilliseconds;
      double linuxMilliseconds  = dotnetMilliseconds - EpochMillisecondDifference;

      return Math.Round (value: linuxMilliseconds, digits: 0, mode: MidpointRounding.AwayFromZero);
    }
  }

  /// <summary>
  ///   Static extension methods for System.DateTime.
  /// </summary>
  extension (DateTime)
  {
    /// <summary>
    ///   Converts a Unix/Linux timestamp (milliseconds since the Linux epoch) to a <see cref="DateTime" />.
    /// </summary>
    /// <param name="timestamp">The number of milliseconds elapsed since the Linux epoch (1 January 1970).</param>
    /// <returns>A <see cref="DateTime" /> corresponding to the specified <paramref name="timestamp" />.</returns>
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
