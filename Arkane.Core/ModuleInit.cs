#region header

// Arkane.Core - ModuleInit.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 1:48 PM

#endregion

#region using

using System.Runtime.CompilerServices;

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Module initializer. The marked static method will be called by the runtime before any code in this assembly is
///   executed, allowing us to perform any necessary initialization of static data, etc. before the rest of the code in
///   this assembly runs.
/// </summary>
file static class ModuleInit
{
  #pragma warning disable CA2255

  [UsedImplicitly]
  [ModuleInitializer]
  public static void InitializeModule ()
  {
    AppContext.SetSwitch (switchName: "Switch.ArkaneSystems.Arkane.Core.Presence", isEnabled: true);
  }

  #pragma warning restore CA2255
}
