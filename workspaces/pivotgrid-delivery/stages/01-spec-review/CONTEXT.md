# Spec Review

Audit the API spec against the component source to identify undocumented, spec-ahead, and mismatched parameters.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Paths to spec and source |
| API spec | /workspaces/Marilo/docs/component-specs/pivotgrid/ | Full directory | What is documented |
| Component source | UNKNOWN | Parameter and event declarations only | What is implemented |
| Spec coverage format | shared/spec-coverage-format.md | Full file | Gap record format |
| Gap workspace closure reports | /workspaces/Marilo/workspaces/pivotgrid-gap-analysis/stages/06-validate/output/ | List + read relevant reports | Current resolution state |

## Process

1. List all parameters in the component source (public API surface only).
2. List all parameters documented in the spec.
3. Produce three lists:
   a. Implemented but not documented (undocumented parameters).
   b. Documented but not implemented (spec ahead of code).
   c. Documented and implemented but mismatched.
4. For each item in list (a) and (c): create a spec gap record.
5. For each item in list (b): note whether it is a known planned gap or unknown.
6. Produce a priority-ordered spec gap list.
7. Run the Audit checklist before writing to output/.
8. Write output/pivotgrid-spec-gap-list.md.
9. Update _config/delivery-context.md: last spec audit date and open spec gap count.

## Audit

| Check | Pass Condition |
|-------|----------------|
| All source parameters inventoried | Count matches source file scan |
| All spec parameters inventoried | Count matches spec file scan |
| No gap record missing a type classification (a/b/c) | Every gap has a type |
| Priority order justified | Each priority rank has a brief rationale |
| No spec content duplicated in this output | Output references spec, not copies it |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Spec gap list | output/pivotgrid-spec-gap-list.md | spec-coverage-format.md |
