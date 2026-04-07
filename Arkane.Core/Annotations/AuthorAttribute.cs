#region header

// Arkane.Core - AuthorAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-04 6:49 PM

#endregion

#region using

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using Metalama.Patterns.Contracts;

#endregion

namespace ArkaneSystems.Arkane.Annotations;

/// <summary>
///   An attribute containing authorship information for an assembly or smaller code artifact.
/// </summary>
/// <param name="name">The name of the author.</param>
/// <param name="emailAddress">The email address of the author.</param>
/// <remarks>
///   <para>
///     The e-mail address is considered the unique part of the author's identity; names both change and are more
///     variable in representation. Equality comparisons are done on this basis, and with the assumption that e-mail
///     addresses are non-case-sensitive.
///   </para>
///   <para>
///     The name of the author is not considered unique, and is not used for equality comparisons.
///   </para>
///   <para>
///     It is suggested that this be applied to the assembly for the overall author, and then
///     deviations from this be marked out by individual usages.
///   </para>
/// </remarks>
[PublicAPI]
[AttributeUsage (AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class AuthorAttribute ([Required] string name, [Required] [Email] string emailAddress)
  : Attribute, IEquatable<AuthorAttribute>
{
  /// <summary>
  ///   The name of the author.
  /// </summary>
  public string Name { get; } = name;

  /// <summary>
  ///   The e-mail address of the author.
  /// </summary>
  public string EmailAddress { get; } = emailAddress;

  #region Overrides of Object

  /// <inheritdoc />
  public override string ToString () => $"Author: {this.Name} <{this.EmailAddress}>";

  #endregion

  #region Equality members

  /// <inheritdoc />
  public override bool Equals ([NotNullWhen (true)] object? obj)
    => ReferenceEquals (objA: this, objB: obj) || (obj is AuthorAttribute other && this.Equals (other));

  /// <inheritdoc />
  public bool Equals (AuthorAttribute? other)
  {
    if (other is null)
      return false;
    if (ReferenceEquals (objA: this, objB: other))
      return true;

    return string.Equals (a: this.EmailAddress, b: other.EmailAddress, comparisonType: StringComparison.OrdinalIgnoreCase);
  }

  /// <inheritdoc />
  public override int GetHashCode () => StringComparer.OrdinalIgnoreCase.GetHashCode (this.EmailAddress);

  /// <summary>Determines whether two <see cref="AuthorAttribute" /> instances are equal.</summary>
  /// <param name="left">The left-hand operand.</param>
  /// <param name="right">The right-hand operand.</param>
  /// <returns>
  ///   <see langword="true" /> if <paramref name="left" /> equals <paramref name="right" />; otherwise
  ///   <see langword="false" />.
  /// </returns>
  public static bool operator == (AuthorAttribute? left, AuthorAttribute? right) => Equals (objA: left, objB: right);

  /// <summary>Determines whether two <see cref="AuthorAttribute" /> instances are not equal.</summary>
  /// <param name="left">The left-hand operand.</param>
  /// <param name="right">The right-hand operand.</param>
  /// <returns>
  ///   <see langword="true" /> if <paramref name="left" /> does not equal <paramref name="right" />; otherwise
  ///   <see langword="false" />.
  /// </returns>
  public static bool operator != (AuthorAttribute? left, AuthorAttribute? right) => !Equals (objA: left, objB: right);

  #endregion
}
