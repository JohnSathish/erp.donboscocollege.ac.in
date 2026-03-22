# Dev Tooling, Environments & CI/CD

## Local Development
- **Prerequisites:** .NET 8 SDK, Node.js 20 LTS, PNPM, Docker Desktop, Azure CLI (optional), Git.
- **Repository structure:**
  - `src/server`: ASP.NET Core solution using `dotnet` CLI, packaged via `Directory.Build.*` for shared settings.
  - `src/client`: Angular workspace managed via `nx` and `pnpm`.
  - `docker-compose.yml`: orchestrates Postgres, Redis, Seq (logging), Mailhog (email testing), IdentityServer, API, and Angular dev containers.
- **Developer workflow:**
  - `docker compose up db redis seq mailhog` to start infrastructure services.
  - `dotnet watch --project src/server/Api` for hot reload API.
  - `pnpm nx serve portal` for Angular client.
  - Use `Makefile`/PowerShell `build.ps1` to wrap common tasks (build, test, lint).
- **Secrets management:** `.env.development` loaded via `dotnet user-secrets` for API and `.env` for Angular; no secrets committed.

## Shared Environments
| Environment | Purpose | Hosting | Data |
|-------------|---------|---------|------|
| Dev | QA & feature validation for integrated modules | Azure App Service (single instance) + Azure PostgreSQL Flexible Server (non-zone redundant) | Frequent refresh from sanitized production snapshot |
| QA | UAT with stable builds | Azure App Service (scaled-out) + Azure PostgreSQL | Controlled data set seeded via migrations + scripts |
| Production | Live system | Azure App Service (Autoscale) + Azure PostgreSQL Hyperscale (or Flexible) | Live data with HA and backups |

Additional services per environment: Azure Redis Cache, Azure Storage (blob/file) for documents, Azure Application Insights for telemetry, Azure Key Vault for secrets.

## CI/CD Pipeline
- **Tooling:** GitHub Actions (default) with self-hosted runners optional; Azure DevOps pipeline template as alternative.
- **Workflow:**
  1. Trigger on pull requests and main branch commits.
  2. Jobs:
     - **Build & Test Backend:** `dotnet restore/build/test`, run analyzers, publish artifacts.
     - **Build & Test Frontend:** `pnpm install`, `pnpm lint`, `pnpm test`, `pnpm nx build portal`.
     - **Infrastructure Validation:** `terraform fmt/validate` or `bicep build` for IaC templates.
  3. On main merge, create docker images (`erp-api`, `erp-identity`, `erp-portal`), push to Azure Container Registry.
  4. Deploy using GitHub Actions environment approvals:
     - Dev: automatic deployment via `az webapp deploy` or `az containerapp update`.
     - QA/Prod: gated approvals with manual checks.
  5. Run database migrations (`dotnet ef database update`) during deploy or via migration job.
- **Observability:** pipeline publishes test coverage, lint reports, and security scan summaries.

## Infrastructure as Code
- `infra/azure/` folder containing:
  - `bicep/` modules for App Service, PostgreSQL, Redis, Storage, Identity.
  - `terraform/` optional alternative with reusable modules.
- Parameter files per environment.
- CI step enforces formatting and drift detection (e.g., `terraform plan`, `what-if`).

## Security & Compliance Guardrails
- Container image scanning using Trivy or Microsoft Defender for DevOps.
- Dependency scanning via GitHub Dependabot + `dotnet list package --vulnerable` and `pnpm audit`.
- Secrets scanning enforced (GitHub secret scanning, Gitleaks pre-commit hook).
- Role-based access to pipelines and environment secrets.

## Backup & Disaster Recovery
- Automated PostgreSQL backups (point-in-time restore) with weekly validation.
- Storage account replication (ZRS) for documents.
- Runbook documented in `docs/runbooks/` for restoration procedures.




