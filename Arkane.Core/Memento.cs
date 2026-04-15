#region header

// Arkane.Core - Memento.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 10:27 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Remember an action to be performed on disposal; returned from a method needing later closure.
/// </summary>
/// <remarks>
///   Remember an action to be performed on disposal; returned from a method needing later closure.
/// </remarks>
[PublicAPI]
public sealed class Memento (Action action) : IDisposable
{
  private Action Action { get; init; } = action;

  private bool Disposed { get; set; }

  void IDisposable.Dispose ()
  {
    if (this.Disposed)
      return;

    this.Disposed = true;
    this.Action ();
  }
}
