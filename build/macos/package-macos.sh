#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null 2>&1 && pwd)"
ROOT="$(cd "$SCRIPT_DIR/../.." >/dev/null 2>&1 && pwd)"
RUNTIME_IDENTIFIER="${TDMAKER_RUNTIME_IDENTIFIER:-osx-arm64}"

pwsh -File "$ROOT/build/package-release.ps1" -RuntimeIdentifier "$RUNTIME_IDENTIFIER"
