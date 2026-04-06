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
    public int ToBit () => !@this ? 0 : 1;
  }

  #endregion
}
