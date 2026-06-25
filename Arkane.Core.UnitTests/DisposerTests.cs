#region header

// Arkane.Core.UnitTests - DisposerTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-03 1:39 PM

#endregion

#region using

using ArkaneSystems.Arkane;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests;

[TestClass]
public class DisposerByDelegationTests
{
  [TestMethod]
  public async Task DisposeAsync_WhenAsyncActionProvided_InvokesAsyncAction ()
  {
    // Arrange
    var target = new CountingAsyncDisposableTarget ();
    var disposer = new DisposerByDelegation<CountingAsyncDisposableTarget> (obj: target,
                                                                             disposeAction: static o => o.Dispose (),
                                                                             asyncDisposeAction: static o => o.DisposeAsync ());

    // Act
    await disposer.DisposeAsync ();

    // Assert
    Assert.AreEqual (expected: 0, actual: target.DisposeCount);
    Assert.AreEqual (expected: 1, actual: target.DisposeAsyncCount);
  }

  [TestMethod]
  public async Task DisposeAsync_WhenNoAsyncAction_UsesSyncFallback ()
  {
    // Arrange
    var target = new CountingAsyncDisposableTarget ();
    var disposer = new DisposerByDelegation<CountingAsyncDisposableTarget> (obj: target,
                                                                             disposeAction: static o => o.Dispose ());

    // Act
    await disposer.DisposeAsync ();

    // Assert
    Assert.AreEqual (expected: 1, actual: target.DisposeCount);
    Assert.AreEqual (expected: 0, actual: target.DisposeAsyncCount);
  }

  [TestMethod]
  public async Task DisposeAsync_WhenCalledConcurrently_InvokesAsyncDelegateOnlyOnce ()
  {
    // Arrange
    using var disposeEntered = new ManualResetEventSlim (initialState: false);
    using var releaseDispose = new ManualResetEventSlim (initialState: false);
    using var barrier        = new Barrier (participantCount: 3);
    var       target         = new CountingAsyncDisposableTarget (disposeEntered: disposeEntered, releaseDispose: releaseDispose);
    var disposer = new DisposerByDelegation<CountingAsyncDisposableTarget> (obj: target,
                                                                             disposeAction: static o => o.Dispose (),
                                                                             asyncDisposeAction: static o => o.DisposeAsync ());

    Task<Exception?> firstDispose  = Task.Run (() => DisposeAsyncAndCaptureException (disposer: disposer, barrier: barrier));
    Task<Exception?> secondDispose = Task.Run (() => DisposeAsyncAndCaptureException (disposer: disposer, barrier: barrier));

    // Act
    barrier.SignalAndWait ();
    bool enteredDispose = disposeEntered.Wait (timeout: TimeSpan.FromSeconds (value: 5));
    releaseDispose.Set ();
    Exception?[] exceptions = await Task.WhenAll (firstDispose, secondDispose);

    // Assert
    Assert.IsTrue (condition: enteredDispose);
    Assert.AreEqual (expected: 1, actual: target.DisposeAsyncCount);
    Assert.AreEqual (expected: 1, actual: exceptions.Count (predicate: static ex => ex == null));
    Assert.AreEqual (expected: 1, actual: exceptions.Count (predicate: static ex => ex is ObjectDisposedException));
  }

  [TestMethod]
  public async Task DisposeAsync_WhenDisposedSynchronously_ThrowsObjectDisposedException ()
  {
    // Arrange
    var target   = new CountingAsyncDisposableTarget ();
    var disposer = new DisposerByDelegation<CountingAsyncDisposableTarget> (obj: target,
                                                                             disposeAction: static o => o.Dispose (),
                                                                             asyncDisposeAction: static o => o.DisposeAsync ());
    disposer.Dispose ();

    // Act / Assert
    await Assert.ThrowsExactlyAsync<ObjectDisposedException> (action: async () => await disposer.DisposeAsync ());
    Assert.AreEqual (expected: 1, actual: target.DisposeCount);
    Assert.AreEqual (expected: 0, actual: target.DisposeAsyncCount);
  }

  [TestMethod]
  public void Dispose_WhenDisposedAsynchronously_ThrowsObjectDisposedException ()
  {
    // Arrange
    var target   = new CountingAsyncDisposableTarget ();
    var disposer = new DisposerByDelegation<CountingAsyncDisposableTarget> (obj: target,
                                                                             disposeAction: static o => o.Dispose (),
                                                                             asyncDisposeAction: static o => o.DisposeAsync ());
    disposer.DisposeAsync ().AsTask ().GetAwaiter ().GetResult ();

    // Act / Assert
    Assert.ThrowsExactly<ObjectDisposedException> (action: disposer.Dispose);
    Assert.AreEqual (expected: 0, actual: target.DisposeCount);
    Assert.AreEqual (expected: 1, actual: target.DisposeAsyncCount);
  }

