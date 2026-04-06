#region header

// Arkane.Core - ExtensionMethods-System-String.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 5:53 PM

#endregion

#region using

using System.Globalization;
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
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  /// <summary>
  ///   Extension methods for nullable strings. These are separate from the non-nullable string extensions to allow for more
  ///   precise nullability annotations and to avoid ambiguity when calling extension methods on nullable string instances.
  /// </summary>
  /// <param name="this">The nullable string instance.</param>
  extension (string? @this)
  {
    /// <summary>
    ///   Safely denullify a nullable string. Nulls become String.Empty.
    /// </summary>
    /// <returns>The original string, or String.Empty if null.</returns>
    [PublicAPI]
    [ContractAnnotation ("null=>notnull")]
    public string Safe () => @this ?? string.Empty;
  }

  /// <summary>
  ///   Extension methods for System.String.
  /// </summary>
  /// <param name="this">The string instance.</param>
  extension (string @this)
  {
    /// <summary>
    ///   Perform a disemvowelment on a string, removing all lowercase and uppercase vowels ('a', 'e', 'i', 'o', 'u', 'A', 'E',
    ///   'I', 'O', 'U') from the original string.
    /// </summary>
    /// <returns>
    ///   A new string with all lowercase and uppercase vowels ('a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U') removed
    ///   from the original string.
    /// </returns>
    [PublicAPI]
    public string Disemvowel () => @this.Remove ('a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U');

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

    /// <summary>
    ///   Encrypts the current string using AES with the provided <paramref name="key" />, prepending the
    ///   randomly-generated IV to the returned byte array.
    /// </summary>
    /// <param name="key">The AES encryption key. Must be 16, 24, or 32 bytes (128, 192, or 256 bits).</param>
    /// <returns>
    ///   A byte array containing the AES IV followed by the AES-encrypted UTF-8 bytes of the current string.
    ///   Pass this value to <c>DecryptWithAes</c> along with the same key to recover the original string.
    /// </returns>
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

    #region HasValue

    /// <summary>
    ///   Test whether a string has a value; i.e., is not null, or empty.
    /// </summary>
    /// <returns>True if the string is neither null or empty; false otherwise.</returns>
    [PublicAPI]
    [Pure]
    [ContractAnnotation ("null=>false")]
    public bool HasValue () => !string.IsNullOrEmpty (@this);

    /// <summary>
    ///   Test whether a string has a value; i.e., is not null, empty, or composed solely of whitespace.
    /// </summary>
    /// <returns>
    ///   True if the string is not null, not empty, and not composed entirely of whitespace
    ///   characters; false otherwise.
    /// </returns>
    [PublicAPI]
    [Pure]
    [ContractAnnotation ("null=>false")]
    public bool HasNonWhiteSpaceValue () => !string.IsNullOrWhiteSpace (@this);

    #endregion

    #region AsNullIf

    /// <summary>
    ///   Returns null if the current string empty; otherwise, returns the original string.
    /// </summary>
    /// <returns>A null reference if the string is empty; otherwise, the original string.</returns>
    [PublicAPI]
    public string? AsNullIfEmpty () => string.IsNullOrEmpty (@this) ? null : @this;

    /// <summary>
    ///   Returns null if the current string is empty, or consists only of white-space characters; otherwise,
    ///   returns the original string.
    /// </summary>
    /// <returns>
    ///   A null reference if the string is empty, or contains only white-space characters; otherwise, the original
    ///   string.
    /// </returns>
    [PublicAPI]
    public string? AsNullIfWhitespace () => string.IsNullOrWhiteSpace (@this) ? null : @this;

    #endregion

    #region Remove

    /// <summary>
    ///   Remove any instances of the given character(s) from the string.
    /// </summary>
    /// <param name="toRemove">The character(s) to remove.</param>
    /// <returns>The string with the specified characters removed.</returns>
    [PublicAPI]
    public string Remove (params char[] toRemove)
    {
      string result = @this;
      foreach (char c in toRemove)
        result = result.Replace (oldValue: c.ToString (), newValue: string.Empty);

      return result;
    }

    /// <summary>
    ///   Remove any instance of the given string from the string.
    /// </summary>
    /// <param name="toRemove">The string(s) to remove.</param>
    /// <returns>The string with the specified substrings removed.</returns>
    [PublicAPI]
    public string Remove (params string[] toRemove)
    {
      string result = @this;
      foreach (string s in toRemove)
        result = result.Replace (oldValue: s, newValue: string.Empty);

      return result;
    }

    #endregion

    #region Conversions

    /// <summary>
    ///   Attempts to parse the current string as a <see cref="DateTime" /> using a set of supported date formats,
    ///   returning <paramref name="defaultValue" /> when the string is <see langword="null" />, empty, or cannot
    ///   be parsed.
    /// </summary>
    /// <param name="defaultValue">The value to return when parsing fails. Defaults to <see cref="DateTime.MinValue" />.</param>
    /// <returns>
    ///   The parsed <see cref="DateTime" /> in UTC if successful; otherwise <paramref name="defaultValue" />.
    /// </returns>
    [PublicAPI]
    public DateTime AsDateTime (DateTime defaultValue = default)
      => string.IsNullOrWhiteSpace (@this) ||
         !DateTime.TryParseExact (s: @this,
                                  formats: DateFormats,
                                  provider: CultureInfo.InvariantCulture,
                                  style: DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                                  result: out DateTime result)
           ? defaultValue
           : result;

    /// <summary>
    ///   Parses a string into an enum. The parsing is case-insensitive by default, and returns
    ///   <paramref name="defaultValue" /> if the string is null, empty, or cannot be parsed into the enum type.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="ignoreCase">Indicates whether to ignore case when parsing.</param>
    /// <param name="defaultValue">The default value to return if parsing fails.</param>
    /// <returns>The parsed enum value, or the default value if parsing fails.</returns>
    [PublicAPI]
    public T AsEnum<T> (bool ignoreCase = true, T defaultValue = default) where T : struct, Enum
      => string.IsNullOrWhiteSpace (@this) ||
         !Enum.TryParse (value: @this, ignoreCase: ignoreCase, result: out T result)
           ? defaultValue
           : result;

    #endregion

    #region Hashing

    /// <summary>
    ///   Computes the MD5 hash of the current string's UTF-8 byte representation.
    /// </summary>
    /// <returns>The 16-byte MD5 hash of the current string.</returns>
    /// <remarks>Do not use this function to hash passwords; use a dedicated password hashing algorithm instead.</remarks>
    [PublicAPI]
    public byte[] GenerateMd5Hash ()
    {
      byte[] stringBytes = Encoding.UTF8.GetBytes (@this);

      return MD5.HashData (stringBytes);
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

      return MD5.HashData (saltedString);
    }

    /// <summary>
    ///   Computes the SHA-256 hash of the current string's UTF-8 byte representation.
    /// </summary>
    /// <returns>The 32-byte SHA-256 hash of the current string.</returns>
    /// <remarks>Do not use this function to hash passwords; use a dedicated password hashing algorithm instead.</remarks>
    [PublicAPI]
    public byte[] GenerateSha256Hash ()
    {
      byte[] stringBytes = Encoding.UTF8.GetBytes (@this);

      return SHA256.HashData (stringBytes);
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

      return SHA256.HashData (saltedString);
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

  #region Supporting data for string extension methods

  // The date formats supported by AsDateTime. These are tried in order until one matches the input string.
  private static readonly string[] DateFormats =
  [
    "ddd MMM dd HH:mm:ss %zzzz yyyy",
    "yyyy-MM-dd\\THH:mm:ss.000Z",
    "yyyy-MM-dd\\THH:mm:ss\\Z",
    "yyyy-MM-dd HH:mm:ss",
    "yyyy-MM-dd HH:mm",
  ];

  // Matches sequences of one or more alphanumeric characters (letters and digits). Used by RemoveNonAlphanumeric.
  [GeneratedRegex ("[A-Za-z0-9]+")]
  private static partial Regex AlphanumericRegex ();

  #endregion
}
