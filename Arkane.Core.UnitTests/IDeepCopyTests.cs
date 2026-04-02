#region header

// Arkane.Core.UnitTests - IDeepCopyTests.cs
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

using Moq;

#endregion

namespace Arkane.Core.UnitTests;

/// <summary>
///   Unit tests for IDeepCopy interface.
/// </summary>
[TestClass]

// ReSharper disable once InconsistentNaming
public class IDeepCopyTests
{
  #region Nested type: DerivedTestClass

  /// <summary>
  ///   Derived test helper class.
  /// </summary>
  [UsedImplicitly]
  public class DerivedTestClass : TestClass
  {
    public int DerivedValue { get; set; }
  }

  #endregion

  #region Nested type: TestClass

  /// <summary>
  ///   Test helper class.
  /// </summary>
  public class TestClass
  {
    public int Value { get; set; }

    public string? Name { get; set; }
  }

  #endregion

  /// <summary>
  ///   Tests that DeepCopy returns a new instance.
  /// </summary>
  [TestMethod]
  public void DeepCopy_WhenCalled_ReturnsNewInstance ()
  {
    // Arrange
    var mockDeepCopy = new Mock<IDeepCopy<TestClass>> ();
    var copy         = new TestClass { Value = 42 };
    mockDeepCopy.Setup (x => x.DeepCopy ()).Returns (copy);

    // Act
    TestClass result = mockDeepCopy.Object.DeepCopy ();

    // Assert
    Assert.IsNotNull (result);
    mockDeepCopy.Verify (expression: x => x.DeepCopy (), times: Times.Once);
  }

  /// <summary>
  ///   Tests that DeepCopy can be called multiple times.
  /// </summary>
  [TestMethod]
  public void DeepCopy_WhenCalledMultipleTimes_ReturnsMultipleInstances ()
  {
    // Arrange
    var mockDeepCopy = new Mock<IDeepCopy<TestClass>> ();
    var copy1        = new TestClass { Value = 1 };
    var copy2        = new TestClass { Value = 2 };
    mockDeepCopy.SetupSequence (x => x.DeepCopy ())
                .Returns (copy1)
                .Returns (copy2);

    // Act
    TestClass result1 = mockDeepCopy.Object.DeepCopy ();
    TestClass result2 = mockDeepCopy.Object.DeepCopy ();

    // Assert
    Assert.IsNotNull (result1);
    Assert.IsNotNull (result2);
    Assert.AreNotSame (notExpected: result1, actual: result2);
    mockDeepCopy.Verify (expression: x => x.DeepCopy (), times: Times.Exactly (2));
  }

  /// <summary>
  ///   Tests that DeepCopy returns instance with copied field values.
  /// </summary>
  [TestMethod]
  public void DeepCopy_WhenCalled_ReturnsCopiedFieldValues ()
  {
    // Arrange
    var mockDeepCopy = new Mock<IDeepCopy<TestClass>> ();
    var copy         = new TestClass { Value = 42, Name = "Test" };
    mockDeepCopy.Setup (x => x.DeepCopy ()).Returns (copy);

    // Act
    TestClass result = mockDeepCopy.Object.DeepCopy ();

    // Assert
    Assert.AreEqual (expected: 42,     actual: result.Value);
    Assert.AreEqual (expected: "Test", actual: result.Name);
  }

  /// <summary>
  ///   Tests that DeepCopy works with string type.
  /// </summary>
  [TestMethod]
  public void DeepCopy_WithStringType_ReturnsString ()
  {
    // Arrange
    var mockDeepCopy = new Mock<IDeepCopy<string>> ();
    mockDeepCopy.Setup (x => x.DeepCopy ()).Returns ("copy");

    // Act
    string result = mockDeepCopy.Object.DeepCopy ();

    // Assert
    Assert.AreEqual (expected: "copy", actual: result);
  }

  /// <summary>
  ///   Tests that DeepCopy works with value type.
  /// </summary>
  [TestMethod]
  public void DeepCopy_WithValueType_ReturnsValue ()
  {
    // Arrange
    var mockDeepCopy = new Mock<IDeepCopy<int>> ();
    mockDeepCopy.Setup (x => x.DeepCopy ()).Returns (42);

    // Act
    int result = mockDeepCopy.Object.DeepCopy ();

    // Assert
    Assert.AreEqual (expected: 42, actual: result);
  }

  /// <summary>
  ///   Tests that IDeepCopy implements ICloneable.
  /// </summary>
  [TestMethod]
  public void DeepCopy_InterfaceImplementsICloneable_IsAssignable ()
  {
    // Arrange & Act
    var mockDeepCopy = new Mock<IDeepCopy<TestClass>> ();
    var copy         = new TestClass { Value = 42 };
    mockDeepCopy.Setup (x => x.DeepCopy ()).Returns (copy);
    mockDeepCopy.As<ICloneable> ().Setup (x => x.Clone ()).Returns (copy);
    IDeepCopy<TestClass> instance = mockDeepCopy.Object;

    // Assert
    Assert.IsInstanceOfType<ICloneable> (instance);
  }

  /// <summary>
  ///   Tests that ICloneable.Clone can call DeepCopy.
  /// </summary>
  [TestMethod]
  public void DeepCopy_ICloneableCloneCalled_CanDelegateToDeepCopy ()
  {
    // Arrange
    var mockDeepCopy = new Mock<IDeepCopy<TestClass>> ();
    var copy         = new TestClass { Value = 42 };
    mockDeepCopy.Setup (x => x.DeepCopy ()).Returns (copy);
    mockDeepCopy.As<ICloneable> ().Setup (x => x.Clone ()).Returns (() => mockDeepCopy.Object.DeepCopy ());
    var cloneable = (ICloneable)mockDeepCopy.Object;

    // Act
    object? result = cloneable.Clone ();

    // Assert
    Assert.IsNotNull (result);
    Assert.AreSame (expected: copy, actual: result);
  }

  /// <summary>
  ///   Tests that DeepCopy supports covariance with derived types.
  /// </summary>
  [TestMethod]
  public void DeepCopy_WithCovariance_SupportsAssignmentToDerivedType ()
  {
    // Arrange
    var mockDeepCopy = new Mock<IDeepCopy<DerivedTestClass>> ();
    var copy         = new DerivedTestClass { Value = 42, DerivedValue = 100 };
    mockDeepCopy.Setup (x => x.DeepCopy ()).Returns (copy);
    IDeepCopy<TestClass> baseInterface = mockDeepCopy.Object;

    // Act
    TestClass result = baseInterface.DeepCopy ();

    // Assert
    Assert.IsNotNull (result);
    Assert.IsInstanceOfType<DerivedTestClass> (result);
  }
}
