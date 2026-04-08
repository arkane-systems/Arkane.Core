#region header

// Arkane.Core - ExtensionMethods-System-Type.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 6:49 PM

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
  ///   Extension methods for System.Type.
  /// </summary>
  /// <param name="this">The Type instance.</param>
  extension (Type @this)
  {
    /// <summary>
    ///   Retrieves the default value for this Type.
    /// </summary>
    /// <returns>The default value for this Type.</returns>
    /// <remarks>
    ///   If a reference Type or a System.Void Type is supplied, this method always returns null.  If a value type is
    ///   supplied which is not publicly visible or which contains generic parameters, this method will fail with an
    ///   exception.
    /// </remarks>
    [PublicAPI]
    public object? GetDefault ()
    {
      // If the Type is a reference type, or if the Type is a System.Void, return null.
      if (!@this.IsValueType || (@this == typeof (void)))
        return null;

      // If the supplied Type has generic parameters, its default value cannot be determined
      if (@this.ContainsGenericParameters)
        throw new
          ArgumentException ($"The supplied value type {@this} contains generic parameters, so the default value cannot be retrieved");

      // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct/enum), return a
      // default instance of the value type
      if (@this.IsPrimitive || !@this.IsNotPublic)
        try { return Activator.CreateInstance (@this); }
        catch (Exception e)
        {
          throw new ArgumentException (message:
                                       $"The Activator.CreateInstance method could not create a default instance of the supplied value type {@this}.",
                                       innerException: e);
        }

      // Fail with exception
      throw new
        ArgumentException ($"The supplied value type {@this} is not a publicly-visible type, so the default value cannot be retrieved");
    }

    /// <summary>
    ///   Determines whether this type derives from — or is itself — the specified open generic type.
    /// </summary>
    /// <param name="genericType">The open generic type definition to test against (e.g., <c>typeof(List&lt;&gt;)</c>).</param>
    /// <returns>
    ///   <see langword="true" /> if this type is, or inherits from, a closed construction of
    ///   <paramref name="genericType" />; otherwise <see langword="false" />.
    /// </returns>
    [PublicAPI]
    public bool IsDerivedFromGenericType (Type genericType)
    {
      if ((@this == typeof (object)) || @this.BaseType is null)
        return false;

      return (@this.IsGenericType && (@this.GetGenericTypeDefinition () == genericType)) ||
             @this.BaseType.IsDerivedFromGenericType (genericType);
    }

    /// <summary>
    ///   Reports whether a value of type T (or a null reference of type T) contains the default value for that Type.
    /// </summary>
    /// <remarks>
    ///   Reports whether the object is empty or uninitialized for a reference type or nullable value type (i.e. is null) or
    ///   whether the object contains a default value for a non-nullable value type (i.e. int = 0, bool = false, etc.)
    ///   <para></para>
    ///   NOTE: For non-nullable value types, this method introduces a boxing/unboxing performance penalty.
    /// </remarks>
    /// <param name="objectValue">The object value to test, or null for a reference Type or nullable value Type</param>
    /// <returns>
    ///   true = The object contains the default value for its Type.
    ///   <para></para>
    ///   false = The object has been changed from its default value.
    /// </returns>
    [PublicAPI]
    public bool IsObjectSetToDefault (object? objectValue)
    {
      // Get the default value of type ObjectType
      object? defaultType = @this.GetDefault ();

      // If a non-null ObjectValue was supplied, compare Value with its default value and return the result
      if (objectValue != null)
        return objectValue.Equals (defaultType);

      // Since a null ObjectValue was supplied, report whether its default value is null
      return defaultType == null;
    }

    #region GetAttributes

    /// <summary>
    ///   Retrieves the first custom attribute of the specified type applied to the current member, optionally searching
    ///   the inheritance chain.
    /// </summary>
    /// <remarks>
    ///   If multiple attributes of the specified type are applied, only the first is returned. This
    ///   method can be used to determine if a particular attribute is present and to access its data.
    /// </remarks>
    /// <typeparam name="T">The type of attribute to search for. Must derive from Attribute.</typeparam>
    /// <param name="includeInherited">
    ///   true to search the inheritance chain for the attribute; otherwise, false. The default is
    ///   true.
    /// </param>
    /// <returns>An instance of the specified attribute type if found; otherwise, null.</returns>
    [PublicAPI]
    public T? GetAttribute<T> (bool includeInherited = true) where T : Attribute
      => @this.GetAllAttributes<T> (includeInherited).FirstOrDefault ();

    /// <summary>
    ///   Retrieves the custom attributes of the specified type that are applied to this Type.
    /// </summary>
    /// <typeparam name="T">The type of the custom attributes to retrieve. Must derive from Attribute.</typeparam>
    /// <param name="includeInherited">
    ///   true to search this member's inheritance chain to find the attributes; otherwise, false.  The default is true.
    /// </param>
    /// <returns>
    ///   An enumeration of the custom attributes of type T that are applied to this Type.
    /// </returns>
    [PublicAPI]
    public IEnumerable<T> GetAllAttributes<T> (bool includeInherited = true) where T : Attribute
      => @this.GetCustomAttributes (attributeType: typeof (T), inherit: includeInherited).Cast<T> ();

    #endregion
  }

  #endregion
}
