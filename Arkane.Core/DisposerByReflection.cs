#region header

// Arkane.Core - DisposerByReflection.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 10:31 AM

#endregion

#region using

using System.Reflection;
using System.Runtime.ExceptionServices;

using ArkaneSystems.Arkane.Properties;

using JetBrains.Annotations;

using Metalama.Patterns.Contracts;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Serves as a wrapper around objects that require disposal, but that do not implement the <see cref="IDisposable" />
///   interface themselves.  This is useful for objects that are not under our control, but that we need to ensure are
///   disposed of properly. This class uses reflection to attempt to call an appropriate method on the wrapped object.
/// </summary>
/// <typeparam name="T">The type of the object being wrapped.</typeparam>
[PublicAPI]
public class DisposerByReflection<T> : DisposerBase<T>
{
  /// <summary>
  ///   Instantiates a new instance of the <see cref="DisposerByReflection{T}" /> class, wrapping the specified object and
  ///   using reflection to find a suitable method to call for disposal. By default, it looks for a public parameterless
  ///   instance method named "Dispose", but you can specify a different method name if needed.
  /// </summary>
  /// <param name="obj">The object to wrap.</param>
  /// <param name="methodName">The name of the method to call for disposal.</param>
  /// <exception cref="ArgumentException">
  ///   Thrown if method is static, has parameters, or contains unassigned generic type
  ///   parameters.
  /// </exception>
  public DisposerByReflection (T obj, [Required] string methodName = "Dispose")
    : this (obj: obj, method: GetPublicParameterlessInstanceMethod (obj: obj, methodName: methodName))
  { }

  /// <summary>
  ///   Initializes a new instance of the DisposerByReflection class using the specified object and a parameterless
  ///   instance method to call via reflection to perform disposal.
  /// </summary>
  /// <param name="obj">The object instance on which the disposal method will be invoked.</param>
  /// <param name="method">
  ///   A MethodInfo representing a parameterless, non-static instance method to be called for disposal. The method must
  ///   not have unassigned generic type parameters.
  /// </param>
  /// <exception cref="ArgumentException">
  ///   Thrown if method is static, has parameters, or contains unassigned generic type
  ///   parameters.
  /// </exception>
  public DisposerByReflection ([NotNullContractAttribute] T obj, MethodInfo method)
    : base (obj)
  {
    if (method.IsStatic)
      throw new ArgumentException (message: Resources.DisposerByReflection_DisposerByReflection_CantBeStaticMethod,
                                   paramName: nameof (method));

    if (method.GetParameters ().Length != 0)
      throw new ArgumentException (message: Resources.DisposerByReflection_DisposerByReflection_MustBeParameterlessMethod,
                                   paramName: nameof (method));

    if (method.ContainsGenericParameters)
      throw new ArgumentException (message: Resources
                                    .DisposerByReflection_DisposerByReflection_CantBeMethodWithUnassignedGenericTypeParams,
                                   paramName: nameof (method));

    this.Method = method;
  }

  /// <summary>
  ///   The metadata information for the method to which <see cref="DisposerBase{T}.Dispose" /> will delegate. This method is
  ///   expected to be a parameterless instance method that performs the necessary cleanup when called. The method should not
  ///   be static and must not contain unassigned generic type parameters. Access to this property is protected, and it is
  ///   set during construction of the DisposerByReflection instance.
  /// </summary>
  public MethodInfo Method { get; protected set; }

  private static MethodInfo GetPublicParameterlessInstanceMethod (T                 obj,
                                                                  [Required] string methodName)
  {
    MethodInfo? method = obj!.GetType ()
                             .GetMethod (name: methodName,
                                         bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding,
                                         binder: null,
                                         callConvention: CallingConventions.HasThis,
                                         types: Type.EmptyTypes,
                                         modifiers: null);

    return !(method == null)
             ? method
             : throw new ArgumentException (message: Resources
                                                    .DisposerByReflection_GetPublicParameterlessInstanceMethod_ObjectDoesNotHaveAPublicParameterInstanceMethod
                                                    .FillWith (methodName),
                                            paramName: nameof (methodName));
  }

  #region Overrides of DisposerBase<T>

  /// <inheritdoc />
  protected override void DisposeImplementation (bool disposing)
  {
    if (!disposing)
      return;

    try { this.Method.Invoke (obj: this.Object, parameters: null); }
    catch (TargetInvocationException ex) { ExceptionDispatchInfo.Capture (ex.InnerException!).Throw (); }
  }

  #endregion
}
