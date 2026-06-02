# SharpCastXml

A C# wrapper and object model for [CastXML](https://github.com/CastXML/CastXML). It runs CastXML over C/C++ sources (or parses CastXML's GCC-XML output directly) and exposes the result as a strongly-typed C++ model you can walk, query, and render into generated code.

Based on the parser from [SharpGenTools](https://github.com/SharpGenTools/SharpGenTools).

- **Target framework:** `netstandard2.0`
- **Package:** [`SharpCastXml`](https://www.nuget.org/packages/SharpCastXml) on NuGet
- **Repository:** https://github.com/KevinGliewe/SharpCastXml

## Features

- Run CastXML on C/C++ translation units, or parse a pre-generated CastXML (`--castxml-gccxml`) XML file.
- A complete C++ object model: namespaces, structs/unions, fields, enums, functions, methods, parameters, fundamental types, typedefs, constants, and more (see `SharpCastXml.CppModel`).
- A filter (`ContextConfig.Process`) to restrict the model to types declared in your own headers, ignoring system/STL noise.
- Reads `__attribute__((annotate("{ ... }")))` annotations (parsed as [Hjson](https://hjson.github.io/)) and binds them to typed annotation classes.
- An annotation-driven **rendering** framework for generating source code from the model, using [Autofac](https://autofac.org/) for view resolution.

## Installation

```xml
<PackageReference Include="SharpCastXml" Version="1.7.1" />
```

You also need the [CastXML](https://github.com/CastXML/CastXML) executable available (on `PATH`, or pass an explicit path — see below).

## Usage

### Parse an existing CastXML XML file

If you already have CastXML output (`castxml --castxml-gccxml -o out.xml input.cpp`):

```csharp
using SharpCastXml.Config;
using SharpCastXml.Logging.Impl;
using SharpCastXml.Logging;
using SharpCastXml.Parser;

var consoleLogger = new ConsoleLogger();
var logger = new Logger(consoleLogger, consoleLogger);

var parser = new CppParser(logger);
parser.Initialize(new ContextConfig
{
    // Only emit types declared in these source files (paths must match the
    // <File name="..."> entries CastXML recorded in the XML).
    Process = { "C:/path/to/my_header.hpp" },
});

CppModule module = parser.ParseXml("out.xml");

foreach (var inc in module.Includes)
    foreach (var s in inc.Structs)
        Console.WriteLine($"{s.Name} (size={s.Size})");
```

### Run CastXML directly on sources

```csharp
using SharpCastXml.Config;
using SharpCastXml.Parser;

var castXml = new CastXml(logger, new IncludeDirectoryResolver(logger), executablePath: null);
var parser  = new CppParser(logger, castXml);

parser.Initialize(new ContextConfig
{
    Id      = Path.GetFullPath("test.c"),
    Process = { Path.GetFullPath("test.c") },
    Include = { Path.GetFullPath("test.h") },
    Macros  = { ["ARRSIZE"] = "42 * 3" },
});

CppModule module = parser.Run(new CppModule());
```

Pass an explicit `executablePath` to `CastXml(...)` if `castxml` is not on `PATH`.

## Annotations & code generation

Annotate C++ declarations so the generator can recognize and shape them. The JSON-ish payload is parsed with Hjson (single quotes allowed):

```cpp
struct __attribute__((annotate("{ hello: 'world' }"))) MyStruct {
    int x;
};
```

Define a matching annotation class and a view, then register and render:

```csharp
public class TestAnnotation
{
    public string hello { get; set; }
}

[ViewName("TestView")]
public class TestView : CppElementView<CppStruct, TestAnnotation>
{
    public override void Render()
    {
        WL(Annotation.hello);   // WL = write line into the IndentedTextWriter
    }
}

// Wire up Autofac + the rendering context
var builder = new Autofac.ContainerBuilder();
builder.RegisterType<TestView>();
var container = builder.Build();

var ctx = new RenderingContext(container);
ctx.RegisterView<TestView, CppStruct, TestAnnotation>();

var sb = new StringBuilder();
using var writer = new IndentedTextWriter(new StringWriter(sb));
ctx.Render(module.Includes.First().Structs.First(), writer);

Console.WriteLine(sb.ToString());
```

A view only renders elements that carry a matching annotation, so you can drive generation entirely from the annotations in your C++ headers.

## The C++ model

`SharpCastXml.CppModel` mirrors the C++ declarations CastXML emits. Key types:

| Type | Represents |
|------|------------|
| `CppModule` | The whole translation unit; entry point (`.Includes`, `.Items`) |
| `CppInclude` | A source/header file and its top-level declarations |
| `CppNamespace` | A namespace scope |
| `CppStruct` | A struct/union/class (`.Fields`, `.Bases`, `.Size`, `.Attributes`) |
| `CppField` | A data member (`.Offset`, `.Datatype`) |
| `CppEnum` / `CppEnumItem` | Enumerations and their constants |
| `CppFunction` / `CppMethod` / `CppParameter` | Functions, methods, parameters |
| `CppFundamentalType` | Built-in types (`int`, `long long`, `double`, …) |

Most elements expose `.Items` for recursive traversal and `.Attributes` for the raw `annotate(...)` payloads.

## Requirements

- .NET / .NET Standard 2.0-compatible runtime (the package targets `netstandard2.0`; consumers typically run on .NET 8+).
- The [CastXML](https://github.com/CastXML/CastXML) executable (only needed when running CastXML directly via `CppParser.Run` / the `CastXml` class).

> **Note:** When pairing CastXML with a brand-new MSVC toolchain/STL, prefer a recent CastXML build — older releases can mis-emit or omit parts of the newer standard library's type graph.

## Building

```sh
dotnet build src/SharpCastXml/SharpCastXml.csproj -c Release
dotnet pack  src/SharpCastXml/SharpCastXml.csproj -c Release -p:Version=<x.y.z> -o ./localfeed
```

`Version.props` holds the default `VersionPrefix`; the published version is supplied at pack time via `-p:Version=`.

## Dependencies

Autofac · Figgle · Hjson · Newtonsoft.Json · Microsoft.Win32.Registry

## Acknowledgements

The C++ parser and object model are derived from [SharpGenTools](https://github.com/SharpGenTools/SharpGenTools).
