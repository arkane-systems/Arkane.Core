#region header

// Arkane.Core - IShallowCopy.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-02-12 3:57 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Supports shallow copying, which creates a new instance of a class whose fields reference the same objects as the
///   original object.
/// </summary>
/// <typeparam name="T">The type of the shallow-copyable object.</typeparam>
/// <remarks>
///   <para>
///     DO NOT implement this on the same class as <see cref="ICloneable.Clone" />.
///   </para>
///   <para>
///     <see cref="ICloneable" /> must be implemented to call the <see cref="IShallowCopy{T}" />
///     method.
///   </para>
/// </remarks>
[PublicAPI]
public interface IShallowCopy<out T> : ICloneable
{
  #region Implementation of ICloneable

  /// <inheritdoc />
  object ICloneable.Clone () => this.ShallowCopy () ?? throw new NullReferenceException ();

  #endregion

  /// <summary>
  ///   Creates a shallow copy of an object, a new instance of the class whose fields reference the same objects as the
  ///   original object.
  /// </summary>
  /// <returns>A shallow copy of the object.</returns>
  T ShallowCopy ();
}
