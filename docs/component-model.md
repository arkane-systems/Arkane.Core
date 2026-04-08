# Component Model

Arkane.Core provides a small set of interfaces to support .NET's Managed Extensibility Framework (MEF). All types live under the **`ArkaneSystems.Arkane.ComponentModel.Composition`** namespace.

---

## IGenericMetadata

A generic metadata contract for MEF parts.

```csharp
public interface IGenericMetadata
{
    string Name { get; }
}
```

Use `IGenericMetadata` as the metadata type when exporting a MEF part that needs to be identified by a name.

```csharp
using System.ComponentModel.Composition;
using ArkaneSystems.Arkane.ComponentModel.Composition;

[Export(typeof(IProcessor))]
[ExportMetadata("Name", "FastProcessor")]
public class FastProcessor : IProcessor { /* ... */ }

// Importing with metadata:
[ImportMany]
public IEnumerable<Lazy<IProcessor, IGenericMetadata>> Processors { get; set; }

// Selecting by name:
IProcessor processor = Processors
    .First(p => p.Metadata.Name == "FastProcessor")
    .Value;
```
