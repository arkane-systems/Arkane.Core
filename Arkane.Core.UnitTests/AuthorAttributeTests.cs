#region header

// Arkane.Core.UnitTests - AuthorAttributeTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 7:21 PM

#endregion

#region using

using ArkaneSystems.Arkane.Annotations;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests;

[TestClass]
public class AuthorAttributeTests
{
  [TestMethod]
  public void ToString_WhenCalled_ReturnsFormattedAuthorInformation ()
  {
    // Arrange
    var attribute = new AuthorAttribute (name: "Jane Doe", emailAddress: "jane@example.com");

    // Act
    var result = attribute.ToString ();

    // Assert
    Assert.AreEqual (expected: "Author: Jane Doe <jane@example.com>", actual: result);
  }

  [TestMethod]
  public void Equals_WhenComparingSameInstance_ReturnsTrue ()
  {
    // Arrange
    var attribute = new AuthorAttribute (name: "Jane Doe", emailAddress: "jane@example.com");

    // Act
    // ReSharper disable once EqualExpressionComparison
    bool result = attribute.Equals (obj: attribute);

    // Assert
    Assert.IsTrue (condition: result);
  }

  [TestMethod]
  public void Equals_WhenComparingDifferentInstancesWithEquivalentEmail_ReturnsFalse ()
  {
    // Arrange
    var left  = new AuthorAttribute (name: "Jane Doe",   emailAddress: "jane@example.com");
    var right = new AuthorAttribute (name: "John Smith", emailAddress: "JANE@EXAMPLE.COM");

    // Act
    bool result = left.Equals (obj: right);

    // Assert
    Assert.IsFalse (condition: result);
  }

  [TestMethod]
  public void EqualityOperators_WhenEmailsDifferOnlyByCase_ReturnExpectedResults ()
  {
    // Arrange
    var left  = new AuthorAttribute (name: "Jane Doe",     emailAddress: "jane@example.com");
    var right = new AuthorAttribute (name: "Someone Else", emailAddress: "JANE@EXAMPLE.COM");

    // Act
    bool areEqual    = left == right;
    bool areNotEqual = left != right;

    // Assert
    Assert.IsFalse (condition: areEqual);
    Assert.IsTrue (condition: areNotEqual);
  }

  [TestMethod]
  public void GetHashCode_WhenCalledMultipleTimesOnSameInstance_ReturnsSameHashCode ()
  {
    // Arrange
    var attribute = new AuthorAttribute (name: "Jane Doe", emailAddress: "jane@example.com");

    // Act
    int firstHash  = attribute.GetHashCode ();
    int secondHash = attribute.GetHashCode ();

    // Assert
    Assert.AreEqual (expected: firstHash, actual: secondHash);
  }
}
