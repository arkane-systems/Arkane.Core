#region header

// Arkane.Core - ExtensionMethods-System-Math.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 6:37 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>

// This part of the extension methods class is reserved for extension methods on System.Math.
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  extension (Math)
  {
    #region Constants

    /// <summary>
    ///   The mathematical constant τ (tau), equal to 2π ≈ 6.283185307.
    ///   Represents one full turn in radians.
    /// </summary>
    [PublicAPI]
    public static double Tau => 2.0 * Math.PI;

    #endregion

    #region Angle conversion methods

    /// <summary>Converts an angle from radians to degrees.</summary>
    /// <param name="radians">The angle in radians to convert.</param>
    /// <returns>The equivalent angle expressed in degrees.</returns>
    [PublicAPI]
    public static double ToDegrees (double radians) => radians * (180.0 / Math.PI);

    /// <summary>Converts an angle from degrees to radians.</summary>
    /// <param name="degrees">The angle in degrees to convert.</param>
    /// <returns>The equivalent angle expressed in radians.</returns>
    [PublicAPI]
    public static double ToRadians (double degrees) => degrees * (Math.PI / 180.0);

    #endregion

    #region Greek-lettered constants

    #pragma warning disable IDE1006

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///   The mathematical constant π (pi) ≈ 3.14159265358979.
    ///   Alias for <see cref="Math.PI" />.
    /// </summary>
    /// <remarks>
    ///   Entirely gratuitous.
    /// </remarks>
    [PublicAPI]
    public static double π => Math.PI;

    // ReSharper disable once InconsistentNaming
    /// <summary>
    ///   The mathematical constant τ (tau) ≈ 6.28318530717959.
    ///   Alias for <see cref="Math.Tau" />.
    /// </summary>
    /// <remarks>
    ///   Entirely gratuitous.
    /// </remarks>
    [PublicAPI]
    public static double τ => Math.Tau;

    #pragma warning restore IDE1006

    #endregion
  }

  #endregion
}
