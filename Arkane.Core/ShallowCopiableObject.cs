#region header

// Arkane.Core - ShallowCopiableObject.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2018.  All rights reserved.
// 
// Created: 2026-04-02 6:17 PM

#endregion

#region using

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

[PublicAPI]
public class ShallowCopiableObject : IShallowCopy<ShallowCopiableObject>
{
  public ShallowCopiableObject ShallowCopy () => (ShallowCopiableObject)this.MemberwiseClone ();

  public object Clone () => this.ShallowCopy ();
}
