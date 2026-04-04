#region header

// Arkane.Core - Module.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-02 1:48 PM

#endregion

#region using

using System.Runtime.CompilerServices;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   Module initializer. The marked static method will be called by the runtime before any code in this assembly is
///   executed, allowing us to perform any necessary initialization of static data, etc. before the rest of the code in
///   this assembly runs.
/// </summary>
internal static class Module
{
  // This should never be null in practice, but we need to suppress the warning about it being non-nullable without
  // an initializer, since it is.
  internal static IServiceProvider ServiceProvider { get; set; } = null!;

  #pragma warning disable CA2255

  [UsedImplicitly]
  [ModuleInitializer]
  public static void InitializeModule ()
  {
    // Set AppContext switch to flag the presence of Arkane.Core.
    AppContext.SetSwitch (switchName: "Switch.ArkaneSystems.Arkane.Core.Presence", isEnabled: true);

    // Build services provided by this library. This is not currently intended to be used by external code, but
    // it allows us to use DI within this library, and also to provide services to external code if we choose to
    // do so in the future.
    var services = new ServiceCollection ();

    ServiceProvider = services.BuildServiceProvider (validateScopes: true);
  }

  #pragma warning restore CA2255
}
