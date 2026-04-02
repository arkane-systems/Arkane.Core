#region header

// Arkane.Core - EventArgs.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 6:49 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

[PublicAPI]
public class EventArgs<T> (T value) : EventArgs
{
  public T Value { get; } = value;
}