  [TestMethod]
  public async Task Dispose_WhenCalledConcurrently_InvokesDelegateOnlyOnce ()
  {
    // Arrange
    using var disposeEntered = new ManualResetEventSlim (initialState: false);
    using var releaseDispose = new ManualResetEventSlim (initialState: false);
    using var barrier        = new Barrier (participantCount: 3);
    var       target         = new BlockingDisposableTarget (disposeEntered: disposeEntered, releaseDispose: releaseDispose);
    var disposer = new DisposerByDelegation<BlockingDisposableTarget> (obj: target,
                                                                       disposeAction: static o => o.Dispose ());

    Task<Exception?> firstDispose  = Task.Run (() => DisposeAndCaptureException (disposer: disposer, barrier: barrier));
    Task<Exception?> secondDispose = Task.Run (() => DisposeAndCaptureException (disposer: disposer, barrier: barrier));

    // Act
    barrier.SignalAndWait ();
    bool enteredDispose = disposeEntered.Wait (timeout: TimeSpan.FromSeconds (value: 5));
    releaseDispose.Set ();
    Exception?[] exceptions = await Task.WhenAll (firstDispose, secondDispose);

    // Assert
    Assert.IsTrue (condition: enteredDispose);
    Assert.AreEqual (expected: 1, actual: target.DisposeCount);
    Assert.AreEqual (expected: 1, actual: exceptions.Count (predicate: static ex => ex == null));
    Assert.AreEqual (expected: 1, actual: exceptions.Count (predicate: static ex => ex is ObjectDisposedException));
  }

  private static async Task<Exception?> DisposeAsyncAndCaptureException (IAsyncDisposable disposer, Barrier barrier)
  {
    try
    {
      barrier.SignalAndWait ();
      await disposer.DisposeAsync ();

      return null;
    }
    catch (Exception ex) { return ex; }
  }

  private static Exception? DisposeAndCaptureException (IDisposable disposer, Barrier barrier)
  {
    try
    {
      barrier.SignalAndWait ();
      disposer.Dispose ();

      return null;
    }
    catch (Exception ex) { return ex; }
  }

}

[TestClass]
public class DisposerByReflectionTests
{
  [TestMethod]
  public async Task DisposeAsync_WhenTargetImplementsIAsyncDisposable_InvokesInterfaceMethod ()
  {
    // Arrange
    var target   = new CountingAsyncDisposableTarget ();
    var disposer = new DisposerByReflection<CountingAsyncDisposableTarget> (obj: target);

    // Act
    await disposer.DisposeAsync ();

    // Assert
    Assert.AreEqual (expected: 0, actual: target.DisposeCount);
    Assert.AreEqual (expected: 1, actual: target.DisposeAsyncCount);
  }

  [TestMethod]
  public async Task DisposeAsync_WhenTargetHasValueTaskMethod_InvokesReflectedMethod ()
  {
    // Arrange
    var target   = new AsyncMethodTarget ();
    var disposer = new DisposerByReflection<AsyncMethodTarget> (obj: target);

    // Act
    await disposer.DisposeAsync ();

    // Assert
    Assert.AreEqual (expected: 1, actual: target.DisposeCount);
  }

  [TestMethod]
  public async Task DisposeAsync_WhenTargetHasTaskMethod_InvokesReflectedMethod ()
  {
    // Arrange
    var target   = new TaskMethodTarget ();
    var disposer = new DisposerByReflection<TaskMethodTarget> (obj: target);

    // Act
    await disposer.DisposeAsync ();

    // Assert
    Assert.AreEqual (expected: 1, actual: target.DisposeCount);
  }

  [TestMethod]
  public async Task DisposeAsync_WhenNoAsyncPathExists_UsesSyncFallback ()
  {
    // Arrange
    var target   = new BlockingDisposableTarget ();
    var disposer = new DisposerByReflection<BlockingDisposableTarget> (obj: target,
                                                                        asyncMethodName: null);

    // Act
    await disposer.DisposeAsync ();

    // Assert
    Assert.AreEqual (expected: 1, actual: target.DisposeCount);
  }

