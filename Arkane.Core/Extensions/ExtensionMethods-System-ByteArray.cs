#region header

// Arkane.Core - ExtensionMethods-System-ByteArray.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 5:53 PM

#endregion

#region using

using System.Security.Cryptography;
using System.Text;

using ArkaneSystems.Arkane.Properties;

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
  ///   Extension methods for byte arrays.
  /// </summary>
  /// <param name="this">The byte array instance.</param>
  extension (byte[] @this)
  {
    /// <summary>
    ///   Creates a new <see cref="T:System.IO.MemoryStream" /> using this array as the initial buffer.
    /// </summary>
    /// <param name="writable">The setting of the CanWrite property, which determines whether the stream supports writing.</param>
    /// <returns>A MemoryStream based upon this byte array.</returns>
    [PublicAPI]
    public MemoryStream AsMemoryStream (bool writable = true) => new (buffer: @this, writable: writable);

    #region Decryption

    /// <summary>
    ///   Decrypts the current AES-encrypted byte array using the provided <paramref name="key" />.
    ///   The byte array is expected to begin with the AES IV followed by the ciphertext, as produced by
    ///   <c>EncryptWithAes</c>.
    /// </summary>
    /// <param name="key">The AES decryption key. Must match the key used during encryption.</param>
    /// <returns>The decrypted string, decoded from UTF-8.</returns>
    /// <exception cref="ArgumentException">
    ///   Thrown when the byte array is shorter than the expected AES IV length.
    /// </exception>
    [PublicAPI]
    public string DecryptWithAes (byte[] key)
    {
      using var memStream = new MemoryStream ();
      memStream.Write (@this);
      memStream.Position = 0;

      using var aes = Aes.Create ();

      int ivLength = aes.IV.Length;

      if (@this.Length < ivLength)
        throw new ArgumentException (message: Resources.Extension_ByteArray_DecryptWithAes_EncryptedPayloadShorterThanAesIv,
                                     paramName: nameof (@this));

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
