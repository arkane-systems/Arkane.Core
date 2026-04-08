#region header

// Arkane.Core.UnitTests - DivisibleByAttributeTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 10:00 PM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests.Aspects;

/// <summary>
///   Unit tests for the behavior of the <see cref="DivisibleByAttribute" /> aspect when applied to parameters
///   and properties.
/// </summary>
[TestClass]
public class DivisibleByAttributeTests
{
  #region Test targets

  /// <summary>
  ///   Helper class with the <see cref="DivisibleByAttribute" /> aspect applied to parameters and properties.
  /// </summary>
  private sealed class Target
  {
    private int    _intProperty;
    private long   _longProperty;
    private decimal _decimalProperty;

    /// <summary>
    ///   A method whose <c>value</c> parameter must be divisible by 4.
    /// </summary>
    public void AcceptIntDivisibleBy4 ([DivisibleBy (4)] int value) { }

    /// <summary>
    ///   A method whose <c>value</c> parameter must be divisible by 3.
    /// </summary>
    public void AcceptLongDivisibleBy3 ([DivisibleBy (3)] long value) { }

    /// <summary>
    ///   A method whose <c>value</c> parameter must be divisible by 5.
    /// </summary>
    public void AcceptDecimalDivisibleBy5 ([DivisibleBy (5)] decimal value) { }

    /// <summary>
    ///   A method with multiple parameters, each with their own divisibility constraint.
    /// </summary>
    public void AcceptMultipleConstrained ([DivisibleBy (2)] int a, [DivisibleBy (3)] int b) { }

    /// <summary>
    ///   An integer property whose value must be divisible by 6.
    /// </summary>
    [DivisibleBy (6)]
    public int IntPropertyDivisibleBy6
    {
      get => _intProperty;
      set => _intProperty = value;
    }

    /// <summary>
    ///   A long property whose value must be divisible by 4.
    /// </summary>
    [DivisibleBy (4)]
    public long LongPropertyDivisibleBy4
    {
      get => _longProperty;
      set => _longProperty = value;
    }

    /// <summary>
    ///   A decimal property whose value must be divisible by 7.
    /// </summary>
    [DivisibleBy (7)]
    public decimal DecimalPropertyDivisibleBy7
    {
      get => _decimalProperty;
      set => _decimalProperty = value;
    }
  }

  #endregion

  // ---- Parameter contract tests ----

