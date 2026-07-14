---
name: ship
description: Commit the current working changes, update the README, and push the feature branch. Reviews the diff for leftover debug cruft, splits gameplay code from bulk asset imports into separate commits, writes descriptive messages, and never pushes to main. Use when the user says "ship it", "push this", "commit and push", or asks to wrap up the current change.
---

# ship

Package up the current working changes and get them onto the remote feature branch,
the way this project likes it.

## Steps

1. **Survey the changes.** Run `git status -sb` and `git diff` (plus `git diff --staged`
   if anything is already staged). Understand what actually changed before writing any
   message — the message must describe the real diff, not an assumption.

2. **Catch cruft before it's committed.** Scan the diff for things that shouldn't enter
   shared history: `Debug.Log`/`print` calls (especially in `Update`/`FixedTick`/per-frame
   paths — they spam the console), commented-out code, temp/scratch files, leftover
   `TODO`-marked experiments. If you find any, flag it and ask whether to remove it before
   committing. Don't silently strip the user's code, and don't silently immortalize obvious
   debug leftovers either.

3. **Group into commits.** Keep related things together, unrelated things apart:
   - Gameplay **code** (`Assets/Scripts/**`) plus the **scene/prefab** wiring it needs
     (`Assets/Scenes/**`, serialized field values) and doc updates → one feature commit.
   - Bulk **art/asset imports** (`Assets/Objects/**`, `Assets/Materials/**`, models,
     textures, `.fbx`/`.blend`/`.png`) → their own commit.
   This mirrors the user's stated preference to not mix code with asset imports.

4. **Write good messages.** Imperative subject line (~50–72 chars), then a body paragraph
   explaining *what* and *why*. End every commit message with:
   ```
   Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>
   Claude-Session: https://claude.ai/code/session_01EZupzZBJRpmACdh4GWWk3t
   ```
   Use `git commit -F -` with a heredoc for multi-line messages. Commit messages are in
   **English**.

5. **Update the README.** `README.md` lives at the project root and is written in **German**
   (the user's language). Move any now-finished "geplante nächste Schritte" into the
   **"Aktueller Stand"** section, and record the next planned step based on the recent
   conversation. Commit it with the feature commit.

6. **Push — never to `main`.** Push the *current* feature branch explicitly:
   `git push -u origin <current-branch>`. This repo's upstream tracking may point at
   `origin/main` by mistake, so never rely on a bare `git push`. Git LFS uploads happen
   automatically — that's expected (this is a Unity + LFS project). If GitHub prints a
   "Create a pull request" URL, surface it to the user.

## Project notes

- Unity 6000.5.3f1 project, Git LFS tracks the binary assets.
- Preserve existing identifier misspellings (`RBManiplulator`, `accellaration`,
  `jumpStrenght`, `ray_lenght`) — do not "fix" them, it breaks call sites.
- The LF→CRLF warnings on Windows are harmless; ignore them.
