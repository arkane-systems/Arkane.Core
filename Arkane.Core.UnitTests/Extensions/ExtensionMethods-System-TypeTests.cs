#region header

// Arkane.Core.UnitTests - ExtensionMethods-System-TypeTests.cs
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
public class ExtensionMethodsSystemTypeTests
{
  #region Nested type: DerivedType

  private sealed class DerivedType : GenericBase<int>;

  #endregion

  #region Nested type: GenericBase

  // ReSharper disable once UnusedTypeParameter
  private class GenericBase<T>;

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
}
