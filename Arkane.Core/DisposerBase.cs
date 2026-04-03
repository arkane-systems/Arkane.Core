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
///   interface themselves.  This is useful for objects that are not under our control, but that we need to ensure are
///   disposed of properly.
/// </summary>
/// <typeparam name="T">The type of the object being wrapped.</typeparam>
[PublicAPI]
public abstract class DisposerBase<T> (T o) : IDisposable
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
  protected bool IsDisposed;

  /// <summary>
  ///   The object to be managed and disposed by this class.
  /// </summary>
  public T Object { get; protected set; } = o;

  /// <summary>
  ///   Releases all resources used by the current instance of the class.
  /// </summary>
  /// <remarks>
  ///   Call this method when you are finished using the object to free unmanaged resources and perform
  ///   other cleanup operations. After calling Dispose, the object should not be used further.
  /// </remarks>
  /// <exception cref="ObjectDisposedException">Thrown if the object has already been disposed.</exception>
  [PublicAPI]
  public void Dispose ()
  {
    GC.SuppressFinalize (this);
    this.Dispose (true);
  }

  /// <summary>
  ///   Finalizer for the class. This method is called by the garbage collector when the object is being collected, and it
  ///   ensures that any unmanaged resources are released if Dispose was not called explicitly.
  /// </summary>
  /// <remarks>
  ///   Should not be reached; object should be properly disposed.  If it is reached, writes out
  ///   a debug warning.
  /// </remarks>
  ~DisposerBase () => this.Dispose (false);

  /// <summary>
  ///   Releases the unmanaged resources used by the class and optionally releases the managed resources. This method is
  ///   called by both the Dispose method and the finalizer, and it contains the logic for cleaning up resources.
  /// </summary>
  /// <param name="disposing">
  ///   If true, releases both managed and unmanaged resources. If false, releases only unmanaged
  ///   resources.
  /// </param>
  /// <exception cref="ObjectDisposedException">Thrown if the object has already been disposed.</exception>
  protected void Dispose (bool disposing)
  {
    lock (this.disposeSyncRoot)
      this.IsDisposed = !this.IsDisposed ? true : throw new ObjectDisposedException (this.GetType ().FullName);

    this.DisposeImplementation (disposing);
  }

  /// <summary>
  ///   Releases the unmanaged resources used by the object and optionally releases the managed resources.
  /// </summary>
  /// <remarks>
  ///   This method is called by the public Dispose method and the finalizer. When disposing is true,
  ///   managed and unmanaged resources should be released. When disposing is false, only unmanaged resources should be
  ///   released.
  /// </remarks>
  /// <param name="disposing">
  ///   true to release both managed and unmanaged resources; false to release only unmanaged
  ///   resources.
  /// </param>
  protected abstract void DisposeImplementation (bool disposing);
}
