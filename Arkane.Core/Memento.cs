#region header

// Arkane.Core - Memento.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-06 9:13 AM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Remember an action to be performed on disposal; returned from a method needing later closure.
/// </summary>
[PublicAPI]
public sealed class Memento (Action action) : IDisposable
{
  private Action Action { get; init; } = action;

  private bool Disposed { get; set; }

  void IDisposable.Dispose ()
  {
    // TODO: Add assume contract that this.Disposed is false, and throw if not.
    this.Disposed = true;
    this.Action ();
  }
}
