# Marilo CLAUDE.md

This file is the shared, always-on context for the Marilo Blazor component library.
Keep it short, trusted, and updated. Every line must earn its place.

---

# OpenWolf

@.wolf/OPENWOLF.md

This project uses OpenWolf for context management.
- Read and follow `.wolf/OPENWOLF.md` every session.
- Check `.wolf/cerebrum.md` before generating code.
- Check `.wolf/anatomy.md` before reading or bulk-editing files.

---

# Project overview

- Marilo is an enterprise-grade, provider-first Blazor component library for .NET 10.
- Components define behavior; providers define visuals via `IMariloProvider`.
- Core packages: `Marilo.Components`, `Marilo.Providers.FluentUI`, `Marilo.Icons`, `Marilo.Core`.
- Fluent UI 2 is the primary provider; future providers (Material, Bootstrap, etc.) must plug into the same abstractions.

---

# Architecture & layout

- Components live under `src/Marilo.Components`, following the ICM workspace pattern.
- Complex/enterprise components use CDW workspaces under `/workspaces/Marilo/workspaces/`.
- Providers, styles, and themes: `src/Marilo.Providers.*` with SCSS-based theming.
- Component specs live in `docs/component-specs/<slug>/` (one nested folder per component, e.g. `docs/component-specs/datagrid/`, `docs/component-specs/chart/tooltip/`). Slugs are lowercase.
- Component mapping is in `src/Marilo.Components/component-mapping.json`.
- Gap analysis and roadmap: `src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md`.

When in doubt:
- Copy existing patterns from nearby components.
- Prefer extending the ICM/CDW layout over inventing new top-level folders.
- Keep behavior in components and visual details in providers.

---

# How to work in this repo

Before significant work:
- Read `CONTEXT.md` and the relevant component spec under `docs/component-specs/<slug>/`.
- For complex components (DataGrid, Scheduler, AllocationScheduler, etc.), also check the CDW workspace under `/workspaces/Marilo/workspaces/<component>/`.

General rules:
- Keep edits small and focused; one concern per change.
- Update or add tests with each behavioral change.
- Keep source, specs, demos, and docs in sync for any public behavior change.
- Prefer improving existing APIs over adding parallel, overlapping ones.

---

# Workflow with Claude Code

Planning:
- Start non-trivial changes in Plan mode.
- Iterate on the plan until it matches the intended API/UX and affected areas.
- Break large changes into reviewable chunks (e.g., spec → component → provider → tests → docs).

Verification:
- Run `dotnet build` and `dotnet test` before marking work complete.
- For UI behavior, ensure the relevant demo page builds and runs.
- For new/changed components, add or update bUnit tests where it makes sense.
- If verification fails, fix the root cause before expanding the change.

Learnings:
- When Claude makes a mistake that shouldn’t recur, add a short bullet to the relevant spec or workspace doc.
- Keep this file focused on rules that apply to almost every session; move per-component details into `component-specs` or workspace-specific docs.

---

# Style & patterns

Coding:
- Follow existing Marilo component naming and folder structure.
- Prefer clear, explicit APIs over “magic” behavior.
- Keep public APIs stable; favor additive changes over breaking ones.
- For providers, keep visual decisions in provider code/SCSS; don’t hard-code look-and-feel in core components.

Docs & demos:
- Each public component should have:
  - A spec folder under `docs/component-specs/<slug>/`.
  - At least one representative demo.
  - XML doc comments for all public APIs.
- When changing behavior, update spec + docs + demos in the same change set.

Tests:
- Mirror patterns from similar components (e.g., existing TreeView / DataGrid tests).
- Keep tests fast and deterministic.
- Prefer bUnit for component behavior; add integration tests only when needed.

---

# Build, run, test (high level)

Assume .NET 10 SDK and standard commands:
- Build: `dotnet build` at the solution level.
- Test: `dotnet test` for all tests, or target specific test projects as needed.
- Run demos/sample app using the existing `dotnet run` commands defined in the repo.

If a more specific command is needed:
- Infer it from the `.sln` and `.csproj` structure.
- Reuse existing scripts and runner projects instead of adding new ones.

---

# Never do

These are high-cost mistakes. Do not do them without explicit instruction in the current session.

- Do not remove or rename ICM or CDW workspace folders.
- Do not change public component APIs or provider contracts without updating specs, demos, docs, and tests together.
- Do not introduce breaking changes to theming or provider contracts without a documented migration path.
- Do not bypass or delete gap-analysis notes in `GAP_ANALYSIS_RESOLUTION_PLAN.md` when closing gaps.
- Do not introduce secrets, credentials, or environment-specific config into the repo.
- Do not add new build pipelines, external services, or global dependencies without explicit guidance in this workspace.

---

# Orchestration (tmux parallel workers)

Marilo supports an optional tmux orchestration layer for parallel multi-component Claude Code work. It is **off by default** — if you are not running a session, nothing changes.

- Entry rules: `.claude/rules/orchestration.md`
- Operational guide: `.claude/orchestration/GUIDE.md`
- Session state: `.claude/orchestration/_orchestrator/session.json`
- Worker memory: `.claude/orchestration/_memory/workers/*.json`
- Templates: `.claude/orchestration/templates/*`
- Worker execution-discipline skills (vendored from [obra/superpowers](https://github.com/obra/superpowers), MIT): see `.claude/skills/NOTICE.md` — `test-driven-development`, `verification-before-completion`, `systematic-debugging`, `requesting-code-review`, `receiving-code-review`. These are enforced at the orchestrator review gate.

When `_orchestrator/session.json` has `status: "active"`, Claude operates in orchestrator mode or worker mode (role determined by tmux session / env vars). When `status: "inactive"` (default), normal single-session operation applies and orchestration rules do nothing.

Use orchestration only when:

- Work spans 2+ components with disjoint file ownership
- A phase is large enough to benefit from wall-clock parallelism
- You would otherwise want to run multiple Claude Code windows side-by-side

Architecture changes, public API changes, and provider contract changes remain orchestrator-only even inside a session — workers escalate instead of making them.

---

# Local overrides

- Put personal preferences in `CLAUDE.local.md` (gitignored).
- Use `CLAUDE.local.md` for editor habits, keyboard shortcuts, or personal workflows that should NOT affect the whole team.
- Keep this shared CLAUDE.md stable, concise, and team-owned.
