#region header

// Arkane.Core - CannotHappenException.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2026.  All rights reserved.
// 
// Created: 2026-04-06 11:24 PM

#endregion

#region using

using ArkaneSystems.Arkane.Properties;

using JetBrains.Annotations;

#endregion

namespace ArkaneSystems.Arkane;

/// <summary>
///   The exception that is thrown to indicate that a code path considered unreachable has been executed.
/// </summary>
/// <remarks>
///   <para>
///     This exception is intended for use in situations where program logic dictates that a particular
///     condition or code path should never occur; e.g., the default branch of switch statements which provide a case for
///     every member of an enum, and the like. Throwing this exception signals a serious internal error and typically
///     indicates a bug in the calling code.
///   </para>
///   <para>
///     This is treated as a special case of <see cref="T:System.InvalidOperationException" />.
///   </para>
/// </remarks>
[PublicAPI]
[Serializable]
public sealed class CannotHappenException : InvalidOperationException
{
  /// <summary>
  ///   Initializes a new instance of the CannotHappenException class with a default message indicating that an impossible
  ///   code path has been reached.
  /// </summary>
  /// <remarks>
  ///   This exception is intended to be thrown in situations where program logic has reached a state
  ///   that should be impossible under correct operation. It is typically used as a guard against unexpected or invalid
  ///   control flow, and may indicate a bug in the code.
  /// </remarks>
  [PublicAPI]
  public CannotHappenException ()
    : base (Resources.CannotHappenException_CannotHappenException_Impossible +
            Resources.CannotHappenException_CannotHappenException_DefaultMessage)
  { }

  /// <summary>
  ///   Initializes a new instance of the CannotHappenException class with a specified error message describing the
  ///   impossible condition encountered.
  /// </summary>
  /// <remarks>
  ///   This exception is intended to be thrown in code paths that are logically unreachable or represent
  ///   states that should never occur. It is typically used to indicate a violation of invariants or assumptions that are
  ///   expected to always hold.
  /// </remarks>
  /// <param name="message">
  ///   The message that describes the error condition. This message is appended to a standard prefix indicating an
  ///   impossible situation.
  /// </param>
  [PublicAPI]
  public CannotHappenException (string message)
    : base (Resources.CannotHappenException_CannotHappenException_Impossible + message)
  { }

  /// <summary>
  ///   Initializes a new instance of the CannotHappenException class with a specified error message and a reference to
  ///   the inner exception that is the cause of this exception.
  /// </summary>
  /// <remarks>
  ///   This exception is intended to be thrown in code paths that are believed to be unreachable or
  ///   represent logically impossible states. The error message is prefixed with a standard phrase to indicate this
  ///   context.
  /// </remarks>
  /// <param name="message">
  ///   The message that describes the error. This message is appended to a standard prefix indicating an impossible
  ///   condition.
  /// </param>
  /// <param name="innerException">
  ///   The exception that is the cause of the current exception, or a null reference if no inner
  ///   exception is specified.
  /// </param>
  [PublicAPI]
  public CannotHappenException (string message, Exception innerException)
    : base (message: Resources.CannotHappenException_CannotHappenException_Impossible + message, innerException: innerException)
  { }
}
