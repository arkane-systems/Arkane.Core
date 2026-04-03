#region header

// Arkane.Core.UnitTests - ExtensionMethodsTests.cs
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
public class ExtensionMethodsTests
{
  #region Nested type: DerivedType

  private sealed class DerivedType : GenericBase<int>;

  #endregion

  #region Nested type: GenericBase

  // ReSharper disable once UnusedTypeParameter
  private class GenericBase<T>;

  #endregion

  [TestMethod]
  public void UpTo_WhenInputIsNull_ReturnsEmptyString ()
  {
    // Arrange
    string? input = null;

    // Act
    #pragma warning disable CS8604 // Possible null reference argument.
    string result = input.UpTo (maxLength: 3);
    #pragma warning restore CS8604 // Possible null reference argument.

    // Assert
    Assert.AreEqual (expected: string.Empty, actual: result);
  }

  [TestMethod]
  public void UpTo_WhenInputLengthExceedsMaxLength_TruncatesAndAppendsSuffix ()
  {
    // Arrange
    const string input = "abcdef";

    // Act
    string result = input.UpTo (maxLength: 3, suffixToUseWhenTooLong: "..*");

    // Assert
    Assert.AreEqual (expected: "abc..*", actual: result);
  }

  [TestMethod]
  public void RemoveNonAlphanumeric_WhenInputContainsMixedCharacters_RemovesNonAlphanumericCharacters ()
  {
    // Arrange
    const string input = "Ab-12_!?C";

    // Act
    string result = input.RemoveNonAlphanumeric ();

    // Assert
    Assert.AreEqual (expected: "Ab12C", actual: result);
  }

  [TestMethod]
  public void IsDerivedFromGenericType_WhenTypeDerivesFromGenericBase_ReturnsTrue ()
  {
    // Arrange
    Type type = typeof (DerivedType);

    // Act
    bool result = type.IsDerivedFromGenericType (genericType: typeof (GenericBase<>));

    // Assert
    Assert.IsTrue (condition: result);
  }

  [TestMethod]
  public void IsDerivedFromGenericType_WhenTypeDoesNotDeriveFromGenericBase_ReturnsFalse ()
  {
    // Arrange
    Type type = typeof (string);

    // Act
    bool result = type.IsDerivedFromGenericType (genericType: typeof (GenericBase<>));

    // Assert
    Assert.IsFalse (condition: result);
  }

  [TestMethod]
  public void Tau_WhenAccessed_EqualsTwoPi ()
  {
    // Act
    double result = Math.Tau;

    // Assert
    Assert.AreEqual (expected: 2.0 * Math.PI, actual: result, delta: 1e-12);
  }

  [TestMethod]
  public void ToDegrees_WhenProvidedRadians_ConvertsToDegrees ()
  {
    // Arrange
    double radians = Math.PI / 2.0;

    // Act
    double result = Math.ToDegrees (radians: radians);

    // Assert
    Assert.AreEqual (expected: 90.0, actual: result, delta: 1e-12);
  }

  [TestMethod]
  public void ToRadians_WhenProvidedDegrees_ConvertsToRadians ()
  {
    // Arrange
    const double degrees = 180.0;

    // Act
    double result = Math.ToRadians (degrees: degrees);

    // Assert
    Assert.AreEqual (expected: Math.PI, actual: result, delta: 1e-12);
  }

  [TestMethod]
  public void GreekLetterConstants_WhenAccessed_MatchExpectedValues ()
  {
    // Act
    double pi  = Math.π;
    double tau = Math.τ;

    // Assert
    Assert.AreEqual (expected: Math.PI,  actual: pi,  delta: 0d);
    Assert.AreEqual (expected: Math.Tau, actual: tau, delta: 0d);
  }
}
