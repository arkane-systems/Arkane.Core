#region header

// Arkane.Core - MefExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-06-25 7:31 AM

#endregion

#region using

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane.ComponentModel.Composition;

/// <summary>
///   Extension methods for Managed Extensibility Framework (MEF) hosts and parts.
/// </summary>
public static class MefExtensions
{
  #region Host extensions

  #region Nested type: $extension
  
  extension<T> ([NotNullContract] T @this) where T : IMefPartHost
  {
    [PublicAPI]
    public void ComposeStandardSourcedParts (IEnumerable<Assembly>? assemblies = null)
    {
      var catalog = new AggregateCatalog ();

      var entryAssembly = Assembly.GetEntryAssembly ();

      // Add internally supplied parts (default to entry assembly; in case this would ultimately lead back to
      // unmanaged code, back it up with the calling assembly).
      catalog.Catalogs.Add (new AssemblyCatalog (entryAssembly ?? Assembly.GetCallingAssembly ()));

      // Add parts supplied in specified assemblies, if any.
      if (assemblies != null)
        foreach (Assembly assembly in assemblies)
          catalog.Catalogs.Add (new AssemblyCatalog (assembly));

      // Add parts in application extensions subfolder.
      if (entryAssembly != null)
        try
        {
          string location = entryAssembly.Location;

          if (!string.IsNullOrEmpty (location))
          {
            string directoryName = Path.Combine (path1: Path.GetDirectoryName (location)!, path2: "Extensions");
            catalog.Catalogs.Add (new DirectoryCatalog (directoryName));
          }
        }

        // Ignore NotSupportedException, which indicates that we are a dynamically loaded assembly
        // and therefore have no location, and so cannot load any extensions from a subfolder.
        catch (NotSupportedException) { }

      new CompositionContainer (catalog: catalog, providers: Array.Empty<ExportProvider> ()).ComposeParts (@this);
    }
  }

  #endregion
  
  #endregion Host extensions
}
