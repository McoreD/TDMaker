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

### MediaInfo CLI

- Status: required
- Purpose: structured media inspection and text summaries
- Upstream: <https://mediaarea.net/en/MediaInfo>
- Current CLI source release listed by MediaArea: `v26.01`

Platform notes:

- Linux: use distro packages when available or build from the published CLI/source packages
- Windows/macOS: use the official MediaArea downloads

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
