# Extension Methods

Arkane.Core provides extension methods for a wide range of .NET types. All extensions are defined on the static partial class `ArkaneSystems.Arkane.ExtensionMethods` and are available under the **`ArkaneSystems.Arkane`** namespace.

---

## String (`string`)

### Manipulation

| Method | Description |
|--------|-------------|
| `Disemvowel()` | Returns a copy of the string with all vowels (a, e, i, o, u — upper and lower case) removed. |
| `RemoveNonAlphanumeric()` | Returns a copy of the string containing only `[A-Za-z0-9]` characters. |
| `UpTo(int maxLength, string suffix = "...")` | Truncates the string to `maxLength` characters, appending `suffix` when truncated. |
| `Remove(params char[] toRemove)` | Removes all occurrences of the specified characters. |
| `Remove(params string[] toRemove)` | Removes all occurrences of the specified substrings. |

### Conversions

| Method | Description |
|--------|-------------|
| `AsDateTime(DateTime defaultValue = default)` | Attempts to parse the string as a `DateTime` using several common formats. Returns `defaultValue` when parsing fails. The result is in UTC. |
| `AsEnum<T>(bool ignoreCase = true, T defaultValue = default)` | Parses the string as an enum value of type `T`. Returns `defaultValue` on failure. |

### Hashing

> ⚠️ These methods are for general-purpose use (e.g., checksums, caching keys). Do **not** use them for password hashing — use a dedicated algorithm such as `Argon2` or `PBKDF2` instead.

| Method | Description |
|--------|-------------|
| `GenerateMd5Hash()` | Computes the 16-byte MD5 hash of the string's UTF-8 bytes. |
| `GenerateMd5Hash(byte[] salt)` | Computes the 16-byte MD5 hash of the string's UTF-8 bytes prefixed with `salt`. |
| `GenerateSha256Hash()` | Computes the 32-byte SHA-256 hash of the string's UTF-8 bytes. |
| `GenerateSha256Hash(byte[] salt)` | Computes the 32-byte SHA-256 hash of `salt + UTF8(string)`. |

### Encryption

| Method | Description |
|--------|-------------|
| `EncryptWithAes(byte[] key)` | AES-encrypts the string (UTF-8) using the provided key. Returns a byte array with the randomly generated IV prepended. Pair with `byte[].DecryptWithAes`. |

### FillWith (format helpers)

`FillWith` is a fluent alternative to `string.Format`. The string instance is used as the format template.

```csharp
"Hello, {0}!".FillWith("world")          // "Hello, world!"
"{0} + {1} = {2}".FillWith(1, 2, 3)      // "1 + 2 = 3"
"{0:C}".FillWith(CultureInfo.GetCultureInfo("en-US"), 42.0m)
```

| Overload | Description |
|----------|-------------|
| `FillWith(object? arg0)` | Substitutes one argument. |
| `FillWith(object? arg0, object? arg1)` | Substitutes two arguments. |
| `FillWith(object? arg0, object? arg1, object? arg2)` | Substitutes three arguments. |
| `FillWith(params object?[] args)` | Substitutes any number of arguments. |
| `FillWith(IFormatProvider provider, params object?[] args)` | Substitutes arguments using the given culture/format provider. |

---

## Nullable String (`string?`)

| Method | Description |
|--------|-------------|
| `Safe()` | Returns the string, or `string.Empty` if `null`. |
| `AsEmptyIfNull()` | Alias for `Safe()`. |
| `HasValue()` | Returns `true` if the string is not `null` and not empty. |
| `HasNonWhiteSpaceValue()` | Returns `true` if the string is not `null`, not empty, and not all whitespace. |
| `AsNullIfEmpty()` | Returns `null` if the string is `null` or empty; otherwise the original string. |
| `AsNullIfWhitespace()` | Returns `null` if the string is `null`, empty, or all whitespace; otherwise the original string. |

---

## Boolean (`bool`)

| Method | Description |
|--------|-------------|
| `AsBit()` | Returns `1` for `true`, `0` for `false`. |
| `AsYesNoString()` | Returns the non-localized string `"Yes"` or `"No"`. |
| `AsLocalizedYesNoString()` | Returns the culture-specific equivalent of "Yes" or "No" based on the current thread culture. |

## Nullable Boolean (`bool?`)

| Method | Description |
|--------|-------------|
| `Safe()` | Returns the boolean value, or `false` if `null`. |
| `AsYesNoString(bool maybe = false)` | Returns `"Yes"`, `"No"`, or (when `maybe` is `true`) `"Maybe"` for `null`. Not localized. |
| `AsLocalizedYesNoString(bool maybe = false)` | Localized version of `AsYesNoString`. |

---

## DateTime (`DateTime`)

| Method | Description |
|--------|-------------|
| `ToLinuxTimeStamp()` | Converts the `DateTime` to a Unix timestamp expressed as milliseconds since the Linux epoch (1 Jan 1970 UTC). |
| `DateTime.FromLinuxTimeStamp(double timestamp)` | *(Static)* Converts a Unix timestamp (milliseconds) back to a `DateTime`. |

