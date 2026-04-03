#region header

// Arkane.Core - DeepCopiableObject.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 6:19 PM

#endregion

#region using

using System.Collections.Concurrent;
using System.Reflection;

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   A base class that provides a recursive, reflection-based deep-copy implementation via
///   <see cref="IDeepCopy{T}" /> and <see cref="ICloneable" />.
///   Known immutable types (primitives, strings, <see cref="DateTime" />, <see cref="Guid" />, etc.) are
///   not cloned; object-graph cycles are handled by tracking already-copied references.
/// </summary>
[PublicAPI]
[Serializable]
public class DeepCopiableObject : IDeepCopy<DeepCopiableObject>
{
  private static readonly ConcurrentDictionary<Type, FieldInfo[]> CloneableFieldsCache = new ();

  private static readonly ConcurrentDictionary<Type, bool> ImmutableTypesCache = new ();

  private static readonly MethodInfo MemberwiseCloneMethod =
    typeof (object).GetMethod (name: "MemberwiseClone", bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic)!;

  /// <inheritdoc />
  public virtual DeepCopiableObject DeepCopy ()
    => (DeepCopiableObject)DeepCopiableObject.DeepClone (source: this,
                                                         copiedReferences: new Dictionary<object, object> (ReferenceEqualityComparer
                                                                                                            .Instance));

  /// <inheritdoc />
  public object Clone () => this.DeepCopy ();

  private static object DeepClone (object source, IDictionary<object, object> copiedReferences)
  {
    Type sourceType = source.GetType ();

    if (DeepCopiableObject.IsKnownImmutable (sourceType))
      return source;

    if (copiedReferences.TryGetValue (key: source, value: out object? existingCopy))
      return existingCopy;

    if (source is Array array)
      return DeepCopiableObject.CloneArray (source: array, copiedReferences: copiedReferences);

    object clone = DeepCopiableObject.MemberwiseCloneMethod.Invoke (obj: source, parameters: null)!;
    copiedReferences.Add (key: source, value: clone);

    foreach (FieldInfo field in DeepCopiableObject.GetCloneableFields (sourceType))
    {
      object? fieldValue = field.GetValue (obj: source);

      if (fieldValue is null)
        continue;

      if (DeepCopiableObject.IsKnownImmutable (field.FieldType))
        continue;

      object clonedFieldValue = DeepCopiableObject.DeepClone (source: fieldValue, copiedReferences: copiedReferences);
      field.SetValue (obj: clone, value: clonedFieldValue);
    }

    return clone;
  }

  private static Array CloneArray (Array source, IDictionary<object, object> copiedReferences)
  {
    var clone = (Array)source.Clone ();
    copiedReferences.Add (key: source, value: clone);

    Type? elementType = source.GetType ().GetElementType ();

    if (elementType is null || DeepCopiableObject.IsKnownImmutable (elementType))
      return clone;

    var indices = new int[source.Rank];
    DeepCopiableObject.CopyArrayElements (source: source,
                                          clone: clone,
                                          dimension: 0,
                                          indices: indices,
                                          copiedReferences: copiedReferences);

    return clone;
  }

  private static void CopyArrayElements (Array                       source,
                                         Array                       clone,
                                         int                         dimension,
                                         int[]                       indices,
                                         IDictionary<object, object> copiedReferences)
  {
    int lowerBound = source.GetLowerBound (dimension: dimension);
    int upperBound = source.GetUpperBound (dimension: dimension);

    for (int index = lowerBound; index <= upperBound; index++)
    {
      indices[dimension] = index;

      if (dimension < source.Rank - 1)
      {
        DeepCopiableObject.CopyArrayElements (source: source,
                                              clone: clone,
                                              dimension: dimension + 1,
                                              indices: indices,
                                              copiedReferences: copiedReferences);

        continue;
      }

      object? value = source.GetValue (indices);

      if (value is null)
        continue;

      if (DeepCopiableObject.IsKnownImmutable (value.GetType ()))
        continue;

      clone.SetValue (value: DeepCopiableObject.DeepClone (source: value, copiedReferences: copiedReferences), indices: indices);
    }
  }

  private static FieldInfo[] GetCloneableFields (Type type)
  {
    return DeepCopiableObject.CloneableFieldsCache.GetOrAdd (key: type,
                                                             valueFactory: static currentType =>
                                                                           {
                                                                             const BindingFlags bindingFlags =
                                                                               BindingFlags.Instance  |
                                                                               BindingFlags.Public    |
                                                                               BindingFlags.NonPublic |
                                                                               BindingFlags.DeclaredOnly;
                                                                             var   fields        = new List<FieldInfo> ();
                                                                             Type? typeToInspect = currentType;

                                                                             while (typeToInspect is not null)
                                                                             {
                                                                               fields.AddRange (collection: typeToInspect
                                                                                                           .GetFields (bindingAttr:
                                                                                                                       bindingFlags)
                                                                                                           .Where (predicate:
                                                                                                                   static field
                                                                                                                     => !field
                                                                                                                         .IsStatic));
                                                                               typeToInspect = typeToInspect.BaseType;
                                                                             }

                                                                             return [.. fields];
                                                                           });
  }

  private static bool IsKnownImmutable (Type type)
  {
    return DeepCopiableObject.ImmutableTypesCache.GetOrAdd (key: type,
                                                            valueFactory: static currentType
                                                                            => currentType.IsPrimitive                         ||
                                                                               currentType.IsEnum                              ||
                                                                               (currentType == typeof (string))                ||
                                                                               (currentType == typeof (decimal))               ||
                                                                               (currentType == typeof (DateTime))              ||
                                                                               (currentType == typeof (DateTimeOffset))        ||
                                                                               (currentType == typeof (TimeSpan))              ||
                                                                               (currentType == typeof (Guid))                  ||
                                                                               (currentType == typeof (Uri))                   ||
                                                                               (currentType == typeof (Version))               ||
                                                                               typeof (Type).IsAssignableFrom (c: currentType) ||
                                                                               typeof (Delegate).IsAssignableFrom (c: currentType));
  }
}
