#region header

// Arkane.Core.UnitTests - ExtensionMethods-System-ActionTests.cs
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

namespace Arkane.Core.UnitTests.Extensions;

[TestClass]
[UsedImplicitly]
public class ExtensionMethodsSystemActionTests
{
  #region AsMemento

  [TestMethod]
  public void AsMemento_ReturnsIDisposable ()
  {
    // Arrange
    Action action = () => { };

    // Act
    IDisposable result = action.AsMemento ();

    // Assert
    Assert.IsNotNull (value: result);
    result.Dispose ();
  }

  [TestMethod]
  public void AsMemento_WhenDisposed_ExecutesAction ()
  {
    // Arrange
    bool actionExecuted = false;
    Action action = () => actionExecuted = true;

    // Act
    using (action.AsMemento ()) { }

    // Assert
    Assert.IsTrue (condition: actionExecuted);
  }

  [TestMethod]
  public void AsMemento_WhenDisposedTwice_ExecutesActionOnlyOnce ()
  {
    // Arrange
    int callCount = 0;
    Action action = () => callCount++;
    IDisposable memento = action.AsMemento ();

    // Act
    memento.Dispose ();
    memento.Dispose ();

    // Assert
    Assert.AreEqual (expected: 1, actual: callCount);
  }

  #endregion

  #region WithFailFast

  [TestMethod]
  public void WithFailFast_WhenActionSucceeds_ExecutesAction ()
  {
    // Arrange
    bool actionExecuted = false;
    Action action = () => actionExecuted = true;

    // Act
    action.WithFailFast () ();

    // Assert
    Assert.IsTrue (condition: actionExecuted);
  }

  [TestMethod]
  public void WithFailFast_ReturnsDelegate ()
  {
    // Arrange
    Action action = () => { };

    // Act
    Action wrapped = action.WithFailFast ();

    // Assert
    Assert.IsNotNull (value: wrapped);
  }

  #endregion
}
