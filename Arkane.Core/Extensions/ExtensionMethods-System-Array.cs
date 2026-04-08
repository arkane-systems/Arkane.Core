#region header

// Arkane.Core - ExtensionMethods-System-Array.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-06 6:26 PM

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

  #endregion
}
