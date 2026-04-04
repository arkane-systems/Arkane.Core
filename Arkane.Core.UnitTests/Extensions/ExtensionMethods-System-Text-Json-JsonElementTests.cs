#region header

// Arkane.Core.UnitTests - ExtensionMethods-System-Text-Json-JsonElementTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 11:00 PM

#endregion

#region using

using System.Text.Json;

using ArkaneSystems.Arkane;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests.Extensions;

[TestClass]
public class ExtensionMethodsSystemTextJsonJsonElementTests
{
  #region IsNull

  [TestMethod]
  public void IsNull_WhenValueKindIsNull_ReturnsTrue ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("null").RootElement;

    // Act & Assert
    Assert.IsTrue (condition: element.IsNull ());
  }

  [TestMethod]
  public void IsNull_WhenValueKindIsObject_ReturnsFalse ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("{}").RootElement;

    // Act & Assert
    Assert.IsFalse (condition: element.IsNull ());
  }

  [TestMethod]
  public void IsNull_WhenValueKindIsString_ReturnsFalse ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("\"hello\"").RootElement;

    // Act & Assert
    Assert.IsFalse (condition: element.IsNull ());
  }

  #endregion

  #region GetStringProperty

  [TestMethod]
  public void GetStringProperty_WhenObjectHasStringProperty_ReturnsPropertyValue ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("{\"name\":\"Alice\"}").RootElement;

    // Act
    string? result = element.GetStringProperty (propertyName: "name");

    // Assert
    Assert.AreEqual (expected: "Alice", actual: result);
  }

  [TestMethod]
  public void GetStringProperty_WhenPropertyIsMissing_ReturnsDefaultValue ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("{}").RootElement;

    // Act
    string? result = element.GetStringProperty (propertyName: "missing", defaultValue: "fallback");

    // Assert
    Assert.AreEqual (expected: "fallback", actual: result);
  }

  [TestMethod]
  public void GetStringProperty_WhenPropertyIsMissing_ReturnsNull ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("{}").RootElement;

    // Act
    string? result = element.GetStringProperty (propertyName: "missing");

    // Assert
    Assert.IsNull (value: result);
  }

  [TestMethod]
  public void GetStringProperty_WhenElementIsNotObject_ReturnsDefaultValue ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("[1,2,3]").RootElement;

    // Act
    string? result = element.GetStringProperty (propertyName: "name", defaultValue: "fallback");

    // Assert
    Assert.AreEqual (expected: "fallback", actual: result);
  }

  [TestMethod]
  public void GetStringProperty_WhenElementIsNull_ReturnsDefaultValue ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("null").RootElement;

    // Act
    string? result = element.GetStringProperty (propertyName: "name", defaultValue: "fallback");

    // Assert
    Assert.AreEqual (expected: "fallback", actual: result);
  }

  [TestMethod]
  public void GetStringProperty_WhenElementIsNumber_ReturnsDefaultValue ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("42").RootElement;

    // Act
    string? result = element.GetStringProperty (propertyName: "name", defaultValue: "fallback");

    // Assert
    Assert.AreEqual (expected: "fallback", actual: result);
  }

  [TestMethod]
  public void GetStringProperty_WhenPropertyValueIsNotString_ReturnsDefaultValue ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("{\"count\":42}").RootElement;

    // Act
    string? result = element.GetStringProperty (propertyName: "count", defaultValue: "fallback");

    // Assert
    Assert.AreEqual (expected: "fallback", actual: result);
  }

  #endregion

  #region AsDateTime

  [TestMethod]
  public void AsDateTime_WhenObjectHasValidDateProperty_ReturnsDateTime ()
  {
    // Arrange
    JsonElement element = JsonDocument.Parse ("{\"date\":\"2024-01-15T12:00:00Z\"}").RootElement;

    // Act
    DateTime result = element.AsDateTime (propertyName: "date");

    // Assert
    Assert.AreEqual (expected: new DateTime (2024, 1, 15, 12, 0, 0, DateTimeKind.Utc), actual: result);
  }

  [TestMethod]
  public void AsDateTime_WhenPropertyIsMissing_ReturnsDefaultValue ()
  {
    // Arrange
    JsonElement element    = JsonDocument.Parse ("{}").RootElement;
    DateTime    defaultVal = new (2000, 1, 1);

    // Act
    DateTime result = element.AsDateTime (propertyName: "missing", defaultValue: defaultVal);

    // Assert
    Assert.AreEqual (expected: defaultVal, actual: result);
  }

  #endregion
}
