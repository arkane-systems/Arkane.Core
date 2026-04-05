#region header

// Arkane.Core.UnitTests - ExtensionMethods-System-TypeTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 6:58 PM

#endregion

#region using

using ArkaneSystems.Arkane;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests.Extensions;

[TestClass]
public class ExtensionMethodsSystemTypeTests
{
  #region Nested type: DerivedType

  private sealed class DerivedType : GenericBase<int>;

  #endregion

  #region Nested type: GenericBase

  // ReSharper disable once UnusedTypeParameter
  private class GenericBase<T>;

  #endregion

  #region Nested type: GenericStruct

  #pragma warning disable CS0649 // Field is never assigned to
  private struct GenericStruct<T>
  {
    public T Value;
  }
  #pragma warning restore CS0649

  #endregion

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
  public void IsDerivedFromGenericType_WhenTypeIsInterface_ReturnsFalse ()
  {
    // Arrange
    Type type = typeof (IDisposable);

    // Act
    bool result = type.IsDerivedFromGenericType (genericType: typeof (GenericBase<>));

    // Assert
    Assert.IsFalse (condition: result);
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
  public void GetDefault_WhenTypeIsReferenceType_ReturnsNull ()
  {
    // Arrange
    Type type = typeof (string);

    // Act
    object? result = type.GetDefault ();

    // Assert
    Assert.IsNull (value: result);
  }

  [TestMethod]
  public void GetDefault_WhenTypeIsVoid_ReturnsNull ()
  {
    // Arrange
    Type type = typeof (void);

    // Act
    object? result = type.GetDefault ();

    // Assert
    Assert.IsNull (value: result);
  }

  [TestMethod]
  public void GetDefault_WhenTypeContainsGenericParameters_ThrowsArgumentException ()
  {
    // Arrange
    Type type = typeof (GenericStruct<>);

    // Act & Assert
    var ex = Assert.ThrowsExactly<ArgumentException> (() => type.GetDefault ());
    Assert.IsTrue (condition: ex.Message.Contains (value: "contains generic parameters"));
  }

  [TestMethod]
  public void GetDefault_WhenTypeIsPrimitiveValueType_ReturnsDefaultValue ()
  {
    // Arrange
    Type type = typeof (int);

    // Act
    object? result = type.GetDefault ();

    // Assert
    Assert.IsNotNull (value: result);
    Assert.AreEqual (expected: 0, actual: result);
  }

  [TestMethod]
  public void GetDefault_WhenTypeIsPublicValueType_ReturnsDefaultValue ()
  {
    // Arrange
    Type type = typeof (DateTime);

    // Act
    object? result = type.GetDefault ();

    // Assert
    Assert.IsNotNull (value: result);
    Assert.AreEqual (expected: default (DateTime), actual: result);
  }

  [TestMethod]
  public void IsObjectSetToDefault_WhenObjectValueIsNullAndTypeIsReferenceType_ReturnsTrue ()
  {
    // Arrange
    Type type = typeof (string);

    // Act
    bool result = type.IsObjectSetToDefault (objectValue: null);

    // Assert
    Assert.IsTrue (condition: result);
  }

  [TestMethod]
  public void IsObjectSetToDefault_WhenObjectValueIsNullAndTypeIsValueType_ReturnsFalse ()
  {
    // Arrange
    Type type = typeof (int);

    // Act
    bool result = type.IsObjectSetToDefault (objectValue: null);

    // Assert
    Assert.IsFalse (condition: result);
  }

  [TestMethod]
  public void IsObjectSetToDefault_WhenObjectValueIsDefaultForValueType_ReturnsTrue ()
  {
    // Arrange
    Type type = typeof (int);

    // Act
    bool result = type.IsObjectSetToDefault (objectValue: 0);

    // Assert
    Assert.IsTrue (condition: result);
  }

  [TestMethod]
  public void IsObjectSetToDefault_WhenObjectValueIsNotDefaultForValueType_ReturnsFalse ()
  {
    // Arrange
    Type type = typeof (int);

    // Act
    bool result = type.IsObjectSetToDefault (objectValue: 42);

    // Assert
    Assert.IsFalse (condition: result);
  }

  [TestMethod]
  public void IsObjectSetToDefault_WhenObjectValueIsNonNullForReferenceType_ReturnsFalse ()
  {
    // Arrange
    Type type = typeof (string);

    // Act
    bool result = type.IsObjectSetToDefault (objectValue: "test");

    // Assert
    Assert.IsFalse (condition: result);
  }
}
