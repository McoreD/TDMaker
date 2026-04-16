# Architecture

## Intent

The legacy application was a torrent-description and media packaging tool. Its practical workflow was:

1. inspect one file, a set of files, or a disc layout
2. generate screenshots
3. upload screenshots if configured
4. render publish text from tracker-oriented templates
5. create torrent output
6. optionally export XML for older upload flows

The modern repo keeps that intent intact while removing the Windows-only stack.

## Solution Shape

- `TDMaker.Core` contains the workflow model and abstractions.
- `TDMaker.Infrastructure` implements those abstractions with cross-platform services.
- `TDMaker.Cli` exposes the shared workflow to scripts and terminal users.
- `TDMaker.App` exposes the same workflow through Avalonia.

Neither front end owns release logic. They both build a `ReleaseRequest` and invoke the shared services.

## Platform Abstraction Rules

The repo follows these platform boundaries:

- UI concerns stay in `TDMaker.App`.
- CLI concerns stay in `TDMaker.Cli`.
- Filesystem roots and app data paths go through `IPlatformPaths`.
- External executable resolution goes through `IExternalToolLocator`.
- Process execution goes through `IProcessRunner`.
- Media inspection, screenshots, uploads, publishing, settings, and torrent output are all injected behind interfaces.

That means the app can vary per OS without forking the workflow or the front-end logic.

## Current Shared Workflow

`ReleaseWorkflow` is the center of the modern app:

1. inspect inputs with MediaInfo CLI
2. determine output directory
3. create screenshots with FFmpeg when enabled
4. upload screenshots when enabled
5. render publish text using the migrated legacy templates
6. create torrent files with MonoTorrent when enabled
7. write XML when enabled

## Disc Handling

The legacy repo vendored a BDInfo project and used Windows-specific media stacks. The new implementation starts with MediaInfo-based disc inspection so the workflow is cross-platform first. If richer Blu-ray playlist analysis is needed later, it should be added behind the same `IMediaInspector` boundary rather than baked into the UI or CLI.
