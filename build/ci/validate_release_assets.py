#!/usr/bin/env python3
import argparse
from pathlib import Path
import sys


def main() -> int:
    parser = argparse.ArgumentParser(description="Validate expected TDMaker release assets.")
    parser.add_argument("--assets-dir", required=True)
    parser.add_argument("--version", required=True)
    args = parser.parse_args()

    assets_dir = Path(args.assets_dir)
    version = args.version

    expected = [
        f"TDMaker-{version}-windows-x64.zip",
        f"TDMaker-{version}-linux-x64.tar.gz",
        f"TDMaker-{version}-macos-arm64.tar.gz",
    ]

    missing = [name for name in expected if not (assets_dir / name).exists()]
    if missing:
        for name in missing:
            print(f"Missing expected asset: {name}", file=sys.stderr)
        return 1

    print("Validated release assets:")
    for name in expected:
        print(f"- {name}")

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
