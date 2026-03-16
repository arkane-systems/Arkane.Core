# RandomProvider

**Namespace:** `ArkaneSystems.Arkane`

`RandomProvider` is a static helper that provides a correctly-seeded, per-thread singleton instance of `System.Random`.

## The Problem It Solves

Creating multiple `System.Random` instances in quick succession — for example in a tight loop, or across threads at startup — can result in identical seeds, because the default `Random()` constructor seeds from the system clock. Identically-seeded instances produce identical sequences of numbers, which is almost never the desired behaviour.

Additionally, `System.Random` is **not thread-safe**. Using a single shared instance from multiple threads without synchronisation can produce incorrect results or throw exceptions.

`RandomProvider` addresses both issues:

- **Unique seeds per thread:** Each new `ThreadLocal` instance is seeded via `Interlocked.Increment`, guaranteeing that no two threads share the same seed.
- **No locking required:** Each thread owns its own `Random` instance, eliminating the need for synchronisation.

## Usage

```csharp
using ArkaneSystems.Arkane;

Random rng = RandomProvider.GetInstance();
int value = rng.Next(1, 100);
```

Call `RandomProvider.GetInstance()` wherever you need a `Random`. Within any given thread, every call returns the same instance.

## Thread Safety

`GetInstance()` is thread-safe. You may call it concurrently from multiple threads — each thread receives its own dedicated `Random` instance.

> ⚠️ Do not pass the returned `Random` instance to another thread. Always call `GetInstance()` on the thread that will use the instance.

## Notes

- The initial seed is derived from `Environment.TickCount` at class initialisation time and atomically incremented for each new thread, ensuring all per-thread instances are uniquely seeded.
- This class is suitable for general-purpose, non-cryptographic random number generation. For cryptographic purposes, use `System.Security.Cryptography.RandomNumberGenerator` instead.
