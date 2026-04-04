#region header

// Arkane.Core - HashHelpers.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 11:26 AM

#endregion

#region using

using System.Security.Cryptography;

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane.Security.Cryptography;

[PublicAPI]
public static class HashHelpers
{
  [PublicAPI]
  public static byte[] GenerateSalt ()
  {
    const int saltLength = 64; // 512 bits

    var salt = new byte[saltLength];
    RandomNumberGenerator.Fill (salt);

    return salt;
  }
}
