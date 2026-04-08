# Metalama Aspects

Arkane.Core ships a collection of [Metalama](https://www.postsharp.net/metalama) aspect attributes that you apply directly to methods, fields, properties, or parameters to weave cross-cutting behaviour into your compiled code.

> ⚠️ These aspects are marked `[CLSCompliant(false)]` because Metalama's weaving infrastructure requires features that are not CLS-compliant. They are fully usable from any .NET 10 project; the attribute only affects formal CLS compliance analysis.

All aspect types live under the **`ArkaneSystems.Arkane.Aspects`** namespace (or a sub-namespace as noted).

---

## Debugger Break Aspects

**Namespace:** `ArkaneSystems.Arkane.Aspects.Diagnostics`

Two aspects let you inject a `Debugger.Break()` call around a method at compile time — useful for investigating hard-to-reproduce issues without permanently altering the code.

### BreakBeforeAttribute

Triggers a debugger break **before** the target method body executes.

```csharp
[BreakBefore]
public void SuspiciousMethod()
{
    // Execution pauses here in the debugger before this code runs
    DoWork();
}
```

### BreakAfterAttribute

Triggers a debugger break **after** the target method body executes (in a `finally` block, so it fires even if the method throws).

```csharp
[BreakAfter]
public string ComputeResult()
{
    return ExpensiveCalculation(); // pauses after this returns
}
```

---

## DivisibleByAttribute

**Namespace:** `ArkaneSystems.Arkane.Aspects`

A contract aspect that validates that a numeric parameter, field, or property value is evenly divisible by a specified divisor. Throws `ArgumentException` when the constraint is violated.

Eligible types: `int`, `uint`, `long`, `ulong`, `short`, `ushort`, `byte`, `sbyte`, `decimal`.

```csharp
using ArkaneSystems.Arkane.Aspects;

// Validates that 'bits' is a multiple of 8
public static byte[] GenerateKey([DivisibleBy(8)] int bits)
{
    // bits is guaranteed to be divisible by 8 here
}
```

### Constructor

| Parameter | Type | Description |
|-----------|------|-------------|
| `divisor` | `int` | The required divisor. Must be greater than zero. |

### Behaviour

- Applied as a `ContractAspect` — validation runs at the point where a value is assigned to the decorated parameter, field, or property.
- Throws `ArgumentException` with a descriptive message if the value is not divisible by the specified divisor.

---

## Diagnostics Aspects

**Namespace:** `ArkaneSystems.Arkane.Aspects.Diagnostics`

### LogMethodCallsAttribute

Wraps a method to log entry and exit (including any thrown exception) to `Console.WriteLine`.

> 📝 This aspect is intentionally minimal. It is a placeholder that will be expanded to integrate with the library's future built-in logging service.

```csharp
[LogMethodCalls]
public int Compute(int x, int y)
{
    return x + y;
    // Console output:
    // Entering Int32 Compute(Int32, Int32)
    // Exiting Int32 Compute(Int32, Int32) with result 42
}
```

### NoOpAttribute

Replaces the entire body of the target method with a no-op that returns `default` for the method's return type. Useful for temporarily disabling a method during testing or debugging without removing its call sites.

```csharp
[NoOp]
public void SendEmail(string to, string subject, string body)
{
    // This entire method body is replaced at compile time.
    // Nothing happens; the method returns immediately.
}

[NoOp]
public int GetCount()
{
    // Returns 0 (default for int) without executing.
    return expensiveQuery.Count();
}
```

### StopwatchAttribute

Times a method and writes the elapsed milliseconds to `Trace` output using `Trace.WriteLine`. The timing happens in a `finally` block so it is always recorded, even if the method throws.

```csharp
[Stopwatch]
public void LoadData()
{
    // After this returns (or throws), trace output includes:
    // "LoadData executed in 123 ms"
}
```

---

## Resilience Aspects

**Namespace:** `ArkaneSystems.Arkane.Aspects.Resilience`

### RetryAttribute

Automatically retries a method on failure, with a configurable number of attempts and a delay between them. If all retries are exhausted, the last exception propagates normally.

```csharp
// Default: 3 retries, 100 ms delay
[Retry]
public void CallExternalService()
{
    httpClient.Get("https://example.com/api");
}

// Custom: 5 retries, 500 ms delay
[Retry(maxRetries: 5, delay: 500)]
public string FetchData()
{
    return httpClient.GetString("https://example.com/data");
}
```

### Constructor / Properties

| Member | Type | Default | Description |
|--------|------|---------|-------------|
| `MaxRetries` | `int` | `3` | Maximum number of retry attempts after the first failure. |
| `Delay` | `int` | `100` | Milliseconds to wait between retries. |

### Behaviour

- Attempt 0 runs the original method body.
- On exception, if `attempt < MaxRetries`, sleeps for `Delay` ms and retries.
- After `MaxRetries` failed attempts, rethrows the last exception.
- A brief message (including the delay and exception message) is written to `Console` before each retry.
