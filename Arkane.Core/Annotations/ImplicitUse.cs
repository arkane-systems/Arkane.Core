using System.Diagnostics;

namespace ArkaneSystems.Arkane.Annotations;

/// <summary>
/// Specifies the details of an implicitly used symbol when it is marked
/// with <see cref="MeansImplicitUseAttribute"/> or <see cref="UsedImplicitlyAttribute"/>.
/// </summary>
[Flags]
public enum ImplicitUseKindFlags
{
  Default = Access | Assign | InstantiatedWithFixedConstructorSignature,

  /// <summary>Only entity marked with attribute considered used.</summary>
  Access = 1,

  /// <summary>Indicates implicit assignment to a member.</summary>
  Assign = 2,

  /// <summary>
  /// Indicates implicit instantiation of a type with a fixed constructor signature.
  /// That means any unused constructor parameters will not be reported as such.
  /// </summary>
  InstantiatedWithFixedConstructorSignature = 4,

  /// <summary>Indicates implicit instantiation of a type.</summary>
  InstantiatedNoFixedConstructorSignature = 8,
}

/// <summary>
/// Specifies what is considered to be used implicitly when marked
/// with <see cref="MeansImplicitUseAttribute"/> or <see cref="UsedImplicitlyAttribute"/>.
/// </summary>
[Flags]
[System.Diagnostics.CodeAnalysis.SuppressMessage ("Design", "CA1069:Enums values should not be duplicated", Justification = "Acceptable for default value.")]
public enum ImplicitUseTargetFlags
{
  Default = Itself,

  /// <summary>Code entity itself.</summary>
  Itself = 1,

  /// <summary>Members of the type marked with the attribute are considered used.</summary>
  Members = 2,

  /// <summary> Inherited entities are considered used. </summary>
  WithInheritors = 4,

  /// <summary>Entity marked with the attribute and all its members considered used.</summary>
  WithMembers = Itself | Members
}

/// <summary>
/// Indicates that the marked symbol is used implicitly (via reflection, in an external library, and so on),
/// so this symbol will be ignored by usage-checking inspections. <br/>
/// You can use <see cref="ImplicitUseKindFlags"/> and <see cref="ImplicitUseTargetFlags"/>
/// to configure how this attribute is applied.
/// </summary>
/// <example><code>
/// [UsedImplicitly]
/// public class TypeConverter { }
/// 
/// public class SummaryData
/// {
///   [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
///   public SummaryData() { }
/// }
/// 
/// [UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Default)]
/// public interface IService { }
/// </code></example>
[AttributeUsage (AttributeTargets.All)]
public sealed class UsedImplicitlyAttribute (ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags) : Attribute
{
  public UsedImplicitlyAttribute ()
    : this (ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }

  public UsedImplicitlyAttribute (ImplicitUseKindFlags useKindFlags)
    : this (useKindFlags, ImplicitUseTargetFlags.Default) { }

  public UsedImplicitlyAttribute (ImplicitUseTargetFlags targetFlags)
    : this (ImplicitUseKindFlags.Default, targetFlags) { }

  public ImplicitUseKindFlags UseKindFlags { get; } = useKindFlags;

  public ImplicitUseTargetFlags TargetFlags { get; } = targetFlags;

  public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Can be applied to attributes, type parameters, and parameters of a type assignable from <see cref="System.Type"/>.
/// When applied to an attribute, the decorated attribute behaves the same as <see cref="UsedImplicitlyAttribute"/>.
/// When applied to a type parameter or to a parameter of type <see cref="System.Type"/>,
/// indicates that the corresponding type is used implicitly.
/// </summary>
[AttributeUsage (
  AttributeTargets.Class
  | AttributeTargets.GenericParameter
  | AttributeTargets.Parameter)]
public sealed class MeansImplicitUseAttribute (ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags) : Attribute
{
  public MeansImplicitUseAttribute ()
    : this (ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }

  public MeansImplicitUseAttribute (ImplicitUseKindFlags useKindFlags)
    : this (useKindFlags, ImplicitUseTargetFlags.Default) { }

  public MeansImplicitUseAttribute (ImplicitUseTargetFlags targetFlags)
    : this (ImplicitUseKindFlags.Default, targetFlags) { }

  [UsedImplicitly] public ImplicitUseKindFlags UseKindFlags { get; } = useKindFlags;

  [UsedImplicitly] public ImplicitUseTargetFlags TargetFlags { get; } = targetFlags;
}
