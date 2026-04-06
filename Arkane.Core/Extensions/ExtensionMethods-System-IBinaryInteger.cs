#region header

// Arkane.Core - ExtensionMethods-System-IBinaryInteger.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 11:04 PM

#endregion

#region using

using System.Numerics;

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>

// This part of the extension methods class is reserved for extension methods on System.Numerics.IBinaryInteger<T>.
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  extension<T> (T @this) where T : IBinaryInteger<T>
  { }

  #endregion
}
