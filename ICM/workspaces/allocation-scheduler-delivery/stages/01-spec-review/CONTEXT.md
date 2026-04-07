# Spec Review

Audit the API spec against the component source to identify undocumented, spec-ahead, and mismatched parameters.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Paths to spec and source |
| API spec | docs/component-specs/allocation-scheduler/ | Full directory | What is documented |
| Component source | src/Marilo.Components/DataDisplay/AllocationScheduler/ | Parameter and event declarations only | What is implemented |
| Spec coverage format | shared/spec-coverage-format.md | Full file | Gap record format |

## Process

1. List all parameters in the component source (public API surface only).
2. List all parameters documented in the spec.
3. Produce three lists:
   a. Implemented but not documented (undocumented parameters).
   b. Documented but not implemented (spec ahead of code).
   c. Documented and implemented but mismatched (type, name, or behaviour description does not match the source).
4. For each item in list (a) and (c): create a spec gap record using shared/spec-coverage-format.md.
5. For each item in list (b): note whether it is a known planned gap or an unknown gap.
6. Produce a priority-ordered spec gap list.
7. Run the Audit checklist before writing to output/.
8. Write output/allocation-scheduler-spec-gap-list.md.

## Audit

| Check | Pass Condition |
|-------|----------------|
| All source parameters inventoried | Count matches source file scan |
| All spec parameters inventoried | Count matches spec file scan |
| No gap record missing a type classification (a/b/c) | Every gap has a type |
| Priority order justified | Each priority rank has a brief rationale |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Spec gap list | output/allocation-scheduler-spec-gap-list.md | spec-coverage-format.md |
