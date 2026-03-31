# Setup Questionnaire

Run the `setup` trigger to start onboarding. The agent asks these questions conversationally
and fills all placeholders across `_config/` from your answers.

---

1. What is the project name? (e.g. Comet)
2. What is the project slug? Short, lowercase, hyphenated. (e.g. comet)
3. Where is the solution file? Absolute path. (e.g. C:\Projects\AF\comet\src\Parsons.Comet.slnx)
4. Where is the existing test project? Absolute path to the project folder.
5. What unit/integration test framework is already in use? (default: NUnit 4)
6. What Blazor component test framework is in use? (default: bunit — or "none" if not installed)
7. What coverage collection tool should be used? (default: Coverlet via coverlet.collector)
8. What coverage report format is preferred? (default: Cobertura XML + HTML via ReportGenerator)
9. What unit test coverage target percentage? (default: 80)
10. What integration test coverage target percentage? (default: 70)
11. What component test coverage target percentage? (default: 60)
12. Are there any source projects or classes that should be excluded from the coverage audit?
    (e.g. generated code, migrations, Program.cs — or "none")

---

Derived automatically:
- `{{PROJECT_SLUG}}` from Q2
- Coverage targets in `_config/coverage-context.md` from Q9–Q11
- `{{SCOPE_EXCLUSIONS}}` from Q12
