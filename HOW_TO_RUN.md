# How to Run the ERP Admissions Portal Project

This project consists of two parts:
1. **Backend**: .NET 8.0 Web API (runs in Visual Studio)
2. **Frontend**: Angular application (runs separately using NX)

## Prerequisites

### Required Software:
- **Visual Studio 2022** (with .NET 8.0 SDK)
- **PostgreSQL** (database server)
- **Node.js** (v18 or higher)
- **pnpm** (package manager for frontend)

### Verify Prerequisites:
```powershell
# Check .NET SDK version
dotnet --version  # Should be 8.0.x

# Check Node.js version
node --version  # Should be 18.x or higher

# Check pnpm installation
pnpm --version
```

If pnpm is not installed:
```powershell
npm install -g pnpm
```

---

## Step 1: Database Setup

1. **Start PostgreSQL** service
2. **Create database** (if not exists):
   ```sql
   CREATE DATABASE erp_dev;
   ```
3. **Update connection string** in `src/server/Api/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=erp_dev;Username=postgres;Password=YOUR_PASSWORD"
   }
   ```

---

## Step 2: Run Backend API in Visual Studio

### Option A: Using Visual Studio GUI

1. **Open Solution**:
   - Open `ERP.sln` in Visual Studio 2022

2. **Set Startup Project**:
   - Right-click on `ERP.Api` project (in `src/server/Api/`)
   - Select **"Set as Startup Project"**

3. **Choose Launch Profile**:
   - In the toolbar, select one of these profiles:
     - **`https`** - Runs on `https://localhost:7237` (recommended)
     - **`http`** - Runs on `http://localhost:5227`
     - **`IIS Express`** - Runs on IIS Express

4. **Run the Project**:
   - Press **F5** (Debug) or **Ctrl+F5** (Run without debugging)
   - The API will start and Swagger UI will open automatically at `https://localhost:7237/swagger`

### Option B: Using Command Line (Alternative)

```powershell
cd src/server/Api
dotnet run
```

---

## Step 3: Run Frontend Angular Application

The **Node/Nx project is under `src/client`**, not the repo root. Running `npm install` in `E:\Projects\ERP` without a root `package.json` fails with `ENOENT` — use one of the options below.

### First Time Setup:

**Option A — from repo root (npm workspaces):**

```powershell
cd E:\Projects\ERP
npm install
```

This uses the root `package.json` **workspaces** entry so dependencies install for `src/client`.

**Option B — classic (recommended in docs):**

1. **Navigate to client directory**:
   ```powershell
   cd src/client
   ```

2. **Install dependencies**:
   ```powershell
   pnpm install
   ```

### Run Frontend:

```powershell
# From src/client directory
pnpm nx serve applicant-portal
```

The frontend will start on **`http://localhost:4200`**

---

## Step 4: Access the Application

- **Frontend (Angular)**: http://localhost:4200
- **Backend API Swagger**: https://localhost:7237/swagger
- **Backend API (HTTP)**: http://localhost:5227

### Admin admission workflow

After logging in as admin, open **Online Admission → Admission workflow** (`/admin/admissions/workflow`) for the documented 7-stage process (intake → documents → merit → decision → fees → enrollment → reporting). The **Applications** list supports server filters (status, shift/stream, payment) and optional refiners for the current page (workflow stage, category, major).

---

## Running Both Projects Simultaneously

### Option 1: Two Separate Terminals

**Terminal 1** (Backend - Visual Studio):
- Press **F5** in Visual Studio to run `ERP.Api`

**Terminal 2** (Frontend):
```powershell
cd src/client
pnpm nx serve applicant-portal
```

### Option 2: Visual Studio + External Terminal

1. Run backend in Visual Studio (F5)
2. Open PowerShell/Command Prompt separately
3. Run frontend command:
   ```powershell
   cd D:\Projects\ERP\src\client
   pnpm nx serve applicant-portal
   ```

---

## Troubleshooting

### Backend Issues:

