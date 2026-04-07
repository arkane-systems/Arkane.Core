#region header

// Arkane.Core.UnitTests - NoOpAttributeTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 6:30 PM

#endregion

#region using

using ArkaneSystems.Arkane.Aspects.Diagnostics;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests.Aspects;

/// <summary>
///   Unit tests for the behavior of the <see cref="NoOpAttribute" /> aspect when applied to target methods.
/// </summary>
[TestClass]
public class NoOpAttributeTests
{
  #region Nested type: Target

  #region Test target

  /// <summary>
  ///   Helper class with the <see cref="NoOpAttribute" /> aspect applied to various target methods.
  /// </summary>
  private sealed class Target
  {
    /// <summary>
    ///   A method whose body would return 42, but is replaced with a no-op by the aspect.
    /// </summary>
    [NoOp]
    public int MethodReturningInt () => 42;

    /// <summary>
    ///   A method whose body would return "hello", but is replaced with a no-op by the aspect.
    /// </summary>
    [NoOp]
    public string? MethodReturningString () => "hello";

    /// <summary>
    ///   A method whose body would return <see langword="true" />, but is replaced with a no-op by the aspect.
    /// </summary>
    [NoOp]
    public bool MethodReturningBool () => true;

    /// <summary>
    ///   A method whose body would throw, but is replaced with a no-op by the aspect.
    /// </summary>
    [NoOp]
    public void VoidMethod () => throw new InvalidOperationException ("Original body must not execute.");

    /// <summary>
    ///   A method whose body would throw, but is replaced with a no-op by the aspect. Returns a reference type.
    /// </summary>
    [NoOp]
    public object? MethodReturningObject () => throw new InvalidOperationException ("Original body must not execute.");
  }

  #endregion

  #endregion

  /// <summary>
  ///   Verifies that a method returning <see cref="int" /> decorated with <see cref="NoOpAttribute" />
  ///   returns the default value of zero instead of executing its original body.
  /// </summary>
  [TestMethod]
  public void MethodReturningInt_WhenAspectApplied_ReturnsDefaultInt ()
  {
    // Arrange
    var target = new Target ();

    // Act
    int result = target.MethodReturningInt ();

    // Assert
    Assert.AreEqual (expected: default (int), actual: result);
  }

  /// <summary>
  ///   Verifies that a method returning <see cref="string" /> decorated with <see cref="NoOpAttribute" />
  ///   returns <see langword="null" /> instead of executing its original body.
  /// </summary>
  [TestMethod]
  public void MethodReturningString_WhenAspectApplied_ReturnsDefaultString ()
  {
    // Arrange
    var target = new Target ();

    // Act
    string? result = target.MethodReturningString ();

    // Assert
    Assert.AreEqual (expected: default (string), actual: result);
  }

  /// <summary>
  ///   Verifies that a method returning <see cref="bool" /> decorated with <see cref="NoOpAttribute" />
  ///   returns the default value of <see langword="false" /> instead of executing its original body.
  /// </summary>
  [TestMethod]
  public void MethodReturningBool_WhenAspectApplied_ReturnsDefaultBool ()
  {
    // Arrange
    var target = new Target ();

    // Act
    bool result = target.MethodReturningBool ();

    // Assert
    Assert.AreEqual (expected: default (bool), actual: result);
  }

  /// <summary>
  ///   Verifies that a <see langword="void" /> method decorated with <see cref="NoOpAttribute" />
  ///   completes without throwing, even if the original body would have thrown an exception.
  /// </summary>
  [TestMethod]
  public void VoidMethod_WhenAspectApplied_DoesNotThrow ()
  {
    // Arrange
    var target = new Target ();

    // Act & Assert
    target.VoidMethod (); // Should not throw
  }

  /// <summary>
  ///   Verifies that a method returning a reference type decorated with <see cref="NoOpAttribute" />
  ///   returns <see langword="null" /> instead of executing its original body.
  /// </summary>
  [TestMethod]
  public void MethodReturningObject_WhenAspectApplied_ReturnsNull ()
  {
    // Arrange
    var target = new Target ();

    // Act
    object? result = target.MethodReturningObject ();

    // Assert
    Assert.IsNull (value: result);
  }
}
