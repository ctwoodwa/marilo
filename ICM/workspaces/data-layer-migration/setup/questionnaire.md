# Setup Questionnaire

Run the `setup` trigger to start onboarding. The agent asks these questions conversationally
and fills all placeholders across `_config/` from your answers.

---

1. What is the project name? (e.g. Comet)
2. What is the project slug? Short, lowercase, hyphenated. (e.g. comet)
3. Where is the solution file? Absolute path. (e.g. C:\Projects\AF\comet\src\Parsons.Comet.slnx)
4. Where is the API project? Absolute path to the project folder. (e.g. C:\Projects\AF\comet\src\Parsons.Comet.ApiService)
5. Where is the in-memory data store class? Path to the .cs file. (e.g. C:\Projects\AF\comet\src\Parsons.Comet.ApiService\Services\InMemoryDataStore.cs)
6. What is the target database name? (e.g. Comet)
7. What is the SQL Server instance for dev? (default: localhost or (localdb)\mssqllocaldb)
8. What is the SQL Server instance for test? (e.g. test-sql.internal or an Azure SQL connection string reference)
9. What is the SQL Server instance for prod? (e.g. prod-sql.internal or an Azure SQL connection string reference)
10. What schema name should be used? (default: dbo)
11. What should the DbContext class be named? (default: [ProjectName]DbContext — e.g. CometDbContext)
12. Where should the DAB config file be placed? (default: at the solution root or a dedicated `dab/` folder)
13. What REST base route should DAB use? (default: /api)
14. What GraphQL endpoint path should DAB use? (default: /graphql)
15. Where should EF Core migrations be stored? (default: in the API project under `Data/Migrations/`)

---

Derived automatically:
- `{{PROJECT_SLUG}}` from Q2
- `{{DAB_CONFIG_PATH}}` from Q12
- `{{DB_CONTEXT_PROJECT_PATH}}` from Q4
- `{{MIGRATIONS_FOLDER}}` from Q15
