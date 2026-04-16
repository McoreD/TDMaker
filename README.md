# TDMaker

TDMaker is a cross-platform release workbench for the original app's core job:

- inspect media and disc structures
- generate screenshots
- render tracker publish text from templates
- upload screenshots
- create `.torrent` files
- export legacy XML payloads

The modern repo is structured around one shared engine with two front ends:

- `src/TDMaker.Core`: shared domain models, workflow contracts, and release orchestration
- `src/TDMaker.Infrastructure`: platform-aware implementations for tools, HTTP, filesystem, screenshots, publishing, and torrents
- `src/TDMaker.Cli`: scriptable CLI built on the shared workflow
- `src/TDMaker.App`: Avalonia desktop UI for Windows, macOS, and Linux
- `tests/TDMaker.Core.Tests`: shared behavior tests
- `docs`: architecture and tooling notes
- `legacy`: archived WinForms/WPF/Mono implementation kept as migration reference

## Build

```powershell
dotnet build TDMaker.slnx
dotnet test TDMaker.slnx
```

## Run

```powershell
dotnet run --project src/TDMaker.Cli -- tools
dotnet run --project src/TDMaker.Cli -- inspect --help
dotnet run --project src/TDMaker.App
```

## External Tools

The modern app keeps the binary surface deliberately small:

- `ffmpeg`: required for screenshots
- `mediainfo`: required for media inspection

Legacy `mplayer` support is intentionally retired. The old repo supported both FFmpeg and MPlayer for screenshots, but the modern app standardizes on FFmpeg because it is current, cross-platform, and actively maintained.

See [docs/architecture.md](docs/architecture.md) and [docs/tooling.md](docs/tooling.md) for the runtime model and current upstream tool decisions.
