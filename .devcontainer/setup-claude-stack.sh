#!/usr/bin/env bash
# .devcontainer/setup-claude-stack.sh
# Installs and configures the ICM workspace Claude Code stack
# Runs once on container creation via postCreateCommand
# OpenWolf (.wolf/) is already installed via repo — this script wires the stack around it
set -euo pipefail

CLAUDE_CFG="${CLAUDE_CONFIG_DIR:-/home/vscode/.claude}"

# ──────────────────────────────────────────────
# OpenWolf: update config.json chrome_path now
# that the chromium feature has installed it
# ──────────────────────────────────────────────
echo "==> [0/5] OpenWolf — wiring Chromium path"
CHROME_BIN="$(command -v chromium-browser 2>/dev/null || command -v chromium 2>/dev/null || command -v google-chrome 2>/dev/null || true)"
if [ -n "$CHROME_BIN" ]; then
  node -e "
    const fs = require('fs');
    const p = '/workspaces/Marilo/.wolf/config.json';
    const cfg = JSON.parse(fs.readFileSync(p, 'utf8'));
    cfg.openwolf.designqc.chrome_path = '$CHROME_BIN';
    fs.writeFileSync(p, JSON.stringify(cfg, null, 2));
    console.log('    chrome_path set to $CHROME_BIN');
  "
else
  echo "    ! Chromium not found — designqc screenshots will be disabled until chrome_path is set manually"
fi

# ──────────────────────────────────────────────
# 1. MCP Project Management Skill
# ──────────────────────────────────────────────
echo "==> [1/5] MCP Project Management Skill"
npm install -g @mcpmarket/project-management-task-orchestrator 2>/dev/null || \
  npx --yes @mcpmarket/cli install project-management-task-orchestrator
echo "    ✓ MCP project-management installed"

# ──────────────────────────────────────────────
# 2. Superpowers skills
# ──────────────────────────────────────────────
echo "==> [2/5] Superpowers skills"
SKILLS_DIR="$CLAUDE_CFG/skills"
mkdir -p "$SKILLS_DIR"
if [ ! -d "$SKILLS_DIR/superpowers" ]; then
  git clone --depth 1 https://github.com/NickBayard/sf "$SKILLS_DIR/superpowers"
  echo "    ✓ Superpowers cloned to $SKILLS_DIR/superpowers"
else
  git -C "$SKILLS_DIR/superpowers" pull --ff-only
  echo "    ✓ Superpowers updated"
fi

# ──────────────────────────────────────────────
# 3. Claude-Mem MCP server
# ──────────────────────────────────────────────
echo "==> [3/5] Claude-Mem MCP server"
npm install -g claude-mem-mcp 2>/dev/null || \
  echo "    ! claude-mem-mcp not found on npm — install manually if the package becomes available"
MEM_STORE="${CLAUDE_MEM_STORE:-/workspaces/Marilo/.claude-mem}"
mkdir -p "$MEM_STORE"
echo "    ✓ Memory store initialised at $MEM_STORE"

# ──────────────────────────────────────────────
# 4. C# LSP + Stage 05 dotnet build gate
# ──────────────────────────────────────────────
echo "==> [4/5] C# LSP and Stage 05 build gate"
npm install -g @claude-code/lsp-csharp 2>/dev/null || \
  echo "    ! @claude-code/lsp-csharp not found — falling back to dotnet build gate only"

mkdir -p /workspaces/Marilo/.devcontainer
cat > /workspaces/Marilo/.devcontainer/stage05-build-gate.sh <<'GATE'
#!/usr/bin/env bash
# Stage 05 build gate — run before marking any gap implementation complete
# Checks BOTH the solution build and OpenWolf buglog
# Usage: bash .devcontainer/stage05-build-gate.sh [path/to/Project.slnx]
SLNX="${1:-/workspaces/Marilo/Marilo.slnx}"
echo "--- dotnet build gate: $SLNX"
BUILD_OUT=$(dotnet build "$SLNX" --no-restore -v quiet 2>&1)
if echo "$BUILD_OUT" | grep -q " Error\b"; then
  echo "BUILD FAILED — do not mark gap completed. Record errors in implementation log."
  echo "$BUILD_OUT" | grep -i " error"
  exit 1
fi
echo "BUILD PASSED"

BUGLOG="/workspaces/Marilo/.wolf/buglog.json"
if [ -f "$BUGLOG" ]; then
  OPEN_BUGS=$(node -e "
    const b = require('$BUGLOG');
    const open = (b.bugs||[]).filter(x=>x.status==='open').length;
    process.stdout.write(String(open));
  " 2>/dev/null || echo "0")
  if [ "$OPEN_BUGS" -gt "0" ]; then
    echo "WARNING: $OPEN_BUGS open bug(s) in .wolf/buglog.json — review before Stage 06"
  fi
fi
exit 0
GATE
chmod +x /workspaces/Marilo/.devcontainer/stage05-build-gate.sh
echo "    ✓ Stage 05 build gate written (uses Marilo.slnx + buglog check)"

# ──────────────────────────────────────────────
# 5. Claude mcp.json (write only if missing)
# ──────────────────────────────────────────────
echo "==> [5/5] Claude MCP config (~/.claude/mcp.json)"
MCP_FILE="$CLAUDE_CFG/mcp.json"
if [ ! -f "$MCP_FILE" ] || ! grep -q "project-management" "$MCP_FILE" 2>/dev/null; then
  cat > "$MCP_FILE" <<MCPEOF
{
  "mcpServers": {
    "project-management": {
      "command": "npx",
      "args": ["@mcpmarket/project-management-task-orchestrator"],
      "env": {
        "PLAN_FILE": "${GAP_PLAN_FILE:-/workspaces/Marilo/src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md}",
        "WORKSPACE_ROOT": "${GAP_WORKSPACE:-/workspaces/Marilo/workspaces/gap-analysis-resolution}",
        "STATUS_FIELDS": "not started,in progress,completed,blocked"
      }
    },
    "memory": {
      "command": "claude-mem-mcp",
      "args": ["--store", "${CLAUDE_MEM_STORE:-/workspaces/Marilo/.claude-mem}"],
      "env": {
        "NAMESPACE": "marilo-gap-analysis",
        "AUTO_SAVE_KEYS": "gap_context,routing_state,last_completed_stage"
      }
    },
    "lsp-csharp": {
      "command": "npx",
      "args": ["@claude-code/lsp-csharp", "--solution", "/workspaces/Marilo/Marilo.slnx"]
    }
  }
}
MCPEOF
  echo "    ✓ $MCP_FILE written"
else
  echo "    ~ $MCP_FILE already configured — skipped"
fi

# ──────────────────────────────────────────────
# Summary
# ──────────────────────────────────────────────
echo ""
echo "==> Setup complete. Verify with:"
echo "    claude mcp list"
echo "    ls $CLAUDE_CFG/skills/superpowers/"
echo "    ls ${CLAUDE_MEM_STORE:-/workspaces/Marilo/.claude-mem}"
echo "    node .wolf/hooks/session-start.js   # confirm OpenWolf hooks load"
