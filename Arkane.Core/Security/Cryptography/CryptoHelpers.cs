#region header

// Arkane.Core - CryptoHelpers.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 3:27 PM

#endregion

#region using

using System.Security.Cryptography;

using ArkaneSystems.Arkane.Aspects;

using JetBrains.Annotations;

using Metalama.Patterns.Contracts;

#endregion

namespace ArkaneSystems.Arkane.Security.Cryptography;

/// <summary>
///   Provides helper methods for generating cryptographic keys and other cryptographic primitives.
/// </summary>
[PublicAPI]
public static class CryptoHelpers
{
  /// <summary>
  ///   Generates a cryptographically random AES key of the specified size.
  /// </summary>
  /// <param name="bits">
  ///   The key size in bits. Must be a positive multiple of 8, and at least 8.
  ///   Common AES key sizes are 128, 192, and 256 bits.
  /// </param>
  /// <returns>
  ///   A byte array of length <c><paramref name="bits" /> / 8</c> filled with cryptographically random data.
  /// </returns>
  [PublicAPI]
  public static byte[] GenerateKey ([NonNegative] [GreaterThanOrEqual (8)] [DivisibleBy (8)] int bits)
  {
    int keyLength = bits / 8; // convert bits to bytes

    var key = new byte[keyLength];
    RandomNumberGenerator.Fill (key);

    return key;
  }
}
