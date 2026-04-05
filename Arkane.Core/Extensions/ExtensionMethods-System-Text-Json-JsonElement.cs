#region header

// Arkane.Core - ExtensionMethods-System-Text-Json-JsonElement.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 4:10 PM

#endregion

#region using

using System.Text.Json;

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>

// This part of the extension methods class is reserved for extension methods on System.Text.Json.JsonElement.
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  extension (JsonElement @this)
  {
    #region Conversions

    /// <summary>
    ///   Retrieves the string value of a named property from the JSON element and parses it as a
    ///   <see cref="DateTime" />. Returns <paramref name="defaultValue" /> if the property is missing or
    ///   cannot be parsed.
    /// </summary>
    /// <param name="propertyName">The name of the JSON property to retrieve and parse.</param>
    /// <param name="defaultValue">
    ///   The value to return when the property is absent or cannot be parsed as a <see cref="DateTime" />.
    ///   Defaults to <see cref="DateTime.MinValue" />.
    /// </param>
    /// <returns>
    ///   The parsed <see cref="DateTime" /> value, or <paramref name="defaultValue" /> if parsing fails.
    /// </returns>
    [PublicAPI]
    public DateTime AsDateTime (string propertyName, DateTime defaultValue = default)
    {
      string? date = @this.GetStringProperty (propertyName);

      return date?.AsDateTime (defaultValue) ?? defaultValue;
    }

    #endregion

    /// <summary>
    ///   Retrieves the string value of a named property from the JSON element, returning
    ///   <paramref name="defaultValue" /> if the element is not an object, the property does not exist,
    ///   or the property value is not a string.
    /// </summary>
    /// <param name="propertyName">The name of the JSON property to retrieve.</param>
    /// <param name="defaultValue">
    ///   The value to return when the property is missing or not a string. Defaults to <see langword="null" />.
    /// </param>
    /// <returns>The string value of the property, or <paramref name="defaultValue" /> if unavailable.</returns>
    [PublicAPI]
    public string? GetStringProperty (string propertyName, string? defaultValue = null)
    {
      if (@this.ValueKind == JsonValueKind.Object &&
          @this.TryGetProperty (propertyName: propertyName, value: out JsonElement element) &&
          element.ValueKind == JsonValueKind.String)
        return element.GetString () ?? defaultValue;

      return defaultValue;
    }

    /// <summary>
    ///   Determines whether the JSON element has a <see cref="JsonValueKind.Null" /> or
    ///   <see cref="JsonValueKind.Undefined" /> value kind.
    /// </summary>
    /// <returns>
    ///   <see langword="true" /> if the element's <see cref="JsonElement.ValueKind" /> is
    ///   <see cref="JsonValueKind.Null" /> or <see cref="JsonValueKind.Undefined" />; otherwise
    ///   <see langword="false" />.
    /// </returns>
    [PublicAPI]
    public bool IsNull () => @this.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined;
  }

  #endregion
}
