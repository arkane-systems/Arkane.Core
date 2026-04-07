#region header

// Arkane.Core - ExtensionMethods-System-Guid.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-06 11:18 PM

#endregion

#region using

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
  ///   Extension methods for System.Guid.
  /// </summary>
  /// <param name="this">The Guid instance.</param>
  extension (Guid @this)
  {
    /// <summary>
    ///   Determines whether this is an empty GUID.
    /// </summary>
    /// <returns><c>true</c> if the GUID is empty; otherwise, <c>false</c>.</returns>
    [PublicAPI]
    public bool IsEmpty () => @this == Guid.Empty;
  }

  #endregion
}