  [TestMethod]
  public async Task DisposeAsync_WhenTargetImplementsIAsyncDisposable_PrefersInterfaceOverReflectedMethod ()
  {
    // Arrange
    var target   = new DualDisposableTarget ();
    var disposer = new DisposerByReflection<DualDisposableTarget> (obj: target);

    // Act
    await disposer.DisposeAsync ();

    // Assert
    Assert.AreEqual (expected: 0, actual: target.PublicDisposeAsyncCount);
    Assert.AreEqual (expected: 1, actual: target.InterfaceDisposeAsyncCount);
  }

  [TestMethod]
  public async Task DisposeAsync_WhenReflectedAsyncMethodHasUnsupportedReturnType_FallsBackToSync ()
  {
    // Arrange
    var target   = new InvalidAsyncMethodTarget ();
    var disposer = new DisposerByReflection<InvalidAsyncMethodTarget> (obj: target);

    // Act
    await disposer.DisposeAsync ();

    // Assert
    Assert.AreEqual (expected: 1, actual: target.DisposeCount);
    Assert.AreEqual (expected: 0, actual: target.DisposeAsyncCount);
  }

  [TestMethod]
  public async Task DisposeAsync_WhenCalledConcurrently_InvokesAsyncPathOnlyOnce ()
  {
    // Arrange
    using var disposeEntered = new ManualResetEventSlim (initialState: false);
    using var releaseDispose = new ManualResetEventSlim (initialState: false);
    using var barrier        = new Barrier (participantCount: 3);
    var       target         = new CountingAsyncDisposableTarget (disposeEntered: disposeEntered, releaseDispose: releaseDispose);
    var       disposer       = new DisposerByReflection<CountingAsyncDisposableTarget> (obj: target);

    Task<Exception?> firstDispose  = Task.Run (() => DisposeAsyncAndCaptureException (disposer: disposer, barrier: barrier));
    Task<Exception?> secondDispose = Task.Run (() => DisposeAsyncAndCaptureException (disposer: disposer, barrier: barrier));

    // Act
    barrier.SignalAndWait ();
    bool enteredDispose = disposeEntered.Wait (timeout: TimeSpan.FromSeconds (value: 5));
    releaseDispose.Set ();
    Exception?[] exceptions = await Task.WhenAll (firstDispose, secondDispose);

    // Assert
    Assert.IsTrue (condition: enteredDispose);
    Assert.AreEqual (expected: 1, actual: target.DisposeAsyncCount);
    Assert.AreEqual (expected: 1, actual: exceptions.Count (predicate: static ex => ex == null));
    Assert.AreEqual (expected: 1, actual: exceptions.Count (predicate: static ex => ex is ObjectDisposedException));
  }

  [TestMethod]
  public async Task DisposeAsync_WhenDisposedSynchronously_ThrowsObjectDisposedException ()
  {
    // Arrange
    var target   = new CountingAsyncDisposableTarget ();
    var disposer = new DisposerByReflection<CountingAsyncDisposableTarget> (obj: target);
    disposer.Dispose ();

    // Act / Assert
    await Assert.ThrowsExactlyAsync<ObjectDisposedException> (action: async () => await disposer.DisposeAsync ());
  }

  [TestMethod]
  public void Dispose_WhenDisposedAsynchronously_ThrowsObjectDisposedException ()
  {
    // Arrange
    var target   = new CountingAsyncDisposableTarget ();
    var disposer = new DisposerByReflection<CountingAsyncDisposableTarget> (obj: target);
    disposer.DisposeAsync ().AsTask ().GetAwaiter ().GetResult ();

    // Act / Assert
    Assert.ThrowsExactly<ObjectDisposedException> (action: disposer.Dispose);
  }

  [TestMethod]
  public async Task Dispose_WhenCalledConcurrently_InvokesMethodOnlyOnce ()
  {
    // Arrange
    using var disposeEntered = new ManualResetEventSlim (initialState: false);
    using var releaseDispose = new ManualResetEventSlim (initialState: false);
    using var barrier        = new Barrier (participantCount: 3);
    var       target         = new BlockingDisposableTarget (disposeEntered: disposeEntered, releaseDispose: releaseDispose);
    var       disposer       = new DisposerByReflection<BlockingDisposableTarget> (obj: target);

    Task<Exception?> firstDispose  = Task.Run (() => DisposeAndCaptureException (disposer: disposer, barrier: barrier));
    Task<Exception?> secondDispose = Task.Run (() => DisposeAndCaptureException (disposer: disposer, barrier: barrier));

    // Act
    barrier.SignalAndWait ();
    bool enteredDispose = disposeEntered.Wait (timeout: TimeSpan.FromSeconds (value: 5));
    releaseDispose.Set ();
    Exception?[] exceptions = await Task.WhenAll (firstDispose, secondDispose);

    // Assert
    Assert.IsTrue (condition: enteredDispose);
    Assert.AreEqual (expected: 1, actual: target.DisposeCount);
    Assert.AreEqual (expected: 1, actual: exceptions.Count (predicate: static ex => ex == null));
    Assert.AreEqual (expected: 1, actual: exceptions.Count (predicate: static ex => ex is ObjectDisposedException));
  }

