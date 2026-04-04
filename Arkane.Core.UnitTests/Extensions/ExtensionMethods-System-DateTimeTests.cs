#region header

// Arkane.Core.UnitTests - ExtensionMethods-System-DateTimeTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-03 3:59 PM

#endregion

#region using

using ArkaneSystems.Arkane;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests.Extensions;

[TestClass]
public class ExtensionMethodsSystemDateTimeTests
{
  [TestMethod]
  public void ToLinuxTimeStamp_WhenDateIsLinuxEpoch_ReturnsZero ()
  {
    // Arrange
    DateTime linuxEpoch = Epochs.LinuxEpoch;

    // Act
    double result = linuxEpoch.ToLinuxTimeStamp ();

    // Assert
    Assert.AreEqual (expected: 0d, actual: result);
  }

  [TestMethod]
  public void ToLinuxTimeStamp_WhenDateIsAfterEpoch_ReturnsPositiveValue ()
  {
    // Arrange
    DateTime date = new (year: 2024, month: 1, day: 1, hour: 0, minute: 0, second: 0, millisecond: 0);

    // Act
    double result = date.ToLinuxTimeStamp ();

    // Assert
    Assert.IsGreaterThan (lowerBound: 0d, value: result);
  }

  [TestMethod]
  public void ToLinuxTimeStamp_WhenDateIsBeforeEpoch_ReturnsNegativeValue ()
  {
    // Arrange
    DateTime date = new (year: 1960, month: 1, day: 1, hour: 0, minute: 0, second: 0, millisecond: 0);

    // Act
    double result = date.ToLinuxTimeStamp ();

    // Assert
    Assert.IsLessThan (upperBound: 0d, value: result);
  }

  [TestMethod]
  public void ToLinuxTimeStamp_WhenDateHasMilliseconds_RoundsToNearestInteger ()
  {
    // Arrange
    DateTime date = Epochs.LinuxEpoch.AddMilliseconds (value: 1000.7);

    // Act
    double result = date.ToLinuxTimeStamp ();

    // Assert
    Assert.AreEqual (expected: 1001d, actual: result);
  }

  [TestMethod]
  public void FromLinuxTimeStamp_WhenTimestampIsZero_ReturnsLinuxEpoch ()
  {
    // Arrange
    const double timestamp = 0d;

    // Act
    DateTime result = DateTime.FromLinuxTimeStamp (timestamp: timestamp);

    // Assert
    Assert.AreEqual (expected: Epochs.LinuxEpoch, actual: result);
  }

  [TestMethod]
  public void FromLinuxTimeStamp_WhenTimestampIsPositive_ReturnsDateAfterEpoch ()
  {
    // Arrange
    const double timestamp = 1000000d;

    // Act
    DateTime result = DateTime.FromLinuxTimeStamp (timestamp: timestamp);

    // Assert
    Assert.IsTrue (condition: result > Epochs.LinuxEpoch);
  }

  [TestMethod]
  public void FromLinuxTimeStamp_WhenTimestampIsNegative_ReturnsDateBeforeEpoch ()
  {
    // Arrange
    const double timestamp = -1000000d;

    // Act
    DateTime result = DateTime.FromLinuxTimeStamp (timestamp: timestamp);

    // Assert
    Assert.IsTrue (condition: result < Epochs.LinuxEpoch);
  }

  [TestMethod]
  public void ToLinuxTimeStamp_RoundTrip_PreservesTimestamp ()
  {
    // Arrange
    const double expectedTimestamp = 1234567890000d;

    // Act
    DateTime date            = DateTime.FromLinuxTimeStamp (timestamp: expectedTimestamp);
    double   actualTimestamp = date.ToLinuxTimeStamp ();

    // Assert
    Assert.AreEqual (expected: expectedTimestamp, actual: actualTimestamp);
  }

  [TestMethod]
  public void ToLinuxTimeStamp_WhenRoundingDown_RoundsToNearestInteger ()
  {
    // Arrange
    DateTime date = Epochs.LinuxEpoch.AddMilliseconds (value: 1000.4);

    // Act
    double result = date.ToLinuxTimeStamp ();

    // Assert
    Assert.AreEqual (expected: 1000d, actual: result);
  }

  [TestMethod]
  public void ToLinuxTimeStamp_WhenExactlyHalfMillisecond_RoundsAwayFromZero ()
  {
    // Arrange
    DateTime date = Epochs.LinuxEpoch.AddMilliseconds (value: 1000.5);

    // Act
    double result = date.ToLinuxTimeStamp ();

    // Assert
    Assert.AreEqual (expected: 1001d, actual: result);
  }
}
