#region header

// Arkane.Core - ExtensionMethods-System-Numerics-INumber.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 11:02 PM

#endregion

#region using

using System.Numerics;

using ArkaneSystems.Arkane.Properties;

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  /// <summary>
  ///   Extension methods for System.Numerics.INumber.
  /// </summary>
  /// <typeparam name="T">The specific numeric type.</typeparam>
  /// <param name="this">The numeric instance.</param>
  extension<T> (T @this) where T : INumber<T>
  {
    [PublicAPI]
    public T Clamp (T min, T max)
    {
      if (min >= max)
        throw new ArgumentException (message: Resources.Extension_INumber_Clamp_MinMustBeLessThanMax, paramName: nameof (min));

      if (@this < min)
        return min;

      return @this > max ? max : @this;
    }
  }

  #endregion
}
