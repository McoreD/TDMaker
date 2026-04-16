# External Tooling

As of 2026-04-16, the modern app intentionally depends on only two external CLI tools.

## Required

### FFmpeg

- Status: required
- Purpose: screenshot extraction
- Upstream: <https://ffmpeg.org/download.html>
- Current stable release listed by FFmpeg: `8.1`, released on `2026-03-16`

Platform notes:

- Linux: prefer distribution packages from the FFmpeg download page
- Windows: FFmpeg's official download page points to compiled builds from `gyan.dev` and `BtbN`
- macOS: FFmpeg's official download page points to `evermeet.cx` static builds

Automatic install notes:

- TDMaker can download a managed FFmpeg build into `TDMAKER_HOME/tools`
- Windows managed installs currently use the `gyan.dev` release essentials ZIP
- Linux and macOS managed installs use Martin Riedl's scriptable release ZIP endpoints because they provide current amd64/arm64 artifacts suitable for unattended download flows
- macOS auto-download intentionally uses the Martin Riedl arm64/x64 ZIP endpoints instead of `evermeet.cx`, because the Evermeet site explicitly does not provide native Apple Silicon builds

### MediaInfo CLI

- Status: required
- Purpose: structured media inspection and text summaries
- Upstream: <https://mediaarea.net/en/MediaInfo>
- Current CLI release listed by MediaArea download pages: `v26.01`

Platform notes:

- Windows: the official MediaArea Windows download page publishes CLI ZIP packages, including x64 and ARM64 builds
- macOS: the official MediaArea macOS download page publishes a CLI DMG
- Linux: the official MediaArea Ubuntu download page publishes CLI, `libmediainfo0`, and `libzen0` packages for current amd64 and arm64 Ubuntu releases

Automatic install notes:

- TDMaker can download a managed MediaInfo build into `TDMAKER_HOME/tools`
- Windows managed installs use the official MediaArea CLI ZIP (`MediaInfo_CLI_26.01_Windows_x64.zip` or `MediaInfo_CLI_26.01_Windows_ARM64.zip`)
- macOS managed installs use the official MediaArea CLI DMG (`MediaInfo_CLI_26.01_Mac.dmg`) and copy the mounted payload into the managed tools directory
- Linux managed installs currently use the official Ubuntu 24.04 amd64/arm64 `mediainfo`, `libmediainfo0`, and `libzen0` packages, extract them into `TDMAKER_HOME/tools/mediainfo-managed`, and launch the CLI through a wrapper that exports the correct library path

## Retired

### MPlayer

- Status: intentionally not required in the modern app
- Legacy role: alternate screenshot backend
- Upstream: <https://mplayerhq.hu/design5/dload.html>
- Latest release listed by MPlayer: `1.5`, created on `2022-02-27`

Why it was dropped:

- the upstream release cadence is effectively dormant compared with FFmpeg
- the official site recommends current SVN snapshots because the release is old
- the included GUI is described upstream as having no further development and limited bug fixing
- FFmpeg already covers the screenshot workflow cross-platform

## No External Torrent Binary

The legacy app created torrent artifacts in-process. The modern app keeps that pattern and uses the `MonoTorrent` library instead of requiring a separate torrent CLI binary.

## Tool Discovery Contract

The modern app resolves `ffmpeg` and `mediainfo` in this order:

1. explicit path in `settings.json`
2. app-local tools directory under `TDMAKER_HOME`
3. app base directory
4. `PATH`

`TDMAKER_HOME` can be used to pin an app-specific tools directory on any OS.
