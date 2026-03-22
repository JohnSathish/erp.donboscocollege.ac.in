# Applicant Portal Route Guarding & E2E Test Plan

## Goals

1. **Protect applicant-only routes.** Ensure every path under `/app/...` requires a valid JWT and refresh-token workflow.
2. **Provide deterministic E2E flows.** Automate login, refresh, lockout, password change, and logout scenarios with repeatable fixtures.
3. **Verify UX regressions early.** Include smoke checks for dashboard/profile/documents pages and the toast-driven password change feedback.

## Route Guarding Strategy

| Concern | Approach | Notes |
| --- | --- | --- |
| Initial navigation to `/app/...` | Reuse `authGuard` but extend it to check token freshness and optionally trigger silent refresh. | Use `AuthService.profile` and `refreshToken(...)`. Guard should redirect to `/login` if refresh fails. |
| Browser refresh in protected route | Guard should repopulate profile from `localStorage` (already supported) and blocking navigation until refresh completes. | Add `AuthService.restoreSession()` helper invoked from guard. |
| Background token expiry while user active | `AuthInterceptor` already refreshes on 401; add handling for refresh failure → `AuthService.logout()` with toast. | Toast service already available. |
| Unauthorized API responses | Keep interceptor logic; guard should avoid additional network calls if interceptor already handling. |

### Guard Enhancements (Backlog)

- Move guard to new `auth.guard.ts` helper function that calls `AuthService.ensureAuthenticated()` returning an `Observable<boolean>` (wrapping refresh logic).
- Add unit tests for the guard (Jest) covering:
  - Stored profile available → allow navigation.
  - Missing profile but refresh succeeds → allow navigation.
  - Refresh fails → logout + redirect to `/login`.
- Wire guard to every child route under `/app` (already applied via router config).

## E2E Test Plan (Nx / Cypress)

| Test Case | Description | Setup |
| --- | --- | --- |
| `applicant-login-success` | Valid email/password logs in, sees dashboard summary and toast-free state. | Seed account via API factory endpoint `/testing/create-applicant` (use existing integration factory as reference). |
| `applicant-login-lockout` | Enter wrong password > rate limit threshold, ensure error message and final block. | Use backend rate limiter settings (5 attempts). |
| `applicant-refresh-token` | Start session, force token expiration via intercepted API to return 401, ensure interceptor refreshes and request succeeds. | Mock API or adjust JWT expiration via configuration for test run. |
| `password-change` | Use change password modal, expect toast success, forced logout on new login with updated credentials. | Use UI flow plus verifying new toast message. |
| `protected-route-redirect` | Navigate directly to `/app/dashboard` without session -> redirected to `/login`. | Clear storage before test. |
| `logout-clears-session` | Click `Sign Out` and verify tokens removed, redirect to login, and direct `/app/...` again gets blocked. | Use `localStorage` assertions. |
| `document-checklist-display` | After login assert placeholder checklist renders and summary placeholders present (guards against layout regressions). | Verify DOM text and counts. |

### Tooling Notes

- Use **Nx Cypress** (`pnpm nx g @nx/angular:component` / `@nx/cypress:component`?) – choose `@nx/angular:app` e2e or upgrade existing `apps-e2e` project.
- Configure base URL (`http://localhost:4200`) via `cypress.config.ts`.
- Add custom Cypress commands for:
  - `cy.loginApplicant({ email, password })` to hit `/api/auth/applicants/login` directly and store tokens.
  - `cy.createApplicantFixture()` hitting a dedicated test endpoint (optional) or reusing integration factory script.
- Use environment variables for API base URL and credentials.

### CI Considerations

- Add e2e target to GitHub Actions (or future pipeline): `pnpm nx e2e applicant-portal-e2e --watch=false`.
- Ensure Postgres & API are running in CI (docker-compose or Testcontainers) before launching Cypress.
- Keep tests independent: reset DB or use unique account IDs per run.

## Next Implementation Steps

1. Implement `AuthService.ensureAuthenticated()` and extend `authGuard`.
2. Add guard unit tests.
3. Scaffold Nx Cypress project (if not already) under `apps/applicant-portal-e2e`.
4. Seed-runner utilities for creating test accounts.
5. Implement Cypress specs per table above; run locally and wire into CI.

This plan keeps frontend guarded and supplies a roadmap for reliable end-to-end coverage once the Cypress suite is in place.







