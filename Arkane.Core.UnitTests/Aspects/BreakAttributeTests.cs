#region header

// Arkane.Core.UnitTests - BreakAttributeTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-07 10:31 AM

#endregion

#region using

using System.Diagnostics;

using ArkaneSystems.Arkane.Aspects.Diagnostics;

using JetBrains.Annotations;

#endregion

namespace Arkane.Core.UnitTests.Aspects;

/// <summary>
///   Unit tests for the behavior of the <see cref="BreakBeforeAttribute" /> and
///   <see cref="BreakAfterAttribute" /> aspects when applied to target methods.
/// </summary>
/// <remarks>
///   These tests verify that the aspects correctly wrap the target method body: the original method body
///   still executes and returns the expected value, regardless of the debugger break injection.
///   The tests are designed to run safely without a debugger attached; <see cref="Debugger.Break" />
///   is a no-op when no debugger is present in .NET.
/// </remarks>
[TestClass]
public class BreakAttributeTests
{
  // ---- BreakBefore tests ----

  /// <summary>
  ///   Verifies that a method decorated with <see cref="BreakBeforeAttribute" /> still executes its original
  ///   body and returns the expected value.
  /// </summary>
  [TestMethod]
  public void BreakBefore_MethodReturningInt_ExecutesBodyAndReturnsExpectedValue ()
  {
    // Arrange
    var target = new BreakBeforeTarget ();

    // Act
    int result = target.MethodReturningInt ();

    // Assert
    Assert.IsTrue (condition: target.BodyExecuted);
    Assert.AreEqual (expected: 42, actual: result);
  }

  /// <summary>
  ///   Verifies that a method returning a <see cref="string" /> decorated with <see cref="BreakBeforeAttribute" />
  ///   still executes its original body and returns the expected value.
  /// </summary>
  [TestMethod]
  public void BreakBefore_MethodReturningString_ExecutesBodyAndReturnsExpectedValue ()
  {
    // Arrange
    var target = new BreakBeforeTarget ();

    // Act
    string result = target.MethodReturningString ();

    // Assert
    Assert.IsTrue (condition: target.BodyExecuted);
    Assert.AreEqual (expected: "hello", actual: result);
  }

  /// <summary>
  ///   Verifies that a <see langword="void" /> method decorated with <see cref="BreakBeforeAttribute" />
  ///   still executes its original body.
  /// </summary>
  [TestMethod]
  public void BreakBefore_VoidMethod_ExecutesBody ()
  {
    // Arrange
    var target = new BreakBeforeTarget ();

    // Act
    target.VoidMethod ();

    // Assert
    Assert.IsTrue (condition: target.BodyExecuted);
  }

  // ---- BreakAfter tests ----

  /// <summary>
  ///   Verifies that a method decorated with <see cref="BreakAfterAttribute" /> still executes its original
  ///   body and returns the expected value.
  /// </summary>
  [TestMethod]
  public void BreakAfter_MethodReturningInt_ExecutesBodyAndReturnsExpectedValue ()
  {
    // Arrange
    var target = new BreakAfterTarget ();

    // Act
    int result = target.MethodReturningInt ();

    // Assert
    Assert.IsTrue (condition: target.BodyExecuted);
    Assert.AreEqual (expected: 42, actual: result);
  }

  /// <summary>
  ///   Verifies that a method returning a <see cref="string" /> decorated with <see cref="BreakAfterAttribute" />
  ///   still executes its original body and returns the expected value.
  /// </summary>
  [TestMethod]
  public void BreakAfter_MethodReturningString_ExecutesBodyAndReturnsExpectedValue ()
  {
    // Arrange
    var target = new BreakAfterTarget ();

    // Act
    string result = target.MethodReturningString ();

    // Assert
    Assert.IsTrue (condition: target.BodyExecuted);
    Assert.AreEqual (expected: "hello", actual: result);
  }

  /// <summary>
  ///   Verifies that a <see langword="void" /> method decorated with <see cref="BreakAfterAttribute" />
  ///   still executes its original body.
  /// </summary>
  [TestMethod]
  public void BreakAfter_VoidMethod_ExecutesBody ()
  {
    // Arrange
    var target = new BreakAfterTarget ();

    // Act
    target.VoidMethod ();

    // Assert
    Assert.IsTrue (condition: target.BodyExecuted);
  }

  /// <summary>
  ///   Verifies that the return value captured before the debugger break is correctly returned to the caller
  ///   when using <see cref="BreakAfterAttribute" />.
  /// </summary>
  [TestMethod]
  public void BreakAfter_MethodReturningInt_ReturnValueIsPreservedAfterBreak ()
  {
    // Arrange
    var target = new BreakAfterTarget ();

    // Act
    int result = target.MethodReturningInt ();

    // Assert — the return value captured before Debugger.Break() is correctly passed back to the caller
    Assert.AreEqual (expected: 42, actual: result);
  }

  #region Test targets

  /// <summary>
  ///   Helper class with <see cref="BreakBeforeAttribute" /> applied to various target methods.
  /// </summary>
  private sealed class BreakBeforeTarget
  {
    /// <summary>
    ///   Tracks whether the method body was executed, to verify that <c>meta.Proceed()</c> is called
    ///   after the debugger break.
    /// </summary>
    public bool BodyExecuted { get; private set; }

    /// <summary>
    ///   A method whose body returns 42. The aspect injects a debugger break before the body executes.
    /// </summary>
    [BreakBefore]
    public int MethodReturningInt ()
    {
      this.BodyExecuted = true;

      return 42;
    }

    /// <summary>
    ///   A method whose body returns "hello". The aspect injects a debugger break before the body executes.
    /// </summary>
    [BreakBefore]
    public string MethodReturningString ()
    {
      this.BodyExecuted = true;

      return "hello";
    }

    /// <summary>
    ///   A <see langword="void" /> method with a side effect. The aspect injects a debugger break before the body executes.
    /// </summary>
    [BreakBefore]
    public void VoidMethod () { this.BodyExecuted = true; }
  }

  /// <summary>
  ///   Helper class with <see cref="BreakAfterAttribute" /> applied to various target methods.
  /// </summary>
  private sealed class BreakAfterTarget
  {
    /// <summary>
    ///   Tracks whether the method body was executed, to verify that <c>meta.Proceed()</c> is called
    ///   before the debugger break.
    /// </summary>
    public bool BodyExecuted { get; private set; }

    /// <summary>
    ///   A method whose body returns 42. The aspect injects a debugger break after the body executes.
    /// </summary>
    [BreakAfter]
    public int MethodReturningInt ()
    {
      this.BodyExecuted = true;

      return 42;
    }

    /// <summary>
    ///   A method whose body returns "hello". The aspect injects a debugger break after the body executes.
    /// </summary>
    [BreakAfter]
    public string MethodReturningString ()
    {
      this.BodyExecuted = true;

      return "hello";
    }

    /// <summary>
    ///   A <see langword="void" /> method with a side effect. The aspect injects a debugger break after the body executes.
    /// </summary>
    [BreakAfter]
    public void VoidMethod () { this.BodyExecuted = true; }
  }

  #endregion
}
