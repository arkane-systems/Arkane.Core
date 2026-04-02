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

    [PublicAPI]
    public static double Tau => 2.0 * Math.PI;

    #endregion

    #region Angle conversion methods

    [PublicAPI]
    public static double ToDegrees (double radians) => radians * (180.0 / Math.PI);

    [PublicAPI]
    public static double ToRadians (double degrees) => degrees * (Math.PI / 180.0);

    #endregion

    #region Greek-lettered constants

    // ReSharper disable once InconsistentNaming
    [PublicAPI]
    public static double π => Math.PI;

    // ReSharper disable once InconsistentNaming
    [PublicAPI]
    public static double τ => Math.Tau;

    #endregion
  }

  #endregion
}
