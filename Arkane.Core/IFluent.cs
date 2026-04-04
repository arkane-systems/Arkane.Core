#region header

// Arkane.Core - IFluent.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 1:58 PM

#endregion

#region using

using System.ComponentModel;

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Marker interface for fluent APIs, designed to hide undesirable members from IntelliSense and to provide a clear
///   indication of the intended usage of the implementing class. By implementing this interface, a class can signal that
///   it is part of a fluent API, allowing developers to easily identify and utilize the fluent interface pattern while
///   coding.
/// </summary>
[PublicAPI]
public interface IFluent
{
  /// <inheritdoc cref="object.Equals(object?)" />
  [EditorBrowsable (EditorBrowsableState.Never)]
  bool Equals (object? obj);

  /// <inheritdoc cref="object.GetHashCode()" />
  [EditorBrowsable (EditorBrowsableState.Never)]
  int GetHashCode ();

  /// <inheritdoc cref="object.ToString()" />
  [EditorBrowsable (EditorBrowsableState.Never)]
  string ToString ();

  /// <inheritdoc cref="object.GetType()" />
  [EditorBrowsable (EditorBrowsableState.Never)]
  Type GetType ();
}
