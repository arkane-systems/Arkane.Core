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

[PublicAPI]
[AttributeUsage (AttributeTargets.Assembly)]
public class SourceLanguageAttribute (ProgrammingLanguages language = ProgrammingLanguages.CSharp)
  : Attribute
{
  public ProgrammingLanguages Language { get; } = language;

  public bool RewrittenByAspects { get; set; }

  public bool Obfuscated { get; set; }

  public string PostCompilationModifications { get; set; } = string.Empty;
}
