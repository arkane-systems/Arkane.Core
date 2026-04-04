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

    [PublicAPI]
    public DateTime AsDateTime (string propertyName, DateTime defaultValue = default)
    {
      string? date = @this.GetStringProperty (propertyName);

      return date?.AsDateTime (defaultValue) ?? defaultValue;
    }

    #endregion

    [PublicAPI]
    public string? GetStringProperty (string propertyName, string? defaultValue = null)
    {
      if (@this.ValueKind == JsonValueKind.Object &&
          @this.TryGetProperty (propertyName: propertyName, value: out JsonElement element) &&
          element.ValueKind == JsonValueKind.String)
        return element.GetString () ?? defaultValue;

      return defaultValue;
    }

    [PublicAPI]
    public bool IsNull () => @this.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined;
  }

  #endregion
}
