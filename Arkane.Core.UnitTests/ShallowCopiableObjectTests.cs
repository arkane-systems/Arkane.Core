#region header

// Arkane.Core.UnitTests - ShallowCopiableObjectTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 7:21 PM

#endregion

#region using

using ArkaneSystems.Arkane;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests;

[TestClass]
public class ShallowCopiableObjectTests
{
  #region Nested type: DerivedShallowCopiableObject

  private sealed class DerivedShallowCopiableObject : ShallowCopiableObject
  {
    public object? Reference { get; init; }
  }

  #endregion

  [TestMethod]
  public void ShallowCopy_WhenCalled_CreatesDifferentInstanceOfSameRuntimeType ()
  {
    // Arrange
    var original = new DerivedShallowCopiableObject { Reference = new object () };

    // Act
    ShallowCopiableObject copy = original.ShallowCopy ();

    // Assert
    Assert.AreNotSame (notExpected: original, actual: copy);
    Assert.AreEqual (expected: original.GetType (), actual: copy.GetType ());
  }

  [TestMethod]
  public void ShallowCopy_WhenReferencePropertyIsSet_PreservesSameReference ()
  {
    // Arrange
    var reference = new object ();
    var original  = new DerivedShallowCopiableObject { Reference = reference };

    // Act
    var copy = (DerivedShallowCopiableObject)original.ShallowCopy ();

    // Assert
    Assert.AreSame (expected: reference, actual: copy.Reference);
  }

  [TestMethod]
  public void Clone_WhenCalled_DelegatesToShallowCopy ()
  {
    // Arrange
    var original = new DerivedShallowCopiableObject { Reference = new object () };

    // Act
    var clone = (DerivedShallowCopiableObject)((ICloneable)original).Clone ();

    // Assert
    Assert.AreNotSame (notExpected: original, actual: clone);
    Assert.AreSame (expected: original.Reference, actual: clone.Reference);
  }
}