```csharp
DateTime now    = DateTime.UtcNow;
double   ts     = now.ToLinuxTimeStamp();
DateTime back   = DateTime.FromLinuxTimeStamp(ts); // ≈ now
```

---

## Math (`Math`) — static extensions

| Member | Description |
|--------|-------------|
| `Math.Tau` | The constant τ = 2π ≈ 6.283… (one full turn in radians). |
| `Math.ToDegrees(double radians)` | Converts radians to degrees. |
| `Math.ToRadians(double degrees)` | Converts degrees to radians. |
| `Math.π` | Alias for `Math.PI`. |
| `Math.τ` | Alias for `Math.Tau`. |

```csharp
double halfTurn = Math.Tau / 2;        // π
double angle    = Math.ToDegrees(Math.PI); // 180.0
```

---

## Action (`Action`)

| Method | Description |
|--------|-------------|
| `AsMemento()` | Wraps the action in a `Memento` (`IDisposable`) that executes the action when disposed. |
| `WithFailFast()` | Returns a wrapper delegate that calls the action; if the action throws an unhandled exception, it calls `Environment.FailFast` (or breaks into the debugger if one is attached). |

```csharp
// Restore a value when leaving a using block
string originalTitle = window.Title;
using (((Action)(() => window.Title = originalTitle)).AsMemento())
{
    window.Title = "Processing…";
    DoWork();
} // window.Title restored here
```

---

## Array (`Array`)

| Method | Description |
|--------|-------------|
| `GetElementType()` | Returns the element type of the array, drilling through any nested or multidimensional layers (e.g., `int[,][,,]` → `int`). |

## Nullable Array (`Array?`)

| Method | Description |
|--------|-------------|
| `IsNullOrEmpty()` | Returns `true` if the array is `null` or has a length of zero. |

---

## Byte Array (`byte[]`)

| Method | Description |
|--------|-------------|
| `AsMemoryStream(bool writable = true)` | Creates a `MemoryStream` backed by this byte array. |
| `DecryptWithAes(byte[] key)` | Decrypts an AES-encrypted payload (IV prepended) using the provided key. Returns the original UTF-8 string. Pair with `string.EncryptWithAes`. |

---

## Guid (`Guid`)

| Method | Description |
|--------|-------------|
| `IsEmpty()` | Returns `true` if the GUID equals `Guid.Empty`. |

---

## Binary Integer (`IBinaryInteger<T>`)

These extensions apply to any type implementing `System.Numerics.IBinaryInteger<T>` (e.g., `int`, `long`, `uint`, `byte`, etc.).

| Method | Description |
|--------|-------------|
| `Exp(T power)` | Raises the integer to the given non-negative integer power using fast binary exponentiation. |
| `IsEven()` | Returns `true` if the value is even. |
| `IsOdd()` | Returns `true` if the value is odd. |

```csharp
int result = 2.Exp(10);   // 1024
bool even  = 4.IsEven();  // true
bool odd   = 7.IsOdd();   // true
```

---

## Number (`INumber<T>`)

These extensions apply to any type implementing `System.Numerics.INumber<T>`.

| Method | Description |
|--------|-------------|
| `Clamp(T min, T max)` | Returns the value clamped to the inclusive range `[min, max]`. Throws `ArgumentException` if `min >= max`. |

```csharp
int clamped = 150.Clamp(0, 100); // 100
```

---

## JSON (`JsonElement`)

| Method | Description |
|--------|-------------|
| `GetStringProperty(string propertyName, string? defaultValue = null)` | Retrieves the string value of a named property from the JSON object, returning `defaultValue` if absent or not a string. |
| `AsDateTime(string propertyName, DateTime defaultValue = default)` | Retrieves a named property and parses its value as a `DateTime`. Returns `defaultValue` on failure. |
| `IsNull()` | Returns `true` if the element's `ValueKind` is `Null` or `Undefined`. |

```csharp
using System.Text.Json;

JsonElement root = JsonDocument.Parse(json).RootElement;

string? name  = root.GetStringProperty("name");
DateTime date = root.AsDateTime("createdAt");
bool isNull   = root.GetProperty("optionalField").IsNull();
```

---

## Type (`Type`)

| Method | Description |
|--------|-------------|
| `GetDefault()` | Returns the default value for the type (`null` for reference types and `void`; `default(T)` for value types). |
| `IsDerivedFromGenericType(Type genericType)` | Returns `true` if the type is, or inherits from, a closed construction of the given open generic type. |
| `IsObjectSetToDefault(object? objectValue)` | Returns `true` if `objectValue` equals the default value for this type. |
| `GetAttribute<T>(bool includeInherited = true)` | Returns the first custom attribute of type `T` applied to this type, or `null`. |
| `GetAllAttributes<T>(bool includeInherited = true)` | Returns all custom attributes of type `T` applied to this type. |

```csharp
Type t = typeof(int);
object? def = t.GetDefault();    // 0

bool derived = typeof(List<int>).IsDerivedFromGenericType(typeof(List<>)); // true

Type stringType = typeof(string);
var attr = stringType.GetAttribute<SerializableAttribute>(); // null (string isn't [Serializable])
```
