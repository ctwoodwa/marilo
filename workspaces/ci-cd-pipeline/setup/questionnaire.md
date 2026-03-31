# Setup Questionnaire

Run the `setup` trigger to start onboarding. The agent asks these questions conversationally
and fills all placeholders across `_config/` from your answers.

---

1. What is the project name? (e.g. Comet)
2. What is the project slug? Short, lowercase, hyphenated. (e.g. comet)
3. Where is the solution file? Absolute path. (e.g. C:\Projects\AF\comet\src\Parsons.Comet.slnx)
4. What is the default branch? (default: main)
5. What CI/CD platform will you use? (github-actions or azure-pipelines)
6. What is the repository host URL? (e.g. https://github.com/org/comet)
7. What container registry will you use? (e.g. ghcr.io/org, myregistry.azurecr.io — or "none")
8. What .NET version does the solution target? (e.g. 10.0)
9. What Node version is required for the npm build steps? (e.g. 20)
10. Does any project in the solution run npm install and npm build before dotnet build? (yes/no)
    If yes, which project folder contains the package.json?
11. Are there any Docker targets (Dockerfile or DockerDefaultTargetOS properties)? (yes/no)
12. Does the solution use an Aspire AppHost project? (yes/no)
    If yes, what is the AppHost project path?
13. What are your environment names? (default: dev, test, prod)
14. How do branches map to environments?
    (e.g. develop → dev, main → test, tags/v* → prod)
15. Where should failure notifications go? (e.g. email, Slack webhook — or "none")

---

Derived automatically:
- `{{PROJECT_SLUG}}` from Q2
- `{{HAS_NPM_BUILD}}` from Q10
- `{{HAS_DOCKER}}` from Q11
- `{{HAS_ASPIRE}}` from Q12
- `{{DEPLOY_BRANCH_MAP}}` from Q14
