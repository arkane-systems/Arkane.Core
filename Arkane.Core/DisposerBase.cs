#region header

// Arkane.Core - DisposerBase.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-02 11:22 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Serves as a wrapper around objects that require disposal, but that do not implement the <see cref="IDisposable" />
///   or <see cref="IAsyncDisposable" /> interfaces themselves.  This is useful for objects that are not under our control,
///   but that we need to ensure are disposed of properly.
/// </summary>
/// <typeparam name="T">The type of the object being wrapped.</typeparam>
/// <remarks>
///   This class supports both synchronous (<see cref="IDisposable"/>) and asynchronous (<see cref="IAsyncDisposable"/>)
///   disposal patterns. The synchronous <see cref="Dispose()"/> method is always available and sufficient for all use cases.
///   The asynchronous <see cref="DisposeAsync()"/> method provides an optional optimization path for scenarios where async
///   cleanup is beneficial. When neither sync nor async disposal is appropriate, the other is used as a fallback.
/// </remarks>
[PublicAPI]
public abstract class DisposerBase<T> (T o) : IDisposable, IAsyncDisposable
{
  private readonly Lock disposeSyncRoot = new ();

  /// <summary>
  ///   Indicates whether the object has been disposed.
  /// </summary>
  /// <remarks>
  ///   This field is typically used by derived classes to track the disposal state and prevent
  ///   operations on a disposed object. Access within the base disposal path is synchronized to ensure that the
  ///   transition to the disposed state is atomic when accessed from multiple threads.
  /// </remarks>
  #pragma warning disable CS0420

  // Intentional: the finalizer checks IsDisposed without synchronization to avoid lock acquisition in GC context,
  // which could cause deadlock. The state transition is guarded by TransitionToDisposedOrThrow; the finalizer's
  // unsynchronized check is a safe heuristic that prevents double-disposal attempts.
  protected bool IsDisposed;
  #pragma warning restore CS0420

  /// <summary>
  ///   The object to be managed and disposed by this class.
  /// </summary>
  public T Object { get; protected set; } = o;

  /// <summary>
  ///   Releases all resources used by the current instance of the class synchronously.
  /// </summary>
  /// <remarks>
  ///   Call this method when you are finished using the object to free unmanaged resources and perform
  ///   other cleanup operations. After calling Dispose, the object should not be used further.
  ///   
  ///   This method can be called from any context and provides a complete, synchronous cleanup path
  ///   without requiring async/await support. It is the primary entry point for disposal and is always sufficient
  ///   for managing the disposal lifecycle.
  /// </remarks>
  /// <exception cref="ObjectDisposedException">Thrown if the object has already been disposed.</exception>
  [PublicAPI]
  public void Dispose ()
  {
    GC.SuppressFinalize (this);
    this.TransitionToDisposedOrThrow ();
    this.Dispose (true);
  }

  /// <summary>
  ///   Releases all resources used by the current instance asynchronously.
  /// </summary>
  /// <remarks>
  ///   This method provides an optional asynchronous optimization for disposal scenarios where cleanup
  ///   may involve I/O or other time-consuming operations. If no async-specific cleanup logic is available,
  ///   this method falls back to the synchronous <see cref="Dispose(bool)"/> path.
  ///   
  ///   After calling this method, the object should not be used further.
  /// </remarks>
  /// <returns>A <see cref="ValueTask"/> representing the asynchronous disposal operation.</returns>
  /// <exception cref="ObjectDisposedException">Thrown if the object has already been disposed.</exception>
  [PublicAPI]
  public async ValueTask DisposeAsync ()
  {
    GC.SuppressFinalize (this);
    this.TransitionToDisposedOrThrow ();
    await this.DisposeAsyncCore ().ConfigureAwait (false);
    this.Dispose (false);
  }

  /// <summary>
  ///   Finalizer for the class. This method is called by the garbage collector when the object is being collected, and it
  ///   ensures that any unmanaged resources are released if Dispose was not called explicitly.
  /// </summary>
  /// <remarks>
  ///   Should not be reached; object should be properly disposed.  The finalizer is guarded to ensure
  ///   it never throws, which is a critical requirement for finalizers in .NET.
  /// </remarks>
  ~DisposerBase ()
  {
    if (!this.IsDisposed)
      this.Dispose (false);
  }

  /// <summary>
  ///   Releases the unmanaged resources used by the class and optionally releases the managed resources.
  ///   This method is called by both the Dispose method and the finalizer, and it contains the logic for
  ///   cleaning up resources.
  /// </summary>
  /// <remarks>
  ///   This is a pure dispatcher method that delegates to <see cref="DisposeImplementation"/>.
  ///   The disposed state gate is managed separately by <see cref="TransitionToDisposedOrThrow"/>,
  ///   which is called by both the synchronous and asynchronous public entry points.
  /// </remarks>
  /// <param name="disposing">
  ///   If true, releases both managed and unmanaged resources. If false, releases only unmanaged
  ///   resources.
  /// </param>
  protected void Dispose (bool disposing)
  {
    this.DisposeImplementation (disposing);
  }

  /// <summary>
  ///   Releases the unmanaged resources used by the object and optionally releases the managed resources.
  /// </summary>
  /// <remarks>
  ///   This method is called by the public Dispose and DisposeAsync methods. When disposing is true,
  ///   managed and unmanaged resources should be released. When disposing is false, only unmanaged resources
  ///   should be released. Derived classes should override this method to provide their specific cleanup logic.
  /// </remarks>
  /// <param name="disposing">
  ///   true to release both managed and unmanaged resources; false to release only unmanaged
  ///   resources.
  /// </param>
  protected abstract void DisposeImplementation (bool disposing);

  /// <summary>
  ///   Performs async managed-resource cleanup. Override this method in derived classes to provide
  ///   async-specific disposal logic.
  /// </summary>
  /// <remarks>
  ///   The default implementation calls the synchronous <see cref="DisposeImplementation"/> path,
  ///   ensuring backward compatibility with code that has no async cleanup needs. This method is called
  ///   by <see cref="DisposeAsync"/> after the disposed state transition has occurred.
  /// </remarks>
  /// <returns>A <see cref="ValueTask"/> representing the asynchronous cleanup operation.</returns>
  protected virtual ValueTask DisposeAsyncCore ()
  {
    this.DisposeImplementation (true);

    return ValueTask.CompletedTask;
  }

  /// <summary>
  ///   Atomically transitions the object to the disposed state, throwing if already disposed.
  /// </summary>
  /// <remarks>
  ///   This helper manages the disposed state gate and is called by both synchronous and asynchronous
  ///   entry points (<see cref="Dispose()"/> and <see cref="DisposeAsync()"/>) to ensure they share
  ///   consistent state logic. The finalizer does not call this method; it checks <see cref="IsDisposed"/>
  ///   directly to avoid throwing.
  /// </remarks>
  /// <exception cref="ObjectDisposedException">Thrown if the object has already been disposed.</exception>
  private void TransitionToDisposedOrThrow ()
  {
    lock (this.disposeSyncRoot)
      this.IsDisposed = !this.IsDisposed ? true : throw new ObjectDisposedException (this.GetType ().FullName);
  }
}
