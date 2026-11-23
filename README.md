# Graveyard Orbit UI Scaling

A standalone BepInEx plugin that adds a **Text Size** option to Graveyard
Orbit's Display settings. It was built for the Steam Deck and other small
screens, but works at any resolution.

Independent of `Graveyard Orbit Tweaks` and the co-op mod; it can be installed
alongside both.

## ⚠️ Disclaimer

This project is **100% vibe coded** and **100% the product of AI use**. It is an
unofficial fan mod. I make **no claims of ownership** over Graveyard Orbit, its
code, assets, names, or anything derived from it. **All credit and all rights
belong to Anchorhead Games.** If you own the game, you are welcome to use this
mod for private play. No game code or game asset files are redistributed here.

## Install (Windows and Linux are the same)

You only need the release zip — no compiler, no setup, no separate Linux build.

1. Close Graveyard Orbit.
2. In Steam, right-click **Graveyard Orbit** → **Manage** → **Browse local files**.
   That opens the game folder (the one containing `GraveyardOrbit.exe`).
3. Download the latest release zip from
   **[Releases](https://github.com/Grokent/graveyard_orbit_ui_scaling/releases/latest)**
   (asset `GraveyardOrbit-UI-Scaling-0.1.0.zip`).
4. Extract **everything** from the zip into the game folder that opened in step 2.
   It only adds files (`winhttp.dll`, `doorstop_config.ini`,
   `.doorstop_version`, `changelog.txt`, `BepInEx/`); nothing in the original
   game is replaced.
5. **Linux / Proton only:** add this launch option once (game → Properties →
   Launch Options). Windows needs no launch options:

   ```
   WINEDLLOVERRIDES="winhttp=n,b" %command%
   ```

6. Launch the game through Steam normally.

To uninstall, delete `BepInEx/plugins/GraveyardOrbit.UIScaling/`. Remove
`winhttp.dll`, `doorstop_config.ini`, `.doorstop_version`, `changelog.txt` and
`BepInEx/` only if you do not use any other BepInEx mod.

## Using it

Open **Settings → Display → Text Size**, below Resolution:

| Setting | Effect | Notes |
|---|---|---|
| 100% | Current text size | Default |
| 125% | +25% text | Mild improvement |
| 150% | +50% text | Recommended for Steam Deck |
| 200% | +100% text | Experimental; dense menus can feel crowded |

Close the Settings modal to apply the new size.

Only font sizes change. The game's panel and HUD layout stays at its original
scale, so menu options and fixed panels stay onscreen instead of being pushed
out by a larger virtual canvas.

A few fixed-size cards deliberately keep their original text size:

- The Campaign / Overtime / Stats cards on the contracts home page.
- The `200×64` extractor purchase cards for lasers, compliance, etc.
- The NODE KEY legend is allowed to grow and wrap instead.

## How it works

- Reads and writes the `go_settings_ui_scale` PlayerPref.
- Scales UI Toolkit text by walking the open visual trees and adjusting
  `TextElement` font sizes.
- Scales legacy uGUI `Text` components in the HUD.
- Rescans open UI periodically so dynamically built screens pick up the setting.
- Keeps a baseline font size per element so changing the setting repeatedly does
  not compound.

No base game files are modified.

## Build from source (developers)

Requires a legitimate copy of the game, plus `unzip`, `curl` and Python 3.

```bash
./setup.sh      # project-local .NET SDK + BepInEx compile references
./build.sh      # builds the plugin
./deploy.sh     # additively installs BepInEx and copies the plugin
./package.sh    # rebuilds release/GraveyardOrbit-UI-Scaling-0.1.0.zip
```

Set `GAME_DIR` if the game is not in the default Steam library. Everything is
installed project-locally under `tools/`; nothing is installed system-wide.

## Layout

```
src/GraveyardOrbit.UIScaling/   plugin source
release/                        local package output (gitignored)
```

## License and credits

The mod's own code is MIT licensed (see `LICENSE`). The release zip bundles
BepInEx, which is LGPL-2.1 (see `THIRD_PARTY_NOTICES.md` and
`licenses/LGPL-2.1.txt`).

Graveyard Orbit and all related content are © Anchorhead Games. This is an
unofficial, fan-made, AI-generated mod with no affiliation with or endorsement
from Anchorhead Games. All rights to the game and its assets remain with them.

Repository: <https://github.com/Grokent/graveyard_orbit_ui_scaling>
