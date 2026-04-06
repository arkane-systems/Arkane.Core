#region header

// Arkane.Core - ExtensionMethods-System-Action.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-06 9:27 AM

#endregion

#region using

using System.Diagnostics;

using ArkaneSystems.Arkane.Properties;

using JetBrains.Annotations;

#endregion

#pragma warning disable IDE0130
namespace ArkaneSystems.Arkane;

/// <summary>
///   Extension methods host class.
/// </summary>
public static partial class ExtensionMethods
{
  #region Nested type: $extension

  /// <summary>
  ///   Extension methods for System.Action.
  /// </summary>
  extension (Action @this)
  {
    /// <summary>
    ///   Converts the Action into a <see cref="T:ArkaneSystems.Arkane.Memento" /> which will cause it to be executed
    ///   when the Memento is disposed.
    /// </summary>
    /// <returns>The produced Memento.</returns>
    [PublicAPI]
    public IDisposable AsMemento () => new Memento (@this);

    /// <summary>
    ///   Provides a delegate that runs the specified action and fails fast if the action throws an exception.
    /// </summary>
    /// <returns>The wrapper delegate.</returns>
    [PublicAPI]
    public Action WithFailFast ()
      => () =>
         {
           try { @this (); }
           catch (Exception ex)
           {
             if (Debugger.IsAttached)
               Debugger.Break ();
             else
               Environment.FailFast (message: Resources
                                      .Extension_Action_WithFailFast_UnhandledExceptionOccurredDuringExecutionOfAction,
                                     exception: ex);
           }
         };
  }

  #endregion
}
