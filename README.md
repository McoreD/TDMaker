# TDMaker

TDMaker is being rebuilt as a modern cross-platform toolkit for torrent-description workflows.

The new repo layout is:

- `src/TDMaker.Core`: shared domain models and workflow contracts used by both front ends
- `src/TDMaker.Infrastructure`: cross-platform implementations for external tools, filesystem access, HTTP upload, and torrent output
- `src/TDMaker.Cli`: automation-oriented command-line interface
- `src/TDMaker.App`: Avalonia desktop application for Windows, macOS, and Linux
- `tests/TDMaker.Core.Tests`: tests for shared behavior

The legacy WinForms/WPF/Mono code remains in the repo during migration as a reference while the new implementation is brought to feature parity.
