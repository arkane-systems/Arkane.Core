#region header

// Arkane.Core - SubclassResponsibilityException.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-02 11:09 PM

#endregion

#region using

using ArkaneSystems.Arkane.Properties;

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Exception thrown to indicate that implementing a particular method or property is the responsibility
///   of a subclass of the class which has thrown the exception.
/// </summary>
/// <remarks>
///   <para>
///     This is essentially a special case of <see cref="T:System.NotImplementedException" />, relating to
///     functionality that is not implemented in a superclass because subclasses are intended to
///     implement it on its behalf.
///   </para>
///   <para>
///     Before throwing this, bethink you whether the class you're implementing really ought to be an
///     abstract class instead.  That is to be preferred; this is the second choice.
///   </para>
/// </remarks>
[PublicAPI]
[Serializable]
public sealed class SubclassResponsibilityException : NotImplementedException
{
  /// <inheritdoc />
  public SubclassResponsibilityException ()
    : base (Resources.SubclassResponsibilityException_SubclassResponsibilityException_DefaultMessage)
  { }

  /// <inheritdoc />
  public SubclassResponsibilityException (string message) : base (message) { }

  /// <inheritdoc />
  public SubclassResponsibilityException (string message, Exception innerException) : base (message: message,
                                                                                            inner: innerException)
  { }
}