1. **Port Already in Use**:
   - Change port in `src/server/Api/Properties/launchSettings.json`
   - Or stop the process using the port

2. **Database Connection Error**:
   - Verify PostgreSQL is running
   - Check connection string in `appsettings.json`
   - Ensure database `erp_dev` exists

3. **Build Errors**:
   - Right-click solution → **Restore NuGet Packages**
   - Build → **Rebuild Solution**

### Frontend Issues:

1. **Port 4200 Already in Use**:
   ```powershell
   pnpm nx serve applicant-portal --port 4201
   ```

2. **Node Modules Missing**:
   ```powershell
   cd src/client
   pnpm install
   ```

3. **NX Not Found**:
   ```powershell
   npm install -g nx
   # Or use pnpm
   pnpm add -g nx
   ```

### Browser DevTools console (Razorpay / third‑party noise)

While testing **admission fee payment** with Razorpay, the console often shows messages you **cannot remove from your own app code**. Typical cases:

| Message | Cause |
|--------|--------|
| **CORS** / blocked image from `localhost` while the page origin is `https://api.razorpay.com` | Razorpay’s checkout runs in an **HTTPS** iframe. If something (e.g. a **merchant logo** in the Razorpay Dashboard, or a redirect URL) points to **`http://localhost:7070`** or another HTTP URL, the browser blocks it (mixed content + cross‑origin rules). |
| **Mixed content** (HTTPS page loading HTTP) | Same as above: use **HTTPS** for dev URLs in the dashboard, or remove the custom logo URL for local testing. |
| **`Refused to get unsafe header`** (e.g. `x-rtb-fingerprint-id`) | Razorpay’s scripts; **harmless**. |
| **Permissions policy** (`accelerometer`, `deviceorientation`) | Razorpay or the browser; **harmless** for payment flow. |
| **`runtime.lastError` / “Receiving end does not exist”** | Often a **browser extension**; try an incognito window with extensions disabled. |
| **`[webpack-dev-server]` … file is unused** | Reduced by tightening `apps/tsconfig.app.json` excludes; if any remain, they refer to files not on any import path. |

**What to do:** For logo/CORS to `localhost`, open the [Razorpay Dashboard](https://dashboard.razorpay.com/) → your account → **branding / logo** and clear the URL or use a **public HTTPS** image, or ignore these lines during local dev.

---

## Project Structure

```
ERP/
├── ERP.sln                    # Visual Studio Solution
├── src/
│   ├── server/               # Backend (.NET)
│   │   ├── Api/              # Main API Project (Startup)
│   │   ├── Application/      # Application Layer
│   │   ├── Domain/           # Domain Layer
│   │   └── Infrastructure/    # Infrastructure Layer
│   └── client/               # Frontend (Angular/NX)
│       ├── apps/             # Angular Application
│       └── shared/           # Shared Libraries
└── tests/                     # Integration Tests
```

---

## Quick Start Commands Summary

```powershell
# 1. Start PostgreSQL (if not running as service)

# 2. Run Backend (Visual Studio)
# Open ERP.sln → Set ERP.Api as startup → Press F5

# 3. Run Frontend (Terminal)
cd src/client
pnpm install  # First time only
pnpm nx serve applicant-portal
```

---

## Development Tips

1. **Hot Reload**: Both frontend and backend support hot reload
   - Backend: Changes auto-reload (if using `dotnet watch run`)
   - Frontend: Angular dev server auto-reloads on file changes

2. **API Testing**: Use Swagger UI at `https://localhost:7237/swagger`

3. **Database Migrations**: Apply migrations if needed:
   ```powershell
   cd src/server/Infrastructure
   dotnet ef database update --project ../Api/ERP.Api.csproj
   ```

4. **Environment Variables**: Backend uses `appsettings.json` and `appsettings.Development.json`

---

## Notes

- The backend API runs on port **5227** (HTTP) or **7237** (HTTPS)
- The frontend runs on port **4200**
- CORS is configured to allow `http://localhost:4200` and `http://localhost:4201`
- Make sure both services are running for the application to work properly




















