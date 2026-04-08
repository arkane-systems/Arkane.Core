#region header

// Arkane.Core - ExtensionMethods-System-Numerics-IBinaryInteger.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 10:31 AM

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
  ///   Extension methods for System.Numerics.IBinaryInteger.
  /// </summary>
  /// <typeparam name="T">The specific binary integer type.</typeparam>
  /// <param name="this">The binary integer instance.</param>
  extension<T> (T @this) where T : IBinaryInteger<T>
  {
    /// <summary>Raise an integer to the power of.</summary>
    /// <param name="power">An integer that specifies a power.</param>
    /// <returns>The integer raised to the power.</returns>
    [PublicAPI]
    public T Exp ( /* [Positive] */ T power)
    {
      if (power < T.Zero)
        throw new ArgumentOutOfRangeException (paramName: nameof (power),
                                               message: Resources.Extension_IBinaryInteger_Exp_PowerMustBeNonNegative);

      T num = T.One;

      while (power > T.Zero)
      {
        if ((power & T.One) != T.Zero)
          num *= @this;

        power >>= 1;
        @this *=  @this;
      }

      return num;
    }

    /// <summary>Checks to see if this is an even number.</summary>
    /// <returns>True if even; false otherwise.</returns>
    [PublicAPI]
    public bool IsEven () => @this % T.CreateChecked (2) == T.Zero;

    /// <summary>Checks to see if this is an odd number.</summary>
    /// <returns>True if odd; false otherwise.</returns>
    [PublicAPI]
    public bool IsOdd () => @this % T.CreateChecked (2) == T.One;
  }

  #endregion
}
