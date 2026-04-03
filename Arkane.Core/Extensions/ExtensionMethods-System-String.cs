#region header

// Arkane.Core - ExtensionMethods-System-String.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 1:00 PM

#endregion

#region using

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

    /// <summary>
    ///   Returns a new string containing only the alphanumeric characters (<c>[A-Za-z0-9]</c>) from the original string.
    /// </summary>
    /// <returns>A string consisting solely of the alphanumeric characters found in the original string.</returns>
    [PublicAPI]
    public string RemoveNonAlphanumeric ()
    {
      MatchCollection matchCollection = ExtensionMethods.AlphanumericRegex ().Matches (input: @this);
      string          result          = string.Concat (matchCollection.Select (static m => m.Value));

      return result;
    }
  }

  #endregion

  #region Supporting regexes for string extension methods

  [GeneratedRegex ("[A-Za-z0-9]+")]
  private static partial Regex AlphanumericRegex ();

  #endregion Supporting regexes for string extension methods
}
