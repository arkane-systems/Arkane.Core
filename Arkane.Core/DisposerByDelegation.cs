#region header

// Arkane.Core - DisposerByDelegation.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-03 9:49 AM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Serves as a wrapper around objects that require disposal, but that do not implement the <see cref="IDisposable" />
///   interface themselves.  This is useful for objects that are not under our control, but that we need to ensure are
///   disposed of properly. This class uses delegation to a provided delegate to perform the disposal action, which allows
///   for more flexibility and better performance than reflection-based disposal. The delegate can be any method that takes
///   an instance of the wrapped object and performs the necessary cleanup operations.
/// </summary>
/// <remarks>
///   Supports both synchronous and asynchronous disposal. The synchronous <see cref="IDisposable.Dispose" /> method is
///   always available. For asynchronous disposal via <see cref="IAsyncDisposable.DisposeAsync" />, provide an async
///   delegate via the constructor overload; if no async delegate is provided, the synchronous disposal path is used as
///   a fallback.
/// </remarks>
/// <typeparam name="T">The type of the object being wrapped.</typeparam>
[PublicAPI]
public class DisposerByDelegation<T> (T obj, Action<T> disposeAction) : DisposerBase<T> (obj)
{
  /// <summary>
  ///   Represents the action to perform when disposing an instance of type T.
  /// </summary>
  /// <remarks>
  ///   This delegate is typically invoked to release resources or perform cleanup operations specific to
  ///   the type parameter T. The exact behavior depends on the implementation provided when initializing this
  ///   field.
  /// </remarks>
  protected readonly Action<T> DisposeAction = disposeAction ?? throw new ArgumentNullException (nameof (disposeAction));

  /// <summary>
  ///   Represents the asynchronous action to perform when disposing an instance of type T.
  /// </summary>
  /// <remarks>
  ///   This delegate is optional. If provided, it is used for asynchronous disposal via <see cref="IAsyncDisposable.DisposeAsync" />.
  ///   If not provided, the synchronous <see cref="DisposeAction" /> is used as a fallback.
  /// </remarks>
  protected readonly Func<T, ValueTask>? AsyncDisposeAction;

  /// <summary>
  ///   Initializes a new instance of the <see cref="DisposerByDelegation{T}" /> class with both synchronous and
  ///   asynchronous disposal delegates.
  /// </summary>
  /// <param name="obj">The object to wrap.</param>
  /// <param name="disposeAction">The synchronous disposal action.</param>
  /// <param name="asyncDisposeAction">The asynchronous disposal action.</param>
  [PublicAPI]
  public DisposerByDelegation(T obj, Action<T> disposeAction, Func<T, ValueTask> asyncDisposeAction)
    : this(obj, disposeAction)
  {
    this.AsyncDisposeAction = asyncDisposeAction ?? throw new ArgumentNullException(nameof(asyncDisposeAction));
  }

  #region Overrides of DisposerBase<T>

  /// <inheritdoc />
  protected override void DisposeImplementation (bool disposing)
  {
    if (!disposing)
      return;

    this.DisposeAction (this.Object);
  }

  /// <inheritdoc />
  protected override ValueTask DisposeAsyncCore()
  {
    if (this.AsyncDisposeAction is { } fn)
    {
      return fn(this.Object);
    }

    return base.DisposeAsyncCore();
  }

  #endregion
}
