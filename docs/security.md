# Security Utilities

Arkane.Core provides lightweight helpers for generating cryptographic keys and hash-related values. All types live under the **`ArkaneSystems.Arkane.Security.Cryptography`** namespace.

> ⚠️ These utilities generate raw cryptographic primitives (keys, salts). They are **not** a substitute for a complete cryptographic library. Use them as building blocks alongside established frameworks such as `System.Security.Cryptography` and reputable password-hashing libraries.

---

## CryptoHelpers

```csharp
public static class CryptoHelpers
```

Provides helper methods for generating cryptographic keys.

### GenerateKey

```csharp
public static byte[] GenerateKey(int bits)
```

Generates a cryptographically random AES key of the specified size using `RandomNumberGenerator.Fill`.

| Parameter | Constraints | Description |
|-----------|-------------|-------------|
| `bits` | ≥ 8, multiple of 8 | The key size in bits. |

Returns a `byte[]` of length `bits / 8`.

Common AES key sizes are `128`, `192`, and `256` bits.

```csharp
using ArkaneSystems.Arkane.Security.Cryptography;

byte[] key128 = CryptoHelpers.GenerateKey(128); // 16 bytes
byte[] key256 = CryptoHelpers.GenerateKey(256); // 32 bytes
```

The `bits` parameter is validated at compile-time (via the `[DivisibleBy(8)]` and `[GreaterThanOrEqual(8)]` contract aspects) as well as at runtime — passing a non-positive, non-multiple-of-8, or less-than-8 value throws `ArgumentException`.

---

## HashHelpers

```csharp
public static class HashHelpers
```

Provides helper methods for generating hash-related values.

### GenerateSalt

```csharp
public static byte[] GenerateSalt()
```

Generates a cryptographically random 512-bit (64-byte) salt value using `RandomNumberGenerator.Fill`.

```csharp
using ArkaneSystems.Arkane.Security.Cryptography;

byte[] salt = HashHelpers.GenerateSalt(); // 64 random bytes

// Use with string hashing extensions:
byte[] hash = myPassword.GenerateSha256Hash(salt);
```

The returned salt is suitable for use with the string hashing extension methods (`GenerateMd5Hash(salt)`, `GenerateSha256Hash(salt)`) found in the [extension methods](extensions.md#string-string) documentation.

> ⚠️ Do **not** use these hashing methods for password storage. Use a dedicated password-hashing algorithm such as `Argon2`, `bcrypt`, or `PBKDF2`.