  private static async Task<Exception?> DisposeAsyncAndCaptureException (IAsyncDisposable disposer, Barrier barrier)
  {
    try
    {
      barrier.SignalAndWait ();
      await disposer.DisposeAsync ();

      return null;
    }
    catch (Exception ex) { return ex; }
  }

  private static Exception? DisposeAndCaptureException (IDisposable disposer, Barrier barrier)
  {
    try
    {
      barrier.SignalAndWait ();
      disposer.Dispose ();

      return null;
    }
    catch (Exception ex) { return ex; }
  }

}

internal sealed class BlockingDisposableTarget (ManualResetEventSlim? disposeEntered = null, ManualResetEventSlim? releaseDispose = null)
{
  private int disposeCount;

  public int DisposeCount => this.disposeCount;

  public void Dispose ()
  {
    Interlocked.Increment (location: ref this.disposeCount);
    disposeEntered?.Set ();
    releaseDispose?.Wait ();
  }
}

internal sealed class CountingAsyncDisposableTarget
{
  private readonly ManualResetEventSlim? disposeEntered;
  private readonly ManualResetEventSlim? releaseDispose;
  private          int                   disposeCount;
  private          int                   disposeAsyncCount;

  public CountingAsyncDisposableTarget (ManualResetEventSlim? disposeEntered = null, ManualResetEventSlim? releaseDispose = null)
  {
    this.disposeEntered = disposeEntered;
    this.releaseDispose = releaseDispose;
  }

  public int DisposeCount      => this.disposeCount;
  public int DisposeAsyncCount => this.disposeAsyncCount;

  public void Dispose ()
  {
    Interlocked.Increment (location: ref this.disposeCount);
  }

  public ValueTask DisposeAsync ()
  {
    Interlocked.Increment (location: ref this.disposeAsyncCount);
    this.disposeEntered?.Set ();
    this.releaseDispose?.Wait ();
    return ValueTask.CompletedTask;
  }
}

internal sealed class DualDisposableTarget : IAsyncDisposable
{
  private int interfaceDisposeAsyncCount;
  private int publicDisposeAsyncCount;
  private int disposeCount;

  public int InterfaceDisposeAsyncCount => this.interfaceDisposeAsyncCount;
  public int PublicDisposeAsyncCount    => this.publicDisposeAsyncCount;
  public int DisposeCount               => this.disposeCount;

  public void Dispose ()
  {
    Interlocked.Increment (location: ref this.disposeCount);
  }

  public ValueTask DisposeAsync ()
  {
    Interlocked.Increment (location: ref this.publicDisposeAsyncCount);
    return ValueTask.CompletedTask;
  }

  ValueTask IAsyncDisposable.DisposeAsync ()
  {
    Interlocked.Increment (location: ref this.interfaceDisposeAsyncCount);
    return ValueTask.CompletedTask;
  }
}

internal sealed class AsyncMethodTarget
{
  private int disposeCount;

  public int DisposeCount => this.disposeCount;

  public ValueTask DisposeAsync ()
  {
    Interlocked.Increment (location: ref this.disposeCount);
    return ValueTask.CompletedTask;
  }

  public void Dispose () { }
}

internal sealed class TaskMethodTarget
{
  private int disposeCount;

  public int DisposeCount => this.disposeCount;

  public Task DisposeAsync ()
  {
    Interlocked.Increment (location: ref this.disposeCount);
    return Task.CompletedTask;
  }

  public void Dispose () { }
}

internal sealed class InvalidAsyncMethodTarget
{
  private int disposeCount;
  private int disposeAsyncCount;

  public int DisposeCount      => this.disposeCount;
  public int DisposeAsyncCount => this.disposeAsyncCount;

  public Task<int> DisposeAsync ()
  {
    Interlocked.Increment (location: ref this.disposeAsyncCount);
    return Task.FromResult (0);
  }

  public void Dispose ()
  {
    Interlocked.Increment (location: ref this.disposeCount);
  }
}
