# Azure Pipelines Quick Reference

## File Location

`azure-pipelines.yml` at repo root (or configured path in Azure DevOps).

## Top-Level Structure

```yaml
trigger:
  branches:
    include: [main, develop]
  tags:
    include: ['v*']

pr:
  branches:
    include: [main]

pool:
  vmImage: 'windows-latest'   # or ubuntu-latest

variables:
  BuildConfiguration: 'Release'
  DOTNET_SKIP_FIRST_TIME_EXPERIENCE: true

stages:
  - stage: Build
    jobs:
      - job: BuildAndTest
        steps: []
  - stage: Deploy
    dependsOn: Build
    jobs:
      - deployment: DeployDev
        environment: dev
        strategy:
          runOnce:
            deploy:
              steps: []
```

## Common Tasks

| Task | Purpose |
|---|---|
| `UseDotNet@2` | Install .NET SDK |
| `NodeTool@0` | Install Node.js |
| `DotNetCoreCLI@2` | dotnet restore / build / test / publish |
| `Npm@1` | npm install / run |
| `Docker@2` | Build and push Docker images |
| `AzureWebApp@1` | Deploy to Azure App Service |
| `AzureContainerApps@1` | Deploy to Azure Container Apps |
| `PublishTestResults@2` | Publish NUnit test results |
| `PublishCodeCoverageResults@2` | Publish coverage reports |

## Secret and Variable References

```yaml
# Pipeline variable (non-secret)
- script: echo $(MY_VARIABLE)

# Secret (mark as secret in pipeline variables UI)
- script: echo $(MY_SECRET)
  env:
    MY_SECRET: $(MY_SECRET)    # must be mapped to env for script steps

# Variable group (linked in pipeline settings)
variables:
  - group: my-variable-group
  - name: LocalVar
    value: 'local-value'
```

## Environments and Approvals

```yaml
- deployment: DeployProd
  environment: production     # create in Azure DevOps Environments; add approval checks there
  strategy:
    runOnce:
      deploy:
        steps:
          - script: echo "Deploying to prod"
```

## Stage Dependencies

```yaml
stages:
  - stage: Build
  - stage: DeployDev
    dependsOn: Build
    condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/develop'))
  - stage: DeployProd
    dependsOn: Build
    condition: and(succeeded(), startsWith(variables['Build.SourceBranch'], 'refs/tags/v'))
```

## NUnit Test Results

```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: test
    projects: '**/*.Tests.csproj'
    arguments: '--configuration $(BuildConfiguration) --logger trx --results-directory $(Agent.TempDirectory)'

- task: PublishTestResults@2
  inputs:
    testResultsFormat: VSTest
    testResultsFiles: '$(Agent.TempDirectory)/**/*.trx'
```

## Caching

```yaml
- task: Cache@2
  inputs:
    key: 'nuget | "$(Agent.OS)" | **/Directory.Packages.props'
    restoreKeys: |
      nuget | "$(Agent.OS)"
    path: $(NUGET_PACKAGES)
  displayName: Cache NuGet packages
```
