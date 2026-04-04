#region header

// Scratchpad - Program.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 6:04 PM

#endregion

#region using

using ArkaneSystems.Arkane.Security.Cryptography;

using JetBrains.Annotations;

#endregion

byte[] data = CryptoHelpers.GenerateKey (32);
Console.WriteLine (data.Length);
