#region header

// Arkane.Core.UnitTests - RandomProviderTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 9:29 AM

#endregion

#region using

using ArkaneSystems.Arkane;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests;

/// <summary>
///   Unit tests for <see cref="RandomProvider" />.
/// </summary>
[TestClass]
public class RandomProviderTests
{
  /// <summary>
  ///   Tests that GetInstance returns a non-null Random instance.
  /// </summary>
  [TestMethod]
  public void GetInstance_Called_ReturnsNonNullRandomInstance ()
  {
    // Arrange & Act
    Random result = RandomProvider.GetInstance ();

    // Assert
    Assert.IsNotNull (result);
    Assert.IsInstanceOfType<Random> (result);
  }

  /// <summary>
  ///   Tests that GetInstance returns the same instance on subsequent calls from the same thread.
  /// </summary>
  [TestMethod]
  public void GetInstance_CalledMultipleTimes_ReturnsSameInstanceOnSameThread ()
  {
    // Arrange & Act
    Random firstInstance  = RandomProvider.GetInstance ();
    Random secondInstance = RandomProvider.GetInstance ();

    // Assert
    Assert.AreSame (expected: firstInstance, actual: secondInstance);
  }

  /// <summary>
  ///   Tests that GetInstance returns different instances on different threads.
  /// </summary>
  [TestMethod]
  public void GetInstance_CalledOnDifferentThreads_ReturnsDifferentInstances ()
  {
    // Arrange
    // ReSharper disable once RedundantAssignment
    Random? instanceFromMainThread  = null;
    Random? instanceFromOtherThread = null;
    var     threadCompleted         = new ManualResetEventSlim (false);

    // Act
    instanceFromMainThread = RandomProvider.GetInstance ();

    var thread = new Thread (() =>
                             {
                               instanceFromOtherThread = RandomProvider.GetInstance ();
                               threadCompleted.Set ();
                             });

    thread.Start ();
    threadCompleted.Wait ();

    // Assert
    Assert.IsNotNull (instanceFromMainThread);
    Assert.IsNotNull (instanceFromOtherThread);
    Assert.AreNotSame (notExpected: instanceFromMainThread, actual: instanceFromOtherThread);
  }

  /// <summary>
  ///   Tests that GetInstance returns a functional Random instance that can generate random numbers.
  /// </summary>
  [TestMethod]
  public void GetInstance_CalledAndUsed_GeneratesRandomNumbers ()
  {
    // Arrange
    Random random = RandomProvider.GetInstance ();

    // Act
    int value1 = random.Next ();
    int value2 = random.Next ();

    // Assert
    Assert.IsGreaterThanOrEqualTo (lowerBound: 0, value: value1);
    Assert.IsGreaterThanOrEqualTo (lowerBound: 0, value: value2);
  }

  /// <summary>
  ///   Tests that GetInstance returns instances with different seeds.
  /// </summary>
  [TestMethod]
  public void GetInstance_CalledOnMultipleThreads_InstancesHaveDifferentSeeds ()
  {
    // Arrange
    const int threadCount = 5;
    var       instances   = new Random[threadCount];
    var       threads     = new Thread[threadCount];
    var       countdown   = new CountdownEvent (threadCount);

    // Act
    for (var i = 0; i < threadCount; i++)
    {
      int index = i;
      threads[i] = new Thread (() =>
                               {
                                 instances[index] = RandomProvider.GetInstance ();
                                 countdown.Signal ();
                               });
      threads[i].Start ();
    }

    countdown.Wait ();

    // Assert - instances should be distinct
    for (var i = 0; i < threadCount; i++)
    {
      Assert.IsNotNull (instances[i]);
      for (int j = i + 1; j < threadCount; j++)
        Assert.AreNotSame (notExpected: instances[i], actual: instances[j]);
    }
  }
}
