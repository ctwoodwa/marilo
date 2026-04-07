# Pipeline Patterns

Reusable job and step patterns for .NET solutions with npm builds, Docker, and Aspire.

## .NET Build + Test (GitHub Actions)

```yaml
jobs:
  build-and-test:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup Node
        uses: actions/setup-node@v4
        with:
          node-version: '{{NODE_VERSION}}'

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '{{DOTNET_VERSION}}'

      # npm must run before dotnet build for projects with SCSS compilation
      - name: npm install
        run: npm install
        working-directory: src/{{WEB_PROJECT_FOLDER}}

      - name: npm build styles
        run: npm run build:styles
        working-directory: src/{{WEB_PROJECT_FOLDER}}

      - name: dotnet restore
        run: dotnet restore src/{{SOLUTION_FILE}}

      - name: dotnet build
        run: dotnet build src/{{SOLUTION_FILE}} --no-restore --configuration Release

      - name: dotnet test
        run: >
          dotnet test src/{{SOLUTION_FILE}}
          --no-build
          --configuration Release
          --collect:"XPlat Code Coverage"
          --results-directory ./coverage

      - name: Upload coverage
        uses: actions/upload-artifact@v4
        with:
          name: coverage
          path: ./coverage
```

## .NET Build + Test (Azure Pipelines)

```yaml
steps:
  - task: NodeTool@0
    inputs:
      versionSpec: '{{NODE_VERSION}}'

  - task: UseDotNet@2
    inputs:
      version: '{{DOTNET_VERSION}}'

  - script: npm install && npm run build:styles
    displayName: 'npm build'
    workingDirectory: src/{{WEB_PROJECT_FOLDER}}

  - task: DotNetCoreCLI@2
    displayName: 'dotnet build'
    inputs:
      command: build
      projects: 'src/{{SOLUTION_FILE}}'
      arguments: '--configuration Release'

  - task: DotNetCoreCLI@2
    displayName: 'dotnet test'
    inputs:
      command: test
      projects: 'src/{{SOLUTION_FILE}}'
      arguments: '--configuration Release --collect:"XPlat Code Coverage"'
```

## Docker Build and Push (GitHub Actions)

```yaml
- name: Build Docker image
  run: |
    docker build \
      -f src/{{PROJECT_FOLDER}}/Dockerfile \
      -t ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}:${{ github.sha }} \
      src/

- name: Push Docker image
  run: docker push ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}:${{ github.sha }}
```

## Deploy to Azure Container Apps (GitHub Actions)

```yaml
- name: Deploy to Container Apps
  uses: azure/container-apps-deploy-action@v1
  with:
    resourceGroup: ${{ vars.RESOURCE_GROUP }}
    containerAppName: ${{ vars.CONTAINER_APP_NAME }}
    imageToDeploy: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}:${{ github.sha }}
```

## Aspire Manifest Export

```yaml
- name: Export Aspire manifest
  run: >
    dotnet run
    --project src/{{APPHOST_PROJECT}}
    --
    --publisher manifest
    --output-path ./aspire-manifest.json
```

## Secret Reference Syntax

| Platform | Syntax |
|---|---|
| GitHub Actions | `${{ secrets.SECRET_NAME }}` |
| GitHub Actions (vars) | `${{ vars.VAR_NAME }}` |
| Azure Pipelines | `$(SECRET_NAME)` |
| Azure Pipelines (var group) | `$(SECRET_NAME)` (from linked variable group) |

## Coverage Gate Step

```yaml
# GitHub Actions
- name: Enforce coverage threshold
  run: >
    dotnet tool run reportgenerator
    -reports:"./coverage/**/coverage.cobertura.xml"
    -targetdir:"./coverage/report"
    -reporttypes:Html
    && dotnet test src/{{SOLUTION_FILE}}
    --collect:"XPlat Code Coverage"
    -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Threshold=80
```

## Job Dependency Pattern (GitHub Actions)

```yaml
jobs:
  build-and-test:
    runs-on: windows-latest
    # ...

  deploy-dev:
    needs: build-and-test     # ← gates deployment on test success
    if: github.ref == 'refs/heads/develop'
    runs-on: windows-latest
    environment: dev
    # ...

  deploy-prod:
    needs: build-and-test
    if: startsWith(github.ref, 'refs/tags/v')
    runs-on: windows-latest
    environment: production   # ← requires manual approval in GitHub environment settings
    # ...
```
