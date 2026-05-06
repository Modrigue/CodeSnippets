# Code Samples

A personal collection of small, self-contained, and tested code snippets in various languages, intended as ready-to-use building blocks and references.

## Repository layout

The repository is organized by language. Each top-level folder contains either standalone source files or runnable example projects.

### [CSharp/](CSharp/)

Single-file .NET Framework C# utility classes covering common needs:

- [Base64Tools.cs](CSharp/Base64Tools.cs) — Base64 encoding/decoding helpers.
- [ByteTools.cs](CSharp/ByteTools.cs) — byte-array manipulation utilities.
- [ControlRenderHelper.cs](CSharp/ControlRenderHelper.cs) — helpers for rendering WinForms/WPF controls.
- [ImageTools.cs](CSharp/ImageTools.cs) — image loading, conversion, and processing utilities.
- [JSONTools.cs](CSharp/JSONTools.cs) — JSON serialization/deserialization helpers.
- [Logger.cs](CSharp/Logger.cs) — lightweight logging utility.
- [MailWriter.cs](CSharp/MailWriter.cs) — email composition/sending helper.
- [RandomGenerationTools.cs](CSharp/RandomGenerationTools.cs) — randomness and ID generation utilities.
- [UnixTime.cs](CSharp/UnixTime.cs) — conversions between Unix timestamps and `DateTime`.
- [WebServiceRequestHelper.cs](CSharp/WebServiceRequestHelper.cs) — helpers for issuing HTTP requests to web services.

### [GLSL/](GLSL/)

Fragment shaders for image distortion effects:

- [fisheye.shader](GLSL/fisheye.shader) — applies a fisheye lens distortion.
- [antifisheye.shader](GLSL/antifisheye.shader) — reverses a fisheye distortion.

### [TypeScript/](TypeScript/)

Runnable TypeScript example projects. Each project ships with its own `package.json` and `tsconfig.json`; install dependencies with `npm install` and build with `npm run build` (or the script defined in the project).

- [ExpressTSExample/](TypeScript/ExpressTSExample/) — minimal Express HTTP server written in TypeScript.
- [NodeTSExample/](TypeScript/NodeTSExample/) — minimal Node.js + TypeScript starter project.
- [SocketIORoomsExample/](TypeScript/SocketIORoomsExample/) — Socket.IO example demonstrating room-based messaging (see its own [README](TypeScript/SocketIORoomsExample/README.md) for details).

## Usage

The C# files and GLSL shaders are designed to be dropped directly into existing projects. The TypeScript examples are standalone and can be cloned, installed, and run independently.

## License

Provided as-is, for reference and reuse.
