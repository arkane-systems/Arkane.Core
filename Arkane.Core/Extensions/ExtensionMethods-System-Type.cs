#region header

// Arkane.Core - ExtensionMethods-System-Type.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 1:25 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>

// This part of the extension methods class is reserved for extension methods on System.Type.
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  extension (Type @this)
  {
    [PublicAPI]
    public bool IsDerivedFromGenericType (Type genericType)
    {
      if (@this == typeof (object))
        return false;

      return (@this.IsGenericType && (@this.GetGenericTypeDefinition () == genericType)) ||
             @this.BaseType!.IsDerivedFromGenericType (genericType);
    }
  }

  #endregion
}
