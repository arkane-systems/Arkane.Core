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

/// <summary>
///   A unit-like value type with no state, useful as a placeholder type parameter or as the payload
///   of a <see cref="EventArgs{T}" /> when no data needs to be conveyed.
///   All instances are considered equal.
/// </summary>
[PublicAPI]
[StructLayout (LayoutKind.Sequential, Size = 1)]
public struct Empty : IEquatable<Empty>
{
  #pragma warning disable IDE0251

  /// <inheritdoc />
  public bool Equals (Empty other) => true;

  /// <inheritdoc />
  public override bool Equals (object? obj) => obj is Empty;

  /// <inheritdoc />
  public override int GetHashCode () => typeof (Empty).GetHashCode ();

  /// <summary>Returns the string representation of this instance.</summary>
  /// <returns>The literal string <c>"&lt;empty&gt;"</c>.</returns>
  public override string ToString () => "<empty>";

  #pragma warning restore IDE0251

  // ReSharper disable UnusedParameter.Global

  /// <summary>Determines whether two <see cref="Empty" /> values are equal (always <see langword="true" />).</summary>
  /// <param name="_1">The left-hand operand (ignored).</param>
  /// <param name="_2">The right-hand operand (ignored).</param>
  /// <returns>Always <see langword="true" />.</returns>
  public static bool operator == (Empty _1, Empty _2) => true;

  /// <summary>Determines whether two <see cref="Empty" /> values are not equal (always <see langword="false" />).</summary>
  /// <param name="_1">The left-hand operand (ignored).</param>
  /// <param name="_2">The right-hand operand (ignored).</param>
  /// <returns>Always <see langword="false" />.</returns>
  public static bool operator != (Empty _1, Empty _2) => false;

  // ReSharper restore UnusedParameter.Global
}
