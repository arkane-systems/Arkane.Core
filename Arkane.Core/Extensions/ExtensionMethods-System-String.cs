#region header

// Arkane.Core - ExtensionMethods-System-String.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-02 1:00 PM

#endregion

#region using

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>

// This part of the extension methods class is reserved for extension methods on System.String.
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  extension (string @this)
  {
    /// <summary>
    ///   Returns a new string containing only the alphanumeric characters (<c>[A-Za-z0-9]</c>) from the original string.
    /// </summary>
    /// <returns>A string consisting solely of the alphanumeric characters found in the original string.</returns>
    [PublicAPI]
    public string RemoveNonAlphanumeric ()
    {
      MatchCollection matchCollection = AlphanumericRegex ().Matches (input: @this);
      string          result          = string.Concat (matchCollection.Select (static m => m.Value));

      return result;
    }

    /// <summary>
    ///   Returns at most <paramref name="maxLength" /> characters of the string, appending
    ///   <paramref name="suffixToUseWhenTooLong" /> when the string is truncated.
    /// </summary>
    /// <param name="maxLength">The maximum number of characters to return before the suffix.</param>
    /// <param name="suffixToUseWhenTooLong">
    ///   The suffix appended when the string exceeds <paramref name="maxLength" /> characters.
    ///   Defaults to <c>"..."</c>.
    /// </param>
    /// <returns>
    ///   The original string if it is no longer than <paramref name="maxLength" /> characters; otherwise the
    ///   first <paramref name="maxLength" /> characters followed by <paramref name="suffixToUseWhenTooLong" />.
    /// </returns>
    [PublicAPI]
    public string UpTo (int maxLength, string suffixToUseWhenTooLong = "...")
    {
      if (string.IsNullOrEmpty (@this))
        return string.Empty;

      return @this.Length > maxLength
               ? string.Concat (str0: @this.AsSpan (start: 0, length: maxLength), str1: suffixToUseWhenTooLong)
               : @this;
    }

    #region Encryption

    // Paired with DecryptWithAes in ExtensionMethods-System-ByteArray.cs. The IV is prepended to the
    // output of this method, so the decryption method can extract it for use in decryption.
    [PublicAPI]
    public byte[] EncryptWithAes (byte[] key)
    {
      using var aes = Aes.Create ();
      aes.Key = key;

      using var memStream = new MemoryStream ();
      memStream.Write (buffer: aes.IV, offset: 0, count: aes.IV.Length); // Prepend IV to the output

      using var cryptoStream =
        new CryptoStream (stream: memStream, transform: aes.CreateEncryptor (), mode: CryptoStreamMode.Write);

      byte[] stringBytes = Encoding.UTF8.GetBytes (@this);
      cryptoStream.Write (buffer: stringBytes, offset: 0, count: stringBytes.Length);
      cryptoStream.FlushFinalBlock ();

      memStream.Position = 0;

      return memStream.ToArray ();
    }

    #endregion

    #region Hashing

    [PublicAPI]
    public byte[] GenerateMd5Hash ()
    {
      byte[] stringBytes = Encoding.UTF8.GetBytes (@this);

      using var hash = MD5.Create ();

      return hash.ComputeHash (stringBytes);
    }

    /// <summary>
    ///   Computes an MD5 hash for the current string after prefixing it with the specified <paramref name="salt" />.
    /// </summary>
    /// <param name="salt">The salt bytes to prepend to the UTF-8 bytes of the current string before hashing.</param>
    /// <returns>The 16-byte MD5 hash of the salted string.</returns>
    /// <remarks>
    ///   <p>
    ///     This method is suitable for compatibility scenarios. For stronger cryptographic resistance, prefer
    ///     <see cref="GenerateSha256Hash(string,byte[])" />.
    ///   </p>
    ///   <p>Do not use this function to hash passwords; use a dedicated password hashing algorithm instead.</p>
    /// </remarks>
    [PublicAPI]
    public byte[] GenerateMd5Hash (byte[] salt)
    {
      byte[] stringBytes = Encoding.UTF8.GetBytes (@this);

      var saltedString = new byte[stringBytes.Length + salt.Length];
      Buffer.BlockCopy (src: salt,        srcOffset: 0, dst: saltedString, dstOffset: 0,           count: salt.Length);
      Buffer.BlockCopy (src: stringBytes, srcOffset: 0, dst: saltedString, dstOffset: salt.Length, count: stringBytes.Length);

      using var hash = MD5.Create ();

      return hash.ComputeHash (saltedString);
    }

    [PublicAPI]
    public byte[] GenerateSha256Hash ()
    {
      byte[] stringBytes = Encoding.UTF8.GetBytes (@this);

      using var hash = SHA256.Create ();

      return hash.ComputeHash (stringBytes);
    }

    /// <summary>
    ///   Computes a SHA-256 hash for the current string after prefixing it with the specified <paramref name="salt" />.
    /// </summary>
    /// <param name="salt">The salt bytes to prepend to the UTF-8 bytes of the current string before hashing.</param>
    /// <returns>The 32-byte SHA-256 hash of the salted string.</returns>
    /// <remarks>
    ///   <p>
    ///     The hash input is constructed as <c>salt + UTF8(string)</c>, ensuring deterministic salted hashing for the same
    ///     string/salt pair.
    ///   </p>
    ///   <p>Do not use this function to hash passwords; use a dedicated password hashing algorithm instead.</p>
    /// </remarks>
    [PublicAPI]
    public byte[] GenerateSha256Hash (byte[] salt)
    {
      byte[] stringBytes = Encoding.UTF8.GetBytes (@this);

      var saltedString = new byte[stringBytes.Length + salt.Length];
      Buffer.BlockCopy (src: salt,        srcOffset: 0, dst: saltedString, dstOffset: 0,           count: salt.Length);
      Buffer.BlockCopy (src: stringBytes, srcOffset: 0, dst: saltedString, dstOffset: salt.Length, count: stringBytes.Length);

      using var hash = SHA256.Create ();

      return hash.ComputeHash (saltedString);
    }

    #endregion

    #region FillWith

    /// <summary>
    ///   Formats the string using one replacement value.
    /// </summary>
    /// <param name="arg0">The value to substitute for format item <c>{0}</c>.</param>
    /// <returns>The formatted string.</returns>
    [PublicAPI]
    [StringFormatMethod ("@this")]
    public string FillWith (object? arg0) => string.Format (format: @this, arg0: arg0);

    /// <summary>
    ///   Formats the string using two replacement values.
    /// </summary>
    /// <param name="arg0">The value to substitute for format item <c>{0}</c>.</param>
    /// <param name="arg1">The value to substitute for format item <c>{1}</c>.</param>
    /// <returns>The formatted string.</returns>
    [PublicAPI]
    [StringFormatMethod ("@this")]
    public string FillWith (object? arg0, object? arg1) => string.Format (format: @this, arg0: arg0, arg1: arg1);

    /// <summary>
    ///   Formats the string using three replacement values.
    /// </summary>
    /// <param name="arg0">The value to substitute for format item <c>{0}</c>.</param>
    /// <param name="arg1">The value to substitute for format item <c>{1}</c>.</param>
    /// <param name="arg2">The value to substitute for format item <c>{2}</c>.</param>
    /// <returns>The formatted string.</returns>
    [PublicAPI]
    [StringFormatMethod ("@this")]
    public string FillWith (object? arg0, object? arg1, object? arg2)
      => string.Format (format: @this, arg0: arg0, arg1: arg1, arg2: arg2);

    /// <summary>
    ///   Formats the string by substituting each format item with a corresponding value from <paramref name="args" />.
    /// </summary>
    /// <param name="args">The values to substitute into the format items in the string.</param>
    /// <returns>The formatted string.</returns>
    [PublicAPI]
    [StringFormatMethod ("@this")]
    public string FillWith (params object?[] args) => string.Format (format: @this, args: args);

    /// <summary>
    ///   Formats the string using the specified culture-specific formatting provider and replacement values.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="args">The values to substitute into the format items in the string.</param>
    /// <returns>The formatted string.</returns>
    [PublicAPI]
    [StringFormatMethod ("@this")]
    public string FillWith (IFormatProvider provider, params object?[] args)
      => string.Format (provider: provider, format: @this, args: args);

    #endregion
  }

  #endregion

  #region Supporting regexes for string extension methods

  [GeneratedRegex ("[A-Za-z0-9]+")]
  private static partial Regex AlphanumericRegex ();

  #endregion Supporting regexes for string extension methods
}
