#region header

// Arkane.Core.UnitTests - ExtensionMethods-System-MathTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-02 7:20 PM

#endregion

#region using

using ArkaneSystems.Arkane;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests.Extensions;

[TestClass]
public class ExtensionMethodsSystemMathTests
{
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
