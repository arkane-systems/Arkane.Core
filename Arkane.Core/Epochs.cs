#region header

// Arkane.Core - Epochs.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-03 3:54 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Provides well-known epoch <see cref="DateTime" /> values for common operating systems and platforms.
/// </summary>
public static class Epochs
{
  /// <summary>
  ///   Gets the Unix/Linux epoch: midnight UTC on 1 January 1970.
  /// </summary>
  [PublicAPI]
  public static DateTime LinuxEpoch { get; } = new (year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, millisecond: 0);

  /// <summary>
  ///   Gets the classic Mac OS epoch: midnight on 1 January 1904.
  /// </summary>
  [PublicAPI]
  public static DateTime MacEpoch { get; } = new (year: 1904, month: 1, day: 1, hour: 0, minute: 0, second: 0, millisecond: 0);

  /// <summary>
  ///   Gets the Windows/.NET epoch: midnight on 1 January 0001.
  /// </summary>
  [PublicAPI]
  public static DateTime WindowsEpoch { get; } =
    new (year: 0001, month: 1, day: 1, hour: 0, minute: 0, second: 0, millisecond: 0);
}
