#!/usr/bin/env bash
# One-time project-local toolchain setup (no system installs).
set -euo pipefail
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [[ ! -x "$ROOT/tools/dotnet/dotnet" ]]; then
  echo "Installing portable .NET 9 SDK into tools/dotnet ..."
  "$ROOT/tools/dotnet-install.sh" --channel 9.0 --install-dir "$ROOT/tools/dotnet" --no-path
fi

if [[ ! -f "$ROOT/tools/BepInEx/BepInEx/core/BepInEx.dll" ]]; then
  echo "Downloading BepInEx 5.4.23.6 for compile references ..."
  mkdir -p "$ROOT/tools"
  curl -sSL -o "$ROOT/tools/BepInEx_win_x64_5.4.23.6.zip" \
    https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.6/BepInEx_win_x64_5.4.23.6.zip
  unzip -oq "$ROOT/tools/BepInEx_win_x64_5.4.23.6.zip" -d "$ROOT/tools/BepInEx"
fi

echo
echo "Toolchain ready. Next:"
echo "  ./build.sh"
echo "  ./deploy.sh"
