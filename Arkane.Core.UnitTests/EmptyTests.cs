#region header

// Arkane.Core.UnitTests - EmptyTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 7:20 PM

#endregion

#region using

using ArkaneSystems.Arkane;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests;

[TestClass]
public class EmptyTests
{
  [TestMethod]
  public void Equals_WhenComparingTwoEmptyValues_ReturnsTrue ()
  {
    // Arrange
    var left  = new Empty ();
    var right = new Empty ();

    // Act
    bool result = left.Equals (other: right);

    // Assert
    Assert.IsTrue (condition: result);
  }

  [TestMethod]
  public void EqualsObject_WhenObjectIsNotEmpty_ReturnsFalse ()
  {
    // Arrange
    var value = new Empty ();

    // Act
    // ReSharper disable once SuspiciousTypeConversion.Global
    bool result = value.Equals (obj: "not-empty");

    // Assert
    Assert.IsFalse (condition: result);
  }

  [TestMethod]
  public void EqualityOperators_WhenComparingTwoEmptyValues_ReturnExpectedResults ()
  {
    // Arrange
    var left  = new Empty ();
    var right = new Empty ();

    // Act
    bool areEqual    = left == right;
    bool areNotEqual = left != right;

    // Assert
    Assert.IsTrue (condition: areEqual);
    Assert.IsFalse (condition: areNotEqual);
  }

  [TestMethod]
  public void ToString_WhenCalled_ReturnsEmptyToken ()
  {
    // Arrange
    var value = new Empty ();

    // Act
    var result = value.ToString ();

    // Assert
    Assert.AreEqual (expected: "<empty>", actual: result);
  }
}
