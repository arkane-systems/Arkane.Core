#region header

// Arkane.Core - ProgrammingLanguages.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 6:54 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane.Annotations;

/// <summary>
///   Identifies the programming language in which source code is written.
/// </summary>
[PublicAPI]
public enum ProgrammingLanguages
{
  /// <summary>C# (C-Sharp).</summary>
  CSharp,

  /// <summary>Visual Basic .NET.</summary>
  VisualBasic,

  /// <summary>F# (F-Sharp).</summary>
  FSharp,

  /// <summary>Common Intermediate Language (CIL/MSIL).</summary>
  Il,

  /// <summary>Python.</summary>
  Python,

  /// <summary>PowerShell.</summary>
  PowerShell,

  /// <summary>JavaScript.</summary>
  JavaScript,
}
