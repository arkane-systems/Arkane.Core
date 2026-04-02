#region header

// Arkane.Core.UnitTests - IShallowCopyTests.cs
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
///   Tests for the <see cref="IShallowCopy{T}" /> interface.
/// </summary>
[TestClass]

// ReSharper disable once InconsistentNaming
public class IShallowCopyTests
{
  #region Nested type: TestShallowCopyable

  /// <summary>
  ///   Test implementation of IShallowCopy for testing purposes.
  /// </summary>
  private class TestShallowCopyable : IShallowCopy<TestShallowCopyable>
  {
    public int Value { get; set; }

    public object? Reference { get; set; }

    public TestShallowCopyable ShallowCopy () => (TestShallowCopyable)this.MemberwiseClone ();

    object ICloneable.Clone () => this.ShallowCopy ();
  }

  #endregion

  /// <summary>
  ///   Tests that ShallowCopy creates a new instance.
  /// </summary>
  [TestMethod]
  public void ShallowCopy_WhenCalled_CreatesNewInstance ()
  {
    // Arrange
    var original = new TestShallowCopyable { Value = 42, Reference = new object () };

    // Act
    TestShallowCopyable copy = original.ShallowCopy ();

    // Assert
    Assert.IsNotNull (copy);
    Assert.AreNotSame (notExpected: original, actual: copy);
  }

  /// <summary>
  ///   Tests that ShallowCopy returns the correct type.
  /// </summary>
  [TestMethod]
  public void ShallowCopy_WhenCalled_ReturnsCorrectType ()
  {
    // Arrange
    var original = new TestShallowCopyable { Value = 42, Reference = new object () };

    // Act
    TestShallowCopyable copy = original.ShallowCopy ();

    // Assert
    Assert.IsInstanceOfType<TestShallowCopyable> (copy);
  }

  /// <summary>
  ///   Tests that ShallowCopy creates a shallow copy with same value type values.
  /// </summary>
  [TestMethod]
  public void ShallowCopy_WhenCalled_CopiesValueTypeFields ()
  {
    // Arrange
    var original = new TestShallowCopyable { Value = 42, Reference = new object () };

    // Act
    TestShallowCopyable copy = original.ShallowCopy ();

    // Assert
    Assert.AreEqual (expected: original.Value, actual: copy.Value);
  }

  /// <summary>
  ///   Tests that ShallowCopy creates a shallow copy with same reference type references.
  /// </summary>
  [TestMethod]
  public void ShallowCopy_WhenCalled_SharesReferenceTypeFields ()
  {
    // Arrange
    var reference = new object ();
    var original  = new TestShallowCopyable { Value = 42, Reference = reference };

    // Act
    TestShallowCopyable copy = original.ShallowCopy ();

    // Assert
    Assert.AreSame (expected: original.Reference, actual: copy.Reference);
  }

  /// <summary>
  ///   Tests that ShallowCopy maintains independent value type fields.
  /// </summary>
  [TestMethod]
  public void ShallowCopy_WhenValueTypeFieldModified_DoesNotAffectOriginal ()
  {
    // Arrange
    var                 original = new TestShallowCopyable { Value = 42, Reference = new object () };
    TestShallowCopyable copy     = original.ShallowCopy ();

    // Act
    copy.Value = 100;

    // Assert
    Assert.AreEqual (expected: 42,  actual: original.Value);
    Assert.AreEqual (expected: 100, actual: copy.Value);
  }
}
