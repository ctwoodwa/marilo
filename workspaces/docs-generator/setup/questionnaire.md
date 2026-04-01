# Setup Questionnaire

Run the `setup` trigger to start onboarding. The agent asks these questions conversationally
and fills all placeholders across `_config/` from your answers.

Answer everything in one message if you can. Every question has a default you can accept.

---

1. What is the project name? (e.g. Comet)
2. What is the project slug? Short, lowercase, hyphenated identifier used in output file names. (e.g. comet)
3. What type of project is this? (e.g. Web API, Blazor App, CLI tool, library, microservice)
4. What is the primary language and runtime? (e.g. C#/.NET 9, TypeScript/Node 22, Python 3.12)
5. What is the framework? (e.g. ASP.NET Core, React, FastAPI — or "none")
6. Where is the project root? Absolute path or `../..` if running deployed inside the project.
   (e.g. C:\Projects\AF\comet)
7. Where is the docs folder? Absolute path or relative to project root.
   (default: `{{PROJECT_ROOT}}/docs`)
8. Where is the source folder? (default: `{{PROJECT_ROOT}}/src`)
9. Where is the test folder? (default: `{{PROJECT_ROOT}}/tests` — or "none")
10. Who are the primary readers of the docs? (e.g. backend developers on the team)
11. Who are the secondary readers? (e.g. new hires, QA engineers, product managers — or "none")
12. What is the audience's knowledge level? (e.g. intermediate .NET developers familiar with REST APIs)
13. What is the repo URL? (e.g. https://github.com/org/comet — or "none")
14. What CI/CD platform? (default: GitHub Actions — or "none")
15. What doc tone should be used?
    (default: direct and precise, assume developer audience)
16. Which doc types do you want to generate or update? List them in priority order, or accept all defaults.
    (defaults: README, architecture overview, API reference, component docs, how-to guides, ADRs, runbooks)
17. Are there any files or folders to exclude from the scan?
    (e.g. auto-generated files, vendor folders — or "none")

---

Derived automatically from your answers (no need to answer separately):

- `{{PROJECT_SLUG}}` from Q2
- `{{DOCS_FOLDER}}` from Q7 (resolves `{{PROJECT_ROOT}}` from Q6)
- `{{SOURCE_FOLDER}}` and `{{TEST_FOLDER}}` from Q8 and Q9
- Priority order and actions in `_config/scope.md` from Q16
- `{{SCOPE_EXCLUSIONS}}` from Q17
