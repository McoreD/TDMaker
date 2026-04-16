# TDMaker

TDMaker is a modern cross-platform release workbench for media packaging and torrent-description workflows. It preserves the intent of the original Windows-era app, but restructures the product around one shared engine with two front ends:

- an Avalonia desktop app for Windows, macOS, and Linux
- a CLI for scriptable and automation-friendly use

Both surfaces run the same shared workflow and use the same settings model.

## What The App Does

TDMaker is built to take one or more media inputs and turn them into release-ready output.

Core workflow features:

- inspect media files and disc layouts with MediaInfo CLI
- classify input as `SingleFile`, `FileCollection`, `Disc`, or `AudioAlbum`
- derive release title and source labels, with manual override support
- generate screenshots with FFmpeg
- randomize screenshot timestamps across the runtime
- optionally combine screenshots into a contact sheet
- optionally upload screenshots to `ptpimg`
- render publish text from migrated legacy templates
- generate `.torrent` files with `MonoTorrent`
- optionally write legacy XML upload payloads
- write output into a deterministic per-release folder

## Desktop App Features

The Avalonia app in `src/TDMaker.App` is the interactive workbench.

Workbench features:

- queue one or more file or directory inputs
- browse for input files and folders from the UI
- add and remove inputs during a session
- inspect without running the full workflow
- run the full shared workflow from the UI
- view resolved external tool status and paths
- download a managed FFmpeg build into `TDMAKER_HOME/tools` when FFmpeg is missing
- download a managed MediaInfo build into `TDMAKER_HOME/tools` when MediaInfo is missing
- select an active profile from the settings model
- override title, source label, and profile output directory per run
- browse for output directories and external tool paths from the UI
- inspect summarized asset cards for each discovered media file
- view detailed MediaInfo text for the selected asset
- preview generated publish text before posting it elsewhere
- see output directory and workflow warnings in-session

Settings features:

- configure `ffmpeg` and `mediainfo` paths explicitly
- select publish preset
- configure `ptpimg` API key
- enable or disable screenshots, uploads, contact sheets, torrent creation, and XML export
- configure screenshot count and contact-sheet column count
- enable or disable random screenshot frame selection
- wrap publish text in `[pre]`
- center-align publish text
- choose full-size image links vs thumbnail-oriented links
- hide private full paths from publish output

## CLI Features

The CLI in `src/TDMaker.Cli` is built for repeatable terminal workflows and automation.

Commands:

- `tools`: show resolved external tool paths and readiness
- `install-ffmpeg`: download a managed FFmpeg build into the app tools directory
- `install-mediainfo`: download a managed MediaInfo build into the app tools directory
- `inspect`: inspect media and print a summary without producing full workflow output
- `run`: execute the full shared release workflow

CLI options:

- `--profile` / `-p`: choose a saved profile
- `--title`: override the detected title
- `--source`: override the detected source label
- `--output` / `-o`: override the output directory for a run
- `--screenshots <bool>`: override screenshot creation on or off
- `--upload <bool>`: override upload behavior on or off
- `--torrent <bool>`: override torrent creation on or off
- `--xml`: enable legacy XML output for the run

The CLI and the desktop app use the same settings file and the same infrastructure services.

## Supported Inputs

The shared media inspector currently supports:

- single video files
- collections of media files
- audio-oriented folders and file sets
- disc-style folders such as `VIDEO_TS` and `BDMV`

Default recognized media extensions:

- video: `.mkv`, `.mp4`, `.avi`, `.m2ts`, `.ts`, `.mov`, `.vob`, `.mpg`, `.mpeg`
- audio: `.flac`, `.mp3`, `.m4a`, `.aac`, `.ogg`, `.opus`, `.wav`

## Output Artifacts

Depending on profile and runtime options, TDMaker can produce:

- publish text files
- screenshot image files
- screenshot contact sheets
- uploaded screenshot URLs
- per-tracker `.torrent` files
- XML upload payloads

Outputs are grouped into a release-specific folder, either under the configured profile output root or next to the source input when no explicit output root is set.

## Profiles And Presets

The app ships with two default profiles:

- `movies`
- `music`

Current publish presets exposed by the modern app:

- `Default`
- `MTN`
- `Minimal`
- `BTN`