  /// <summary>
  ///   Verifies that passing a value evenly divisible by the configured divisor to a constrained
  ///   <see cref="int" /> parameter does not throw an exception.
  /// </summary>
  [TestMethod]
  public void IntParameter_WithDivisibleValue_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.AcceptIntDivisibleBy4 (value: 8); // 8 % 4 == 0
  }

  /// <summary>
  ///   Verifies that passing a value not evenly divisible by the configured divisor to a constrained
  ///   <see cref="int" /> parameter throws an <see cref="ArgumentException" />.
  /// </summary>
  [TestMethod]
  public void IntParameter_WithNonDivisibleValue_ThrowsArgumentException ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    Assert.ThrowsExactly<ArgumentException> (() => target.AcceptIntDivisibleBy4 (value: 7)); // 7 % 4 != 0
  }

  /// <summary>
  ///   Verifies that passing zero (which is divisible by any non-zero divisor) to a constrained
  ///   <see cref="int" /> parameter does not throw an exception.
  /// </summary>
  [TestMethod]
  public void IntParameter_WithZeroValue_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.AcceptIntDivisibleBy4 (value: 0); // 0 % 4 == 0
  }

  /// <summary>
  ///   Verifies that passing a negative value evenly divisible by the configured divisor to a constrained
  ///   <see cref="int" /> parameter does not throw an exception.
  /// </summary>
  [TestMethod]
  public void IntParameter_WithNegativeDivisibleValue_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.AcceptIntDivisibleBy4 (value: -8); // -8 % 4 == 0
  }

  /// <summary>
  ///   Verifies that passing a value evenly divisible by the configured divisor to a constrained
  ///   <see cref="long" /> parameter does not throw an exception.
  /// </summary>
  [TestMethod]
  public void LongParameter_WithDivisibleValue_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.AcceptLongDivisibleBy3 (value: 9L); // 9 % 3 == 0
  }

  /// <summary>
  ///   Verifies that passing a value not evenly divisible by the configured divisor to a constrained
  ///   <see cref="long" /> parameter throws an <see cref="ArgumentException" />.
  /// </summary>
  [TestMethod]
  public void LongParameter_WithNonDivisibleValue_ThrowsArgumentException ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    Assert.ThrowsExactly<ArgumentException> (() => target.AcceptLongDivisibleBy3 (value: 10L)); // 10 % 3 != 0
  }

  /// <summary>
  ///   Verifies that passing a value evenly divisible by the configured divisor to a constrained
  ///   <see cref="decimal" /> parameter does not throw an exception.
  /// </summary>
  [TestMethod]
  public void DecimalParameter_WithDivisibleValue_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.AcceptDecimalDivisibleBy5 (value: 25m); // 25 % 5 == 0
  }

  /// <summary>
  ///   Verifies that passing a value not evenly divisible by the configured divisor to a constrained
  ///   <see cref="decimal" /> parameter throws an <see cref="ArgumentException" />.
  /// </summary>
  [TestMethod]
  public void DecimalParameter_WithNonDivisibleValue_ThrowsArgumentException ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    Assert.ThrowsExactly<ArgumentException> (() => target.AcceptDecimalDivisibleBy5 (value: 23m)); // 23 % 5 != 0
  }

  /// <summary>
  ///   Verifies that a method with multiple constrained parameters validates each independently, and that a
  ///   violation on the first parameter throws <see cref="ArgumentException" />.
  /// </summary>
  [TestMethod]
  public void MultipleParameters_WhenFirstIsInvalid_ThrowsArgumentException ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    Assert.ThrowsExactly<ArgumentException> (() => target.AcceptMultipleConstrained (a: 3, b: 6)); // 3 % 2 != 0
  }

  /// <summary>
  ///   Verifies that a method with multiple constrained parameters validates each independently, and that a
  ///   violation on the second parameter throws <see cref="ArgumentException" />.
  /// </summary>
  [TestMethod]
  public void MultipleParameters_WhenSecondIsInvalid_ThrowsArgumentException ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    Assert.ThrowsExactly<ArgumentException> (() => target.AcceptMultipleConstrained (a: 4, b: 5)); // 5 % 3 != 0
  }

  /// <summary>
  ///   Verifies that a method with multiple constrained parameters allows the call when all parameters satisfy
  ///   their respective constraints.
  /// </summary>
  [TestMethod]
  public void MultipleParameters_WhenBothAreValid_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.AcceptMultipleConstrained (a: 4, b: 6); // 4 % 2 == 0, 6 % 3 == 0
  }

  // ---- Property contract tests ----

  /// <summary>
  ///   Verifies that assigning a value evenly divisible by the configured divisor to a constrained
  ///   <see cref="int" /> property does not throw an exception.
  /// </summary>
  [TestMethod]
  public void IntProperty_WithDivisibleValue_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.IntPropertyDivisibleBy6 = 12; // 12 % 6 == 0
  }

  /// <summary>
  ///   Verifies that assigning a value not evenly divisible by the configured divisor to a constrained
  ///   <see cref="int" /> property throws an <see cref="ArgumentException" />.
  /// </summary>
  [TestMethod]
  public void IntProperty_WithNonDivisibleValue_ThrowsArgumentException ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    Assert.ThrowsExactly<ArgumentException> (() => { target.IntPropertyDivisibleBy6 = 7; }); // 7 % 6 != 0
  }

  /// <summary>
  ///   Verifies that assigning a value evenly divisible by the configured divisor to a constrained
  ///   <see cref="long" /> property does not throw an exception.
  /// </summary>
  [TestMethod]
  public void LongProperty_WithDivisibleValue_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.LongPropertyDivisibleBy4 = 16L; // 16 % 4 == 0
  }

  /// <summary>
  ///   Verifies that assigning a value not evenly divisible by the configured divisor to a constrained
  ///   <see cref="long" /> property throws an <see cref="ArgumentException" />.
  /// </summary>
  [TestMethod]
  public void LongProperty_WithNonDivisibleValue_ThrowsArgumentException ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    Assert.ThrowsExactly<ArgumentException> (() => { target.LongPropertyDivisibleBy4 = 9L; }); // 9 % 4 != 0
  }

  /// <summary>
  ///   Verifies that assigning a value evenly divisible by the configured divisor to a constrained
  ///   <see cref="decimal" /> property does not throw an exception.
  /// </summary>
  [TestMethod]
  public void DecimalProperty_WithDivisibleValue_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.DecimalPropertyDivisibleBy7 = 49m; // 49 % 7 == 0
  }

  /// <summary>
  ///   Verifies that assigning a value not evenly divisible by the configured divisor to a constrained
  ///   <see cref="decimal" /> property throws an <see cref="ArgumentException" />.
  /// </summary>
  [TestMethod]
  public void DecimalProperty_WithNonDivisibleValue_ThrowsArgumentException ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    Assert.ThrowsExactly<ArgumentException> (() => { target.DecimalPropertyDivisibleBy7 = 50m; }); // 50 % 7 != 0
  }

  /// <summary>
  ///   Verifies that assigning a valid value to a constrained property actually stores and retrieves the value.
  /// </summary>
  [TestMethod]
  public void IntProperty_WithDivisibleValue_StoresTheValue ()
  {
    // Arrange
    var target = new Target ();

    // Act
    target.IntPropertyDivisibleBy6 = 18;

    // Assert
    Assert.AreEqual (expected: 18, actual: target.IntPropertyDivisibleBy6);
  }
}
