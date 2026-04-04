#region header

// Arkane.Core - IDisposable.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-02 11:05 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Defines a disposable wrapper that exposes an underlying value.
/// </summary>
/// <typeparam name="T">
///   The type of value exposed by this disposable instance.
/// </typeparam>
/// <remarks>
///   This interface extends <see cref="System.IDisposable" /> and uses covariance
///   so implementations can be used where a less-derived <typeparamref name="T" /> is expected.
/// </remarks>
[PublicAPI]
public interface IDisposable<out T> : IDisposable
{
  T Value { get; }
}
