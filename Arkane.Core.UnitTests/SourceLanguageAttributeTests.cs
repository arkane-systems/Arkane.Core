#region header

// Arkane.Core.UnitTests - SourceLanguageAttributeTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 7:21 PM

#endregion

#region using

using ArkaneSystems.Arkane.Annotations;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests;

[TestClass]
public class SourceLanguageAttributeTests
{
  [TestMethod]
  public void Constructor_WhenNoLanguageProvided_DefaultsToCSharp ()
  {
    // Act
    var attribute = new SourceLanguageAttribute ();

    // Assert
    Assert.AreEqual (expected: ProgrammingLanguages.CSharp, actual: attribute.Language);
  }

  [TestMethod]
  public void Constructor_WhenLanguageProvided_StoresProvidedLanguage ()
  {
    // Act
    var attribute = new SourceLanguageAttribute (language: ProgrammingLanguages.FSharp);

    // Assert
    Assert.AreEqual (expected: ProgrammingLanguages.FSharp, actual: attribute.Language);
  }

  [TestMethod]
  public void SettableProperties_WhenAssigned_PreserveAssignedValues ()
  {
    // Arrange
    var attribute = new SourceLanguageAttribute
                    {
                      // Act
                      RewrittenByAspects = true, Obfuscated = true, PostCompilationModifications = "IL weaving",
                    };

    // Assert
    Assert.IsTrue (condition: attribute.RewrittenByAspects);
    Assert.IsTrue (condition: attribute.Obfuscated);
    Assert.AreEqual (expected: "IL weaving", actual: attribute.PostCompilationModifications);
  }
}
