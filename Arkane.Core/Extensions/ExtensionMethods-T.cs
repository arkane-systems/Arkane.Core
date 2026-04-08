#region header

// Arkane.Core - ExtensionMethods-T.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-08 8:40 AM

#endregion

#region using

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
  ///   Extension methods for the generic type.
  /// </summary>
  /// <typeparam name="T">The generic type.</typeparam>
  /// <param name="this">The instance of the generic type.</param>
  extension<T> (T @this)
  {
    /// <summary>Swap the values of two references.</summary>
    /// <param name="first">The first reference to swap.</param>
    /// <param name="second">The second reference to swap.</param>
    [PublicAPI]
    public static void Swap (ref T first, ref T second) { (first, second) = (second, first); }
  }

  /// <summary>
  ///   Extension methods for the generic reference type.
  /// </summary>
  /// <typeparam name="T">The generic reference type.</typeparam>
  /// <param name="this">The instance of the generic reference type.</param>
  extension<T> (T @this) where T : class
  {
    /// <summary>
    ///   Perform a given action or sequence of actions on a specified object.
    /// </summary>
    /// <typeparam name="T">Type of the specified object.</typeparam>
    /// <param name="action">The action(s) to take.</param>
    /// <remarks>
    ///   <para>Intended usage:</para>
    ///   <para>
    ///     someVeryVeryLongVariableName.With(x =&gt; {
    ///     x.Int = 123;
    ///     x.Str = "Hello";
    ///     x.Str2 = " World!";
    ///     });
    ///   </para>
    ///   <para>This emulates the Visual Basic.NET With statement.</para>
    /// </remarks>
    /// <exception cref="T:System.Exception">A delegate callback throws an exception.</exception>
    [PublicAPI]
    public void With (Action<T> action) => action (@this);
  }

  #endregion
}
