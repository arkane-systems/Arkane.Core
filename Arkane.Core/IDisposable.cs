#region header

// Arkane.Core - IDisposable.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 11:05 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

[PublicAPI]
public interface IDisposable<out T> : IDisposable
{
  T Value { get; }
}
