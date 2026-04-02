#region header

// Arkane.Core - RandomProvider.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-02-12 2:56 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Provide a per-thread singleton <see cref="System.Random" /> instance to avoid the problem of repeated values that
///   comes with initializing multiple instances before the default seed (current date and time) changes.
/// </summary>
/// <remarks>
///   Done per-thread to avoid the need for locking, since Random is not thread-safe.
/// </remarks>
[PublicAPI]
public class RandomProvider
{
  private static int seed = Environment.TickCount;

  private static readonly ThreadLocal<Random> RandomWrapper =
    new (() => new Random (Interlocked.Increment (ref RandomProvider.seed)));

  /// <summary>
  ///   Get the per-thread singleton instance of <see cref="System.Random" />, properly seeded.
  /// </summary>
  /// <returns>The per-thread singleton instance of <see cref="System.Random" />.</returns>
  /// <remarks>Thread-safe.</remarks>
  public static Random GetInstance () => RandomProvider.RandomWrapper.Value!;
}
