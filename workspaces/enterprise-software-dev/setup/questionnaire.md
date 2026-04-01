# Setup Questionnaire

Run the setup trigger to start onboarding. The agent asks these questions conversationally
and fills all placeholders across the workspace from your answers.

Answer everything in one message if you can. Every question has a default you can accept.

---

1. What is your organization name? (e.g. Acme Corp)
2. What is the solution name? This becomes the root namespace and project prefix. (e.g. MyPlatform)
3. What is the business domain? One sentence. (e.g. B2B order management for logistics companies)
4. What environments will you use? (default: dev, test, prod)
5. What is your primary cloud provider? (default: Azure)
6. What .NET target framework? (default: net9.0)
7. What database provider? (default: SQL Server)
8. What identity provider? (default: Azure AD B2C)
9. What observability backend? (default: Aspire Dashboard for dev + Azure Monitor for test/prod)
10. What CI/CD platform? (default: GitHub Actions)
11. What container runtime? (default: Docker)
12. Any compliance requirements? (e.g. GDPR, SOC2, HIPAA - or none)
13. What architecture style do you prefer? (default: layered with ports-and-adapters at service boundaries)
14. Unit test coverage target percentage? (default: 80)
15. What message broker if any? (default: none - add Azure Service Bus or RabbitMQ if needed)
16. What AI coding tools do you use? (e.g. Claude Code on personal machine + GitHub Copilot in VS Code and Rider at work)
    This determines what Stage 05 generates: copilot-instructions.md for Copilot, or a CLAUDE.md project
    file for Claude Code, or both.

---

Derived automatically from your answers (no need to answer separately):
- Solution namespace from solution name
- Project names from solution name + service type
- OTEL exporter endpoints from cloud provider selection
- Key vault naming from org name + environment
- Stage 05 outputs scoped to AI tool selection from Q16