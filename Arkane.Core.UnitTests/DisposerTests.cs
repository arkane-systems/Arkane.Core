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

internal sealed class BlockingDisposableTarget (ManualResetEventSlim disposeEntered, ManualResetEventSlim releaseDispose)
{
  private int disposeCount;

  public int DisposeCount => this.disposeCount;

  public void Dispose ()
  {
    Interlocked.Increment (location: ref this.disposeCount);
    disposeEntered.Set ();
    releaseDispose.Wait ();
  }
}
