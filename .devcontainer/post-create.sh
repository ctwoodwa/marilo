#!/usr/bin/env bash
# .devcontainer/post-create.sh
# Runs once on container creation via postCreateCommand
set -euo pipefail

WORKSPACE="/workspaces/Marilo"
cd "$WORKSPACE"

echo "==> Installing system dependencies"
sudo apt-get update && sudo apt-get install -y chromium-browser --no-install-recommends

echo "==> Installing .NET Aspire workload"
dotnet workload install aspire

echo "==> Trusting dev certificates"
dotnet dev-certs https --trust

echo "==> Restoring .NET packages"
dotnet restore "$WORKSPACE/Marilo.slnx"

echo "==> Installing Node dependencies"
npm install

# OpenWolf hooks (only if .wolf/hooks exists — it may be gitignored)
if [ -f ".wolf/hooks/package.json" ]; then
  echo "==> Installing OpenWolf hook dependencies"
  npm --prefix .wolf/hooks install
else
  echo "==> Skipping OpenWolf hooks (not present)"
fi

# Claude stack setup (only if script exists)
if [ -f ".devcontainer/setup-claude-stack.sh" ]; then
  echo "==> Running Claude stack setup"
  bash .devcontainer/setup-claude-stack.sh
fi

echo "==> Installing Claude Code skills"
npx skills add anthropic/feature-dev
npx skills add anthropic/frontend-design

echo "==> Setting up Context7 MCP"
npx ctx7 setup --claude --api-key ctx7sk-60b1429c-f0c4-4ef6-bb7d-c6761a69842a

echo "==> Installing Playwright Chromium"
npx playwright install --with-deps chromium

echo "==> Post-create complete"
