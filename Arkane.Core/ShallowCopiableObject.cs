#region header

// Arkane.Core - ShallowCopiableObject.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-02 6:17 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   A base class that provides a default shallow-copy implementation via <see cref="IShallowCopy{T}" />
///   and <see cref="ICloneable" />.
/// </summary>
[PublicAPI]
public class ShallowCopiableObject : IShallowCopy<ShallowCopiableObject>
{
  /// <inheritdoc />
  public ShallowCopiableObject ShallowCopy () => (ShallowCopiableObject)this.MemberwiseClone ();
}
