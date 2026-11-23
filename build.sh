#!/usr/bin/env bash
# Build the UI Scaling plugin with the project-local .NET SDK.
set -euo pipefail
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
DOTNET="${DOTNET:-$ROOT/tools/dotnet}"

if [[ ! -x "$DOTNET/dotnet" ]]; then
  DOTNET="$(command -v dotnet || true)"
fi

[[ -n "$DOTNET" ]] || { echo "No dotnet SDK found. Run ./setup.sh first." >&2; exit 1; }

export DOTNET_ROOT="$DOTNET"
export PATH="$DOTNET:$PATH"
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

dotnet build "$ROOT/src/GraveyardOrbit.UIScaling/GraveyardOrbit.UIScaling.csproj" -c Release "$@"
