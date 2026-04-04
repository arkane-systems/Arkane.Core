#region header

// Arkane.Core - SourceLanguageAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 7:09 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane.Annotations;

/// <summary>
///   An assembly-level attribute that records the primary source language and post-compilation
///   modification state of an assembly.
/// </summary>
/// <param name="language">The primary programming language of the assembly source code.</param>
[PublicAPI]
[AttributeUsage (AttributeTargets.Assembly)]
public class SourceLanguageAttribute (ProgrammingLanguages language = ProgrammingLanguages.CSharp)
  : Attribute
{
  /// <summary>
  ///   The primary programming language in which the assembly was written.
  /// </summary>
  public ProgrammingLanguages Language { get; } = language;

  /// <summary>
  ///   Gets or sets a value indicating whether the assembly's IL has been rewritten by a
  ///   post-compilation aspect weaver (e.g., Metalama or PostSharp).
  /// </summary>
  public bool RewrittenByAspects { get; set; }

  /// <summary>
  ///   Gets or sets a value indicating whether the assembly has been obfuscated after compilation.
  /// </summary>
  public bool Obfuscated { get; set; }

  /// <summary>
  ///   Gets or sets a free-text description of any other post-compilation modifications applied
  ///   to the assembly (e.g., ILMerge, NativeAOT trimming).
  /// </summary>
  public string PostCompilationModifications { get; set; } = string.Empty;
}
