#region header

// Arkane.Core.UnitTests - GlobalAttributes.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 9:37 AM

#endregion

#region using

using JetBrains.Annotations;

#endregion

[assembly: Parallelize (Scope = ExecutionScope.MethodLevel, Workers = 16)]
