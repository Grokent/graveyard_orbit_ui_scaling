#!/usr/bin/env bash
# Additively install BepInEx (if missing) and copy the UI Scaling plugin in.
set -euo pipefail
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
GAME="${GAME_DIR:-$HOME/.local/share/Steam/steamapps/common/Graveyard Orbit}"
DLL="$ROOT/src/GraveyardOrbit.UIScaling/bin/Release/GraveyardOrbit.UIScaling.dll"
BEP_ZIP="$ROOT/tools/BepInEx_win_x64_5.4.23.6.zip"

[[ -d "$GAME" ]] || { echo "Game folder not found: $GAME" >&2; exit 1; }
[[ -f "$DLL" ]] || { echo "Plugin not built. Run ./build.sh first." >&2; exit 1; }

if [[ ! -d "$GAME/BepInEx" ]]; then
  [[ -f "$BEP_ZIP" ]] || { echo "Missing $BEP_ZIP (run ./setup.sh)." >&2; exit 1; }
  echo "Installing BepInEx 5.4.23.6 (additive)"
  unzip -oq "$BEP_ZIP" -d "$GAME"
fi

DEST="$GAME/BepInEx/plugins/GraveyardOrbit.UIScaling"
mkdir -p "$DEST"
cp -f "$DLL" "$DEST/"

echo "Deployed -> $DEST/GraveyardOrbit.UIScaling.dll"
echo "Logs: $GAME/BepInEx/LogOutput.log"
