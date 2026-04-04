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

using JetBrains.Annotations;

using Metalama.Patterns.Contracts;

#endregion

namespace ArkaneSystems.Arkane.Security.Cryptography;

[PublicAPI]
public static class CryptoHelpers
{
  [PublicAPI]
  public static byte[] GenerateKey ([NonNegative] [GreaterThanOrEqual (8)] int bits)
  {
    int keyLength = bits / 8; // convert bits to bytes

    var key = new byte[keyLength];
    RandomNumberGenerator.Fill (key);

    return key;
  }
}
