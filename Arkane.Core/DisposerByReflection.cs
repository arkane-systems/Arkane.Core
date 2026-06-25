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
/// <remarks>
///   Supports both synchronous and asynchronous disposal. The synchronous <see cref="IDisposable.Dispose" /> method is
///   always available. For asynchronous disposal via <see cref="IAsyncDisposable.DisposeAsync" />, the class will first
///   check if the wrapped object implements <see cref="IAsyncDisposable" />, and if so, invoke its <see cref="IAsyncDisposable.DisposeAsync" />
///   method. Otherwise, it will attempt to reflect for a public parameterless instance method with the specified async
///   method name (default: "DisposeAsync") that returns <see cref="ValueTask" /> or <see cref="Task" />. If no async
///   method is found, the synchronous disposal path is used as a fallback.
/// </remarks>
/// <typeparam name="T">The type of the object being wrapped.</typeparam>
[PublicAPI]
public class DisposerByReflection<T> : DisposerBase<T>
{
  /// <summary>
  ///   Instantiates a new instance of the <see cref="DisposerByReflection{T}" /> class, wrapping the specified object and
  ///   using reflection to find suitable methods to call for disposal. By default, it looks for a public parameterless
  ///   instance method named "Dispose" for synchronous disposal and "DisposeAsync" for asynchronous disposal, but you can
  ///   specify different method names if needed.
  /// </summary>
  /// <param name="obj">The object to wrap.</param>
  /// <param name="methodName">The name of the method to call for synchronous disposal.</param>
  /// <param name="asyncMethodName">The name of the method to call for asynchronous disposal. Defaults to "DisposeAsync".</param>
  /// <exception cref="ArgumentException">
  ///   Thrown if the synchronous method is static, has parameters, or contains unassigned generic type
  ///   parameters.
  /// </exception>
  public DisposerByReflection (T obj, [Required] string methodName = "Dispose", string? asyncMethodName = "DisposeAsync")
    : this (obj: obj, method: GetPublicParameterlessInstanceMethod (obj: obj, methodName: methodName))
  {
    this.targetIsIAsyncDisposable = obj is IAsyncDisposable;
    if (!this.targetIsIAsyncDisposable && asyncMethodName is not null)
      this.AsyncMethod = TryGetAsyncMethod (obj, asyncMethodName);
  }

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
  public DisposerByReflection ([NotNullContract] T obj, MethodInfo method)
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

  /// <summary>
  ///   The metadata information for the async method to which <see cref="IAsyncDisposable.DisposeAsync" /> will delegate,
  ///   if available. This method is expected to be a parameterless instance method that returns <see cref="ValueTask" />
  ///   or <see cref="Task" /> and performs asynchronous cleanup when called. If no async method is found, this property
  ///   will be <c>null</c>, and the synchronous disposal path will be used as a fallback.
  /// </summary>
  public MethodInfo? AsyncMethod { get; protected set; }

  /// <summary>
  ///   Internal flag indicating whether the wrapped object implements <see cref="IAsyncDisposable" />.
  /// </summary>
  private readonly bool targetIsIAsyncDisposable;

  private static MethodInfo? TryGetAsyncMethod (T obj, string methodName)
  {
    MethodInfo? method = obj!.GetType ()
                              .GetMethod (name: methodName,
                                          bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding,
                                          binder: null,
                                          callConvention: CallingConventions.HasThis,
                                          types: Type.EmptyTypes,
                                          modifiers: null);

    if (method is null)
      return null;

    // Validate return type is exactly Task or ValueTask
    if (method.ReturnType != typeof(Task) && method.ReturnType != typeof(ValueTask))
      return null;

    return method;
  }

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

  /// <inheritdoc />
  protected override async ValueTask DisposeAsyncCore()
  {
    if (this.targetIsIAsyncDisposable)
    {
      await ((IAsyncDisposable)this.Object!).DisposeAsync().ConfigureAwait(false);
      return;
    }

    if (this.AsyncMethod is { } m)
    {
      try
      {
        if (m.ReturnType == typeof(ValueTask))
        {
          await ((ValueTask)m.Invoke(this.Object, null)!).ConfigureAwait(false);
        }
        else // Task
        {
          await ((Task)m.Invoke(this.Object, null)!).ConfigureAwait(false);
        }
      }
      catch (TargetInvocationException ex)
      {
        ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
      }
      return;
    }

    await base.DisposeAsyncCore().ConfigureAwait(false);
  }

  #endregion
}
