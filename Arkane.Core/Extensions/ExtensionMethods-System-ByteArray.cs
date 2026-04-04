#region header

// Arkane.Core - ExtensionMethods-System-Byte-Array.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 3:39 PM

#endregion

#region using

using System.Security.Cryptography;
using System.Text;

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>

// This part of the extension methods class is reserved for extension methods on System.Byte[].
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  extension (byte[] @this)
  {
    #region Decryption

    // Paired with the encryption method in ExtensionMethods-System-String.cs, this method decrypts a byte array
    // that was encrypted using AES. The byte array is expected to contain the IV followed by the ciphertext. The
    // method reads the IV from the beginning of the byte array, then uses it along with the provided key to create
    // a decryptor and read the plaintext from the crypto stream.
    [PublicAPI]
    public string DecryptWithAes (byte[] key)
    {
      using var memStream = new MemoryStream ();
      memStream.Write (@this);
      memStream.Position = 0;

      using var aes = Aes.Create ();

      int ivLength = aes.IV.Length;
      if (@this.Length < ivLength)
      {
        throw new ArgumentException ("The encrypted payload is shorter than the AES IV.", nameof (@this));
      }

      var iv = new byte[ivLength];
      memStream.ReadExactly (buffer: iv, offset: 0, count: iv.Length);

      using var cryptoStream = new CryptoStream (stream: memStream,
                                                 transform: aes.CreateDecryptor (rgbKey: key, rgbIV: iv),
                                                 mode: CryptoStreamMode.Read);
      using var plainTextStream = new MemoryStream ();
      cryptoStream.CopyTo (plainTextStream);

      return Encoding.UTF8.GetString (plainTextStream.ToArray ());
    }

    #endregion
  }

  #endregion
}
