#region header

// Arkane.Core.UnitTests - MementoTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 4:00 PM

#endregion

#region using

using ArkaneSystems.Arkane;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests;

[TestClass]
[UsedImplicitly]
public class MementoTests
{
  [TestMethod]
  public void Dispose_WhenCalledOnce_ExecutesAction ()
  {
    // Arrange
    int callCount = 0;
    IDisposable memento = new Memento (() => callCount++);

    // Act
    memento.Dispose ();

    // Assert
    Assert.AreEqual (expected: 1, actual: callCount);
  }

  [TestMethod]
  public void Dispose_WhenCalledTwice_ExecutesActionOnlyOnce ()
  {
    // Arrange
    int callCount = 0;
    IDisposable memento = new Memento (() => callCount++);

    // Act
    memento.Dispose ();
    memento.Dispose ();

    // Assert
    Assert.AreEqual (expected: 1, actual: callCount);
  }

  [TestMethod]
  public void Dispose_WhenCalledWithUsingStatement_ExecutesAction ()
  {
    // Arrange
    bool actionExecuted = false;

    // Act
    using (new Memento (() => actionExecuted = true)) { }

    // Assert
    Assert.IsTrue (condition: actionExecuted);
  }
}
