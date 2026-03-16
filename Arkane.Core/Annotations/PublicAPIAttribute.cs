using System.Diagnostics.CodeAnalysis;

namespace ArkaneSystems.Arkane.Annotations;

/// <summary>
/// This attribute is intended to mark publicly available APIs
/// that should not be removed and therefore should never be reported as unused.
/// </summary>
[MeansImplicitUse (ImplicitUseTargetFlags.WithMembers)]
[AttributeUsage (AttributeTargets.All, Inherited = false)]
public sealed class PublicAPIAttribute : Attribute
{
  public PublicAPIAttribute ()
  {
    this.Comment = string.Empty;
  }

  public PublicAPIAttribute ([NotNull] string comment)
  {
    this.Comment = comment;
  }

  public string Comment { get; }
}
