---
name: publish-release
description: "Orchestrate the TDMaker release flow in strict order: run maintenance prep first, confirm changelog only when a changelog file exists, verify build, bump/commit/push/tag, monitor the tag-triggered GitHub Actions workflow every 2 minutes by default, verify GitHub release assets for Windows, Linux, and macOS, then mark the release as pre-release by default unless stable is explicitly requested."
---

# TDMaker Release Publish

## Overview

Use this skill to run release steps in strict order:

1. maintenance prep first
2. changelog confirmation second only when `docs/CHANGELOG.md` exists
3. verify build, then execute bump/commit/push/tag automation
4. monitor the tag-triggered release workflow every 2 minutes by default
5. if failure occurs, inspect logs, fix issues, and retry with the next patch version
6. verify the GitHub release has the expected platform assets
7. set the successful release as pre-release by default unless intentionally publishing stable

Step 3 performs:

- Pre-check: run `dotnet build TDMaker.slnx -c Release`; do not proceed if build fails.
- Prompt for `x/y/z` bump type unless specified.
- Update tracked `Directory.Build.props` files that define `<Version>`.
- Stage all current repo changes.
- Commit with a version-prefixed message.
- Push current branch and create/push annotated tag `vX.Y.Z`.

Step 4-7 performs:

- Find tag run for `Release Build (Windows, Linux, macOS)`.
- Poll run status every 120 seconds until completion.
- On failure, inspect failing job logs and identify the first blocking error.
- Fix root cause in code, workflow, or scripts.
- Re-run local pre-check build.
- Retry release using the next patch bump, then monitor again.
- Verify the final GitHub release contains:
  - `TDMaker-X.Y.Z-windows-x64.zip`
  - `TDMaker-X.Y.Z-linux-x64.tar.gz`
  - `TDMaker-X.Y.Z-macos-arm64.tar.gz`

## Primary Command

From repository root:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh
```

Automated monitor + default pre-release:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh --set-prerelease --bump z --yes
```

Stable release opt-out:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh --no-prerelease --bump z --yes
```

Preview only:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh --bump z --dry-run --yes
```

## When bash is unavailable

On environments where `bash` is not available, execute the flow manually:

1. Maintenance
   - `git status --short`
   - `git pull --ff-only`

2. Optional changelog confirmation
   - If `docs/CHANGELOG.md` exists, update it before release.

3. Build verify
   - `dotnet build TDMaker.slnx -c Release`

4. Bump, commit, push, tag
   - Read current version from root `Directory.Build.props`.
   - Compute next version: patch `Z+1`, minor `Y+1.0`, major `X+1.0.0`.
   - Ensure tag `v<new-version>` does not exist locally or on `origin`.
   - Update tracked `Directory.Build.props` files containing `<Version>`.
   - `git add -A`
   - `git commit -m "[v<new-version>] [CI] Release v<new-version>"`
   - `git push origin <current-branch>`
   - `git tag -a v<new-version> -m "v<new-version>"`
   - `git push origin v<new-version>`

5. Monitor workflow
   - `gh run list --limit 10 --json databaseId,workflowName,headBranch,status,conclusion,url`
   - `gh run view <run-id> --json status,conclusion,jobs,url`

6. Verify release assets
   - `gh release view v<new-version> --json assets,url`
   - Confirm the Windows, Linux, and macOS assets are attached.

7. Set pre-release by default
   - `gh release edit v<new-version> --prerelease`

## Behavior

1. Run maintenance first unless explicitly bypassed.
2. If `docs/CHANGELOG.md` exists, require that step before bumping.
3. Always verify the build before bump/tag.
4. Run `scripts/bump-version-commit-tag.sh`.
5. After tag push, monitor the release workflow every 120 seconds until complete unless `--no-monitor` is intentionally used.
6. On failure, inspect logs, fix root cause, and retry with the next patch version.
7. Verify the final GitHub release contains the expected Windows, Linux, and macOS assets.
8. Mark successful release as pre-release by default unless intentionally publishing stable.

## Guardrails

- Do not skip maintenance unless explicitly requested.
- Do not skip build verification.
- Do not stop at tag creation; monitor the workflow to completion.
- Do not consider the release successful until the GitHub release has the expected assets.
- Always use a new patch version for retries that require new commits or tags.
- Abort on detached HEAD.
- Abort if version format is not `X.Y.Z`.
- Abort if the tag already exists locally or remotely.
- Support `--no-push` and `--no-tag` when a partial flow is intentionally required.

## Notes

- TDMaker currently has historical GitHub tags and releases through `v4.1.0`, and the modern repo baseline is now `v5.0.0`.
- The modern release workflow targets only Windows, Linux, and macOS.
- Each platform asset contains both front ends:
  - `desktop/`: Avalonia desktop app
  - `cli/`: TDMaker CLI
- The release workflow uses native GitHub runners per OS family and uploads the final archives to the GitHub release created for the tag.
