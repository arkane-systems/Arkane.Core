#region header

// Arkane.Core - ExtensionMethods-System-Array.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 10:31 AM

#endregion

#region using

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
  ///   Extension methods for <see cref="Array" />.  Note that these are not extension methods for
  ///   <see cref="System.Array" />, but rather for any array type, such as <c>int[]</c>, <c>string[,]</c>, etc.
  /// </summary>
  /// <param name="this">The array instance.</param>
  extension (Array @this)
  {
    /// <summary>
    ///   Gets the element type of the array.
    /// </summary>
    /// <returns>The element type.</returns>
    /// <remarks>
    ///   Drills down through multidimensional and/or nested arrays to get the element type.  For example, for an array of type
    ///   <c>int[,][,,]</c>, this method will return <c>int</c>.
    /// </remarks>
    [PublicAPI]
    public Type GetElementType ()
    {
      Type elementType = @this.GetType ().GetElementType () ??
                         throw new CannotHappenException (Resources
                                                           .Extension_Array_GetElementType_ArrayHadNoElementType);

      while (elementType.IsArray)
      {
        elementType = elementType.GetElementType () ??
                      throw new
                        CannotHappenException (Resources.Extension_Array_GetElementType_ArrayHadNoElementType);
      }

      return elementType;
    }
  }

  /// <summary>
  ///   Extension methods for nullable array types.
  /// </summary>
  /// <param name="this"></param>
  extension (Array? @this)
  {
    /// <summary>
    ///   Determines whether the specified array is null or has a length of zero.
    /// </summary>
    /// <returns>true if the array is null or empty; otherwise, false.</returns>
    [PublicAPI]
    public bool IsNullOrEmpty () => (@this == null) || (@this.Length == 0);
  }

  #endregion
}
