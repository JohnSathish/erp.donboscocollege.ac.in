# Production deployment checklist

## Configuration (never commit real secrets)

- **Connection string:** set `ConnectionStrings__DefaultConnection` (or User Secrets / Key Vault) for PostgreSQL.
- **JWT:** set `Authentication__Jwt__Secret` to a long random string; configure Issuer/Audience if they differ from Development.
- **Cors:** `appsettings.Production.json` must list real applicant-portal origins. Startup **fails** if `Cors:AllowedOrigins` is missing or empty when `ASPNETCORE_ENVIRONMENT=Production`.
- **SMS / Email / Razorpay:** override credentials via environment variables or a secure store; avoid shipping live keys in source control.
- **HTTPS:** terminate TLS at the reverse proxy (IIS, nginx, Caddy) or use Kestrel + certificate.

## Database

- Run migrations: `dotnet ef database update` against the production database.
- Back up before major releases.

## Build & run

```bash
dotnet publish src/server/Api/ERP.Api.csproj -c Release -o ./publish
```

Set `ASPNETCORE_ENVIRONMENT=Production` and point the process at the published folder.

## Health

- Swagger is **disabled** in Production (see `Program.cs`).
- Root `/` redirects to `/swagger` **only in Development**.

## Smoke tests

- Admin login: `POST /api/auth/admin/login`
- Applicant subject catalog: `GET /api/admissions/class-xii-subjects?board=MBOSE&stream=ARTS`
- After configuring CORS, verify the browser portal can call the API (no CORS errors).
