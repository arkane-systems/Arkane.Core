#region header

// Arkane.Core - DivisibleByAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-05 6:30 PM

#endregion

#region using

using JetBrains.Annotations;

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;

#endregion

namespace ArkaneSystems.Arkane.Aspects;

/// <summary>
///   A Metalama contract aspect that validates that a numeric parameter, field, or property value is evenly
///   divisible by a specified divisor.
/// </summary>
[PublicAPI]
[CLSCompliant (false)]
public class DivisibleByAttribute : ContractAspect
{
  /// <summary>
  ///   Initializes a new instance of the <see cref="DivisibleByAttribute" /> class with the specified divisor.
  /// </summary>
  /// <param name="divisor">The divisor that values must be evenly divisible by. Must not be zero.</param>
  /// <exception cref="ArgumentException">Thrown when <paramref name="divisor" /> is zero.</exception>
  public DivisibleByAttribute (int divisor)
  {
    if (divisor <= 0)
      throw new ArgumentException (message: @"Divisor must be greater than zero.",
                                   paramName: nameof (divisor));

    this.Divisor = divisor;
  }

  /// <summary>
  ///   Gets the divisor that values must be evenly divisible by.
  /// </summary>
  protected int Divisor { get; }

  #region Overrides of ContractAspect

  /// <inheritdoc />
  /// <summary>
  ///   Configures eligibility rules for fields, properties, and indexers.
  ///   Only numeric types are eligible for this aspect.
  /// </summary>
  public override void BuildEligibility (IEligibilityBuilder<IFieldOrPropertyOrIndexer> builder)
  {
    Type[] supportedTypes =
    [
      typeof (int), typeof (uint), typeof (long), typeof (ulong), typeof (short), typeof (ushort), typeof (byte), typeof (sbyte),
      typeof (decimal),
    ];

    base.BuildEligibility (builder);

    builder.Type ()
           .MustSatisfyAny (supportedTypes.Select (supportedType =>
                                                     new Action<IEligibilityBuilder<IType>> (t => t.MustEqual (supportedType)))
                                          .ToArray ());
  }

  /// <inheritdoc />
  /// <summary>
  ///   Configures eligibility rules for parameters.
  ///   Only numeric types are eligible for this aspect.
  /// </summary>
  public override void BuildEligibility (IEligibilityBuilder<IParameter> builder)
  {
    Type[] supportedTypes =
    [
      typeof (int), typeof (uint), typeof (long), typeof (ulong), typeof (short), typeof (ushort), typeof (byte), typeof (sbyte),
      typeof (decimal),
    ];

    base.BuildEligibility (builder);

    builder.Type ()
           .MustSatisfyAny (supportedTypes.Select (supportedType =>
                                                     new Action<IEligibilityBuilder<IType>> (t => t.MustEqual (supportedType)))
                                          .ToArray ());
  }

  /// <inheritdoc />
  /// <summary>
  ///   Validates that the provided value is divisible by the configured <see cref="Divisor" />.
  /// </summary>
  /// <param name="value">The value to validate.</param>
  /// <exception cref="ArgumentException">Thrown when <paramref name="value" /> is not divisible by <see cref="Divisor" />.</exception>
  public override void Validate (dynamic? value)
  {
    if (value % this.Divisor != 0)
      throw new ArgumentException (message: string.Format (AspectResources.DivisibleByAttribute_Validate_ValueMustBeDivisibleBy,
                                                           this.Divisor,
                                                           value),
                                   paramName: nameof (value));
  }

  #endregion
}
