#region header

// Arkane.Core.UnitTests - DeepCopiableObjectTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 10:43 PM

#endregion

#region using

using ArkaneSystems.Arkane;

#endregion

namespace Arkane.Core.UnitTests;

/// <summary>
///   Unit tests for <see cref="DeepCopiableObject" />.
/// </summary>
[TestClass]
public class DeepCopiableObjectTests
{
  #region Nested type: NestedReference

  [Serializable]
  private sealed class NestedReference
  {
    public int Value { get; set; }
  }

  #endregion

  #region Nested type: TestDeepCopiableObject

  [Serializable]
  private sealed class TestDeepCopiableObject : DeepCopiableObject
  {
    public NestedReference? Child { get; set; }

    public TestDeepCopiableObject? SelfReference { get; set; }
  }

  #endregion

  /// <summary>
  ///   Tests that DeepCopy creates an independent nested reference graph.
  /// </summary>
  [TestMethod]
  public void DeepCopy_WhenCalled_CreatesIndependentNestedReferences ()
  {
    // Arrange
    var original = new TestDeepCopiableObject
                   {
                     Child = new NestedReference { Value = 42 }
                   };

    // Act
    var copy = (TestDeepCopiableObject)original.DeepCopy ();

    // Assert
    Assert.AreNotSame (notExpected: original,       actual: copy);
    Assert.IsNotNull (copy.Child);
    Assert.AreNotSame (notExpected: original.Child, actual: copy.Child);
    Assert.AreEqual (expected: 42, actual: copy.Child.Value);
  }

  /// <summary>
  ///   Tests that DeepCopy preserves self-references within the copied graph.
  /// </summary>
  [TestMethod]
  public void DeepCopy_WhenGraphContainsSelfReference_PreservesReferenceLoop ()
  {
    // Arrange
    var original = new TestDeepCopiableObject ();
    original.SelfReference = original;

    // Act
    var copy = (TestDeepCopiableObject)original.DeepCopy ();

    // Assert
    Assert.AreNotSame (notExpected: original, actual: copy);
    Assert.AreSame (expected: copy, actual: copy.SelfReference);
  }
}
