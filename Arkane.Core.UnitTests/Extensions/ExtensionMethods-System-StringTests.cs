#region header

// Arkane.Core.UnitTests - ExtensionMethods-System-StringTests.cs
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
public class ExtensionMethodsSystemStringTests
{
  [TestMethod]
  public void UpTo_WhenInputIsNull_ReturnsEmptyString ()
  {
    // Arrange
    string? input = null;

    // Act
    #pragma warning disable CS8604 // Possible null reference argument.
    string result = input.UpTo (maxLength: 3);
    #pragma warning restore CS8604 // Possible null reference argument.

    // Assert
    Assert.AreEqual (expected: string.Empty, actual: result);
  }

  [TestMethod]
  public void UpTo_WhenInputLengthExceedsMaxLength_TruncatesAndAppendsSuffix ()
  {
    // Arrange
    const string input = "abcdef";

    // Act
    string result = input.UpTo (maxLength: 3, suffixToUseWhenTooLong: "..*");

    // Assert
    Assert.AreEqual (expected: "abc..*", actual: result);
  }

  [TestMethod]
  public void RemoveNonAlphanumeric_WhenInputContainsMixedCharacters_RemovesNonAlphanumericCharacters ()
  {
    // Arrange
    const string input = "Ab-12_!?C";

    // Act
    string result = input.RemoveNonAlphanumeric ();

    // Assert
    Assert.AreEqual (expected: "Ab12C", actual: result);
  }
}
