#region header

// Arkane.Core.UnitTests - ExtensionMethods-System-ByteArray-AesTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 11:00 PM

#endregion

#region using

using System.Security.Cryptography;
using System.Text;

using ArkaneSystems.Arkane;
using ArkaneSystems.Arkane.Security.Cryptography;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests.Extensions;

[TestClass]
public class ExtensionMethodsSystemByteArrayAesTests
{
  [TestMethod]
  public void DecryptWithAes_RoundTrip_ReturnsOriginalString ()
  {
    // Arrange
    byte[] key       = CryptoHelpers.GenerateKey (bits: 256);
    const string original = "Hello, World!";

    // Act
    byte[] ciphertext = original.EncryptWithAes (key);
    string decrypted  = ciphertext.DecryptWithAes (key);

    // Assert
    Assert.AreEqual (expected: original, actual: decrypted);
  }

  [TestMethod]
  public void DecryptWithAes_RoundTrip_NonAsciiText_ReturnsOriginalString ()
  {
    // Arrange
    byte[] key       = CryptoHelpers.GenerateKey (bits: 128);
    const string original = "Ünïcödé tëxt: 日本語 العربية";

    // Act
    byte[] ciphertext = original.EncryptWithAes (key);
    string decrypted  = ciphertext.DecryptWithAes (key);

    // Assert
    Assert.AreEqual (expected: original, actual: decrypted);
  }

  [TestMethod]
  public void EncryptWithAes_TwoCalls_ProduceDifferentCiphertexts ()
  {
    // Arrange - random IV means same plaintext encrypts differently each call
    byte[] key       = CryptoHelpers.GenerateKey (bits: 256);
    const string original = "SamePlaintext";

    // Act
    byte[] ciphertext1 = original.EncryptWithAes (key);
    byte[] ciphertext2 = original.EncryptWithAes (key);

    // Assert
    CollectionAssert.AreNotEqual (notExpected: ciphertext1, actual: ciphertext2);
  }

  [TestMethod]
  public void DecryptWithAes_WhenPayloadIsShorterThanIv_ThrowsArgumentException ()
  {
    // Arrange
    byte[] key     = CryptoHelpers.GenerateKey (bits: 128);
    byte[] tooShort = new byte[4]; // AES IV is 16 bytes, so 4 bytes is too short

    // Act & Assert
    Assert.ThrowsExactly<ArgumentException> (() => tooShort.DecryptWithAes (key));
  }

  [TestMethod]
  public void DecryptWithAes_WhenKeyIsWrong_ThrowsCryptographicException ()
  {
    // Arrange
    byte[] key      = CryptoHelpers.GenerateKey (bits: 256);
    byte[] wrongKey = CryptoHelpers.GenerateKey (bits: 256);
    const string original = "SomeText";

    byte[] ciphertext = original.EncryptWithAes (key);

    // Act & Assert
    Assert.ThrowsExactly<CryptographicException> (() => ciphertext.DecryptWithAes (wrongKey));
  }
}