Profiles control workflow behavior such as screenshot count, torrent creation, XML export, publish formatting, file extension support, tracker configuration, and upload behavior.

## Shared Architecture

The modern repo is intentionally split into UI, CLI, and shared core/infrastructure layers.

- `src/TDMaker.Core`: domain models, abstractions, and shared release orchestration
- `src/TDMaker.Infrastructure`: platform-aware implementations for process execution, tool discovery, settings, media inspection, screenshots, uploads, publishing, and torrents
- `src/TDMaker.Cli`: command-line front end
- `src/TDMaker.App`: Avalonia desktop front end
- `tests/TDMaker.Core.Tests`: shared behavior tests
- `docs`: architecture and tooling notes
- `legacy`: archived WinForms/WPF/Mono code kept for migration reference

Platform abstraction rules:

- UI behavior stays in `TDMaker.App`
- terminal behavior stays in `TDMaker.Cli`
- filesystem roots are resolved through `IPlatformPaths`
- external executables are resolved through `IExternalToolLocator`
- process launching goes through `IProcessRunner`
- workflow services are injected behind interfaces so behavior can vary per OS without forking app logic

## Configuration And Storage

By default, TDMaker stores runtime files under the OS application-data location in a `TDMaker` folder.

Managed paths include:

- `settings.json`
- `logs`
- `tools`
- `workspace`

You can override the app root with:

```powershell
$env:TDMAKER_HOME = "C:\path\to\tdmaker-home"
```

Tool resolution order:

1. explicit path in `settings.json`
2. app-local `tools` directory under `TDMAKER_HOME`
3. app base directory
4. `PATH`

## External Dependencies

The modern app deliberately keeps the required external binary surface small:

- `ffmpeg`: required for screenshot generation
- `mediainfo`: required for media inspection

TDMaker can automatically download a managed FFmpeg build for supported Windows, Linux, and macOS environments. The managed binary is stored under `TDMAKER_HOME/tools` and is picked up by the normal tool resolution flow.

TDMaker can also automatically download a managed MediaInfo build for supported Windows, Linux, and macOS environments. Windows uses the official MediaArea CLI ZIP, macOS uses the official MediaArea CLI DMG, and Linux uses the official MediaArea Ubuntu 24.04 CLI/library packages extracted into the managed tools directory.

No external torrent binary is required because torrent creation is handled in-process by `MonoTorrent`.

Legacy `mplayer` support is intentionally retired in the modern app.

See [docs/tooling.md](docs/tooling.md) for current upstream tool decisions and platform-specific notes.

## Build

```powershell
dotnet build TDMaker.slnx
dotnet test TDMaker.slnx
```

## Run

CLI:

```powershell
dotnet run --project src/TDMaker.Cli -- tools
dotnet run --project src/TDMaker.Cli -- inspect --help
dotnet run --project src/TDMaker.Cli -- run "D:\Media\Example.mkv"
```

Desktop app:

```powershell
dotnet run --project src/TDMaker.App
```

## Release Automation

TDMaker includes a repo-local publish skill at `.ai/skills/publish-release`.

The release flow is designed to:

- verify the release build
- bump `<Version>` in tracked `Directory.Build.props` files
- commit and tag `vX.Y.Z`
- trigger the GitHub Actions release workflow
- build separate archives for Windows, Linux, and macOS
- attach those archives to the matching GitHub release

Primary command:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh
```

Expected GitHub release assets:

- `TDMaker-X.Y.Z-windows-x64.zip`
- `TDMaker-X.Y.Z-linux-x64.tar.gz`
- `TDMaker-X.Y.Z-macos-arm64.tar.gz`

The packaging workflow builds both app surfaces into each asset:

- `desktop/`: Avalonia desktop app
- `cli/`: TDMaker CLI

The tag-triggered workflow definition lives in `.github/workflows/release-build-all-platforms.yml`.

## Current Scope

The modern stack already provides the shared workflow, desktop workbench, CLI, migrated publish templates, cross-platform tool discovery, settings persistence, and legacy archive.

The old Windows-only implementation remains available under `legacy/` for reference while any remaining feature-parity gaps are closed without contaminating the new architecture.

See [docs/architecture.md](docs/architecture.md) for the runtime model and migration direction.
