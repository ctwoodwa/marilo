# Component Delivery Workspace -- Template

Use when a component graduates to CDW-warranted status (see `workspaces/shared/workspace-routing.md`).
Target time to instantiate: under 5 minutes.

## When to Use

A CDW is warranted when a component is **Complex** tier or has active multi-phase gap work requiring
coordinated spec, Example UX, and source/test alignment. Check `workspace-routing.md` graduation criteria.

## How to Instantiate

1. Copy `treeview-delivery/` to a new sibling directory named `{component-slug}-delivery/`
2. Edit `_config/delivery-context.md` — fill in all `{{placeholder}}` fields (see template below)
3. Clear stage outputs: delete any files inside `stages/*/output/` (leave `.gitkeep` if present)
4. Open the new workspace's `CONTEXT.md` and run `setup` to verify routing works

## Folder Structure

```
{component-slug}-delivery/
├── CLAUDE.md                          (workspace identity, triggers, routing table)
├── CONTEXT.md                         (task routing — edit component name only)
├── _config/
│   └── delivery-context.md            (fill this in — see template below)
├── stages/
│   ├── 01-spec-review/
│   │   └── output/                    (clear before first run)
│   ├── 02-example-ux/
│   │   ├── shared/
│   │   └── output/                    (clear before first run)
│   └── 03-sync-check/
│       ├── shared/
│       └── output/                    (clear before first run)
└── shared/
    └── spec-coverage-format.md
```

## delivery-context.md Template

Replace `{{...}}` fields; all other values start at their defaults and are updated by stage runs.

| Placeholder | Fill in | Example |
|-------------|---------|---------|
| `{{component-name}}` | PascalCase display name | `DataGrid` |
| `{{component-slug}}` | lowercase directory slug | `datagrid` |
| `{{complexity-tier}}` | from routing.md | `Complex (CDW warranted)` |
| `{{active-phase}}` | current dev phase | `Phase 1` |
| `{{source-subfolder}}` | subfolder under Marilo.Components/ | `Data` |
| `{{test-path}}` | subfolder under Marilo.Tests.Unit/ | `P1Core` |

```markdown
# Delivery Context -- {{component-name}}

## Component Identity
| Component name | {{component-name}} |
| Component slug | {{component-slug}} |
| Complexity tier | {{complexity-tier}} |
| Active phase   | {{active-phase}} |

## Artifact Paths
| API spec         | /workspaces/Marilo/docs/component-specs/{{component-slug}}/ |
| Example UX       | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/{{component-name}}/ |
| Component source | /workspaces/Marilo/src/Marilo.Components/{{source-subfolder}}/ |
| Test files       | /workspaces/Marilo/tests/Marilo.Tests.Unit/{{test-path}}/{{component-name}}Tests.cs |
| Gap workspace    | /workspaces/Marilo/workspaces/gap-analysis-resolution |

## Spec State
| Spec version | unversioned | Last spec audit | not yet run | Open spec gaps | not yet run |

## Example UX State
| Demo page(s) | {{component-name}}/Overview.razor | Last demo audit | not yet run | Open demo gaps | not yet run |

## Delivery Gate
| Last sync check | not yet run | Gate status | PENDING | Blocking items | not yet run |

## Gap Workspace Link
| Latest closure reports | /workspaces/Marilo/workspaces/gap-analysis-resolution/stages/06-validate/output/ |
| Coverage summary       | /workspaces/Marilo/workspaces/gap-analysis-resolution/_config/coverage-summary.md |
```
