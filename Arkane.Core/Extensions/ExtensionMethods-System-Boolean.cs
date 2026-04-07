#region header

// Arkane.Core - ExtensionMethods-System-Boolean.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-06 9:50 AM

#endregion

#region using

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
  ///   Extension methods for System.Boolean.
  /// </summary>
  extension (bool @this)
  {
    /// <summary>
    ///   Converts the <see cref="T:System.Boolean" /> into number format; {1, 0}.
    /// </summary>
    /// <returns>The numeric value of the boolean.</returns>
    [PublicAPI]
    public int AsBit () => !@this ? 0 : 1;

    #region AsYesNoString

    /// <summary>
    ///   Converts the <see cref="T:System.Boolean" /> into string format; {Yes, No}.
    /// </summary>
    /// <returns>The string value of the boolean.</returns>
    /// <remarks>
    ///   This version is not localized, and is guaranteed to always return "Yes" or "No". Use
    ///   <see cref="M:ArkaneSystems.Arkane.ExtensionMethods.AsLocalizedYesNoString(System.Boolean)" /> if you want
    ///   localization support.
    /// </remarks>
    [PublicAPI]
    public string AsYesNoString () => !@this ? @"No" : @"Yes";

    /// <summary>
    ///   Converts the <see cref="T:System.Boolean" /> into string format; {Yes, No}.
    /// </summary>
    /// <returns>The string value of the boolean.</returns>
    /// <remarks>
    ///   This version is localized, and will return the localized equivalents of "Yes" and "No" based on the current culture.
    ///   Use <see cref="M:ArkaneSystems.Arkane.ExtensionMethods.AsYesNoString(System.Boolean)" /> if you want a non-localized
    ///   version that always returns "Yes" or "No".
    /// </remarks>
    [PublicAPI]
    public string AsLocalizedYesNoString ()
      => !@this ? Resources.Extension_Boolean_AsLocalizedYesNoString_No : Resources.Extension_Boolean_AsLocalizedYesNoString_Yes;

    #endregion
  }

  /// <summary>
  ///   Extension methods for nullable booleans (System.Boolean?).
  /// </summary>
  /// <param name="this">The nullable boolean to extend.</param>
  extension (bool? @this)
  {
    /// <summary>
    ///   Safely denullify a nullable bool. Nulls become false.
    /// </summary>
    /// <returns>The original boolean value, or false if null.</returns>
    [PublicAPI]
    public bool Safe () => @this ?? false;

    #region AsYesNoString

    /// <summary>
    ///   Converts the nullable boolean into string format; {Yes, No, null} or {Yes, No, Maybe}.
    /// </summary>
    /// <returns>The string value of the boolean, or null if it is null and <paramref name="maybe" /> is not true.</returns>
    /// <remarks>
    ///   This version is not localized, and is guaranteed to always return "Yes", "No", or "Maybe". Use
    ///   <see
    ///     cref="M:ArkaneSystems.Arkane.ExtensionMethods.AsLocalizedYesNoString(System.Nullable{System.Boolean},System.Boolean)" />
    ///   if you want localization support.
    /// </remarks>
    [PublicAPI]
    public string? AsYesNoString (bool maybe = false)
      => @this.HasValue
           ? !@this.Value ? @"No" : @"Yes"
           : maybe
             ? @"Maybe"
             : null;

    /// <summary>
    ///   Converts the nullable boolean into string format; {Yes, No, null} or {Yes, No, Maybe}.
    /// </summary>
    /// <returns>The string value of the boolean, or null if it is null and <paramref name="maybe" /> is not true.</returns>
    /// <remarks>
    ///   This version is localized, and will return the localized equivalents of "Yes", "No", or "Maybe" based on the current
    ///   culture.
    ///   Use
    ///   <see cref="M:ArkaneSystems.Arkane.ExtensionMethods.AsYesNoString(System.Nullable{System.Boolean},System.Boolean)" />
    ///   if you want a non-localized version that always returns "Yes", "No", or "Maybe".
    /// </remarks>
    [PublicAPI]
    public string? AsLocalizedYesNoString (bool maybe = false)
      => @this.HasValue
           ? !@this.Value
               ? Resources.Extension_Boolean_AsLocalizedYesNoString_No
               : Resources.Extension_Boolean_AsLocalizedYesNoString_Yes
           : maybe
             ? Resources.Extension_Boolean_AsLocalizedYesNoString_Maybe
             : null;

    #endregion
  }

  #endregion
}
