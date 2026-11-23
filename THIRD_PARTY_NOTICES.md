# Third-party notices

This repository contains **only the Graveyard Orbit UI Scaling mod's own source
code**. No game files and no third-party binaries are committed here. The build
output and the local toolchain under `tools/` are gitignored.

The redistributable ZIP produced by `package.sh` bundles the BepInEx mod loader
and its runtime dependencies. Those components keep their own licenses:

## BepInEx 5.4.23.6 — LGPL-2.1

- Project: https://github.com/BepInEx/BepInEx
- License: GNU Lesser General Public License v2.1
- Full text: [`licenses/LGPL-2.1.txt`](licenses/LGPL-2.1.txt)
- Source code for the exact binaries shipped: https://github.com/BepInEx/BepInEx/tree/v5.4.23.6

BepInEx is redistributed unmodified as separate DLLs inside the ZIP. It is
loaded dynamically by the game at runtime; this mod does not link against or
modify it at build time. Because the LGPL requires the ability to relink, the
corresponding BepInEx source and build instructions are available at the link
above, and the ZIP includes the unmodified loader files.

## HarmonyX / 0Harmony — MIT

- Project: https://github.com/BepInEx/HarmonyX
- License: MIT

## MonoMod — MIT

- Project: https://github.com/MonoMod/MonoMod
- License: MIT

## Mono.Cecil — MIT

- Project: https://github.com/jbevain/cecil
- License: MIT

---

## Game content

This is an unofficial, AI-generated fan mod. Graveyard Orbit and its assets
belong to Anchorhead Games. No game code or game asset files are redistributed
in this repository or in the release zip.
