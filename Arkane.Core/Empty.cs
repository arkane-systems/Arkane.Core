#region header

// Arkane.Core - Empty.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 6:07 PM

#endregion

#region using

using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

[PublicAPI]
[StructLayout (LayoutKind.Sequential, Size = 1)]
public struct Empty : IEquatable<Empty>
{
  #pragma warning disable IDE0251

  public bool Equals (Empty other) => true;

  public override bool Equals (object? obj) => obj is Empty;

  public override int GetHashCode () => typeof (Empty).GetHashCode ();

  public override string ToString () => "<empty>";

  #pragma warning restore IDE0251

  // ReSharper disable UnusedParameter.Global

  public static bool operator == (Empty _1, Empty _2) => true;

  public static bool operator != (Empty _1, Empty _2) => false;

  // ReSharper restore UnusedParameter.Global
}
