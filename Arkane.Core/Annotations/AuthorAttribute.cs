#region header

// Arkane.Core - AuthorAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 6:56 PM

#endregion

#region using

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using Metalama.Patterns.Contracts;

#endregion

namespace ArkaneSystems.Arkane.Annotations;

[PublicAPI]
[AttributeUsage (AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class AuthorAttribute ([Required] string name, [Required] [Email] string emailAddress)
  : Attribute, IEquatable<AuthorAttribute>
{
  public string Name { get; } = name;

  public string EmailAddress { get; } = emailAddress;

  #region Overrides of Object

  /// <inheritdoc />
  public override string ToString () => $"Author: {this.Name} <{this.EmailAddress}>";

  #endregion

  #region Equality members

  /// <inheritdoc />
  public override bool Equals ([NotNullWhen (true)] object? obj)
    => object.ReferenceEquals (objA: this, objB: obj) || (obj is AuthorAttribute other && this.Equals (other));

  /// <inheritdoc />
  public bool Equals (AuthorAttribute? other)
  {
    if (other is null)
      return false;
    if (object.ReferenceEquals (objA: this, objB: other))
      return true;

    return base.Equals (other) &&
           string.Equals (a: this.EmailAddress, b: other.EmailAddress, comparisonType: StringComparison.OrdinalIgnoreCase);
  }

  /// <inheritdoc />
  public override int GetHashCode ()
  {
    var hashCode = new HashCode ();
    hashCode.Add (base.GetHashCode ());
    hashCode.Add (value: this.EmailAddress, comparer: StringComparer.OrdinalIgnoreCase);

    return hashCode.ToHashCode ();
  }

  public static bool operator == (AuthorAttribute? left, AuthorAttribute? right) => object.Equals (objA: left, objB: right);

  public static bool operator != (AuthorAttribute? left, AuthorAttribute? right) => !object.Equals (objA: left, objB: right);

  #endregion
}
