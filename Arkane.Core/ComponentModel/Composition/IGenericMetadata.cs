#region header

// Arkane.Core - IGenericMetadata.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 12:46 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane.ComponentModel.Composition;

/// <summary>
///   A generic metadata interface for use with MEF.
/// </summary>
[PublicAPI]
public interface IGenericMetadata
{
  /// <summary>
  ///   Name of this MEF part.
  /// </summary>
  string Name { get; }
}
