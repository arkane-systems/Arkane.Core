#region header

// Arkane.Core - IDeepCopy.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-02-12 4:01 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Supports deep copying, which creates a new instance of a class whose fields reference new copies of the original
///   object's fields.
/// </summary>
/// <typeparam name="T">The type of the deep-copyable object.</typeparam>
/// <remarks>
///   <para>
///     DO NOT implement this on the same class as <see cref="IShallowCopy{T}" />.
///   </para>
///   <para>
///     <see cref="ICloneable.Clone()" /> must be implemented to call the <see cref="IDeepCopy{T}.DeepCopy()" />
///     method.
///   </para>
/// </remarks>
[PublicAPI]
public interface IDeepCopy<out T> : ICloneable
{
  #region Implementation of ICloneable

  /// <inheritdoc />
  object ICloneable.Clone () => this.DeepCopy () ?? throw new NullReferenceException ();

  #endregion

  /// <summary>
  ///   Creates a deep copy of an object, a new instance of the class whose fields reference new copies of the original
  ///   object's fields.
  /// </summary>
  /// <returns>A deep copy of the object.</returns>
  T DeepCopy ();
}
