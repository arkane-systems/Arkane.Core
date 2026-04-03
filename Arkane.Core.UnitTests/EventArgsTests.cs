#region header

// Arkane.Core.UnitTests - EventArgsTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.

#endregion

#region using

using ArkaneSystems.Arkane;

#endregion

namespace Arkane.Core.UnitTests;

[TestClass]
public class EventArgsTests
{
  [TestMethod]
  public void Constructor_WhenInitializedWithValue_ExposesValueViaProperty ()
  {
    // Arrange
    const int value = 42;

    // Act
    var args = new EventArgs<int> (value: value);

    // Assert
    Assert.AreEqual (expected: value, actual: args.Value);
  }

  [TestMethod]
  public void Constructor_WhenInitializedWithReference_StoresSameReference ()
  {
    // Arrange
    var payload = new object ();

    // Act
    var args = new EventArgs<object> (value: payload);

    // Assert
    Assert.AreSame (expected: payload, actual: args.Value);
  }
}
