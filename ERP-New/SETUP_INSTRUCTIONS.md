# Setup Instructions for ERP-New Project

## ✅ Step 1: Close Current Project & Open New Folder

1. **Close Cursor/VS Code** (to free up memory)
2. **Open the new folder**: `D:\Projects\ERP-New`
3. **Verify structure** is created correctly

## ✅ Step 2: Verify Backend Files Copied

Check these files exist:
- `src/server/Api/Program.cs`
- `src/server/Infrastructure/DependencyInjection.cs`
- `src/server/Api/appsettings.json`

## ✅ Step 3: Implement Scalability Enhancements

I'll implement these when you open the new folder:
1. Database Connection Pooling
2. Response Compression
3. Health Checks
4. API Rate Limiting (next step)

## ✅ Step 4: Install Required NuGet Packages

Run these commands in `src/server/Api`:
```bash
dotnet add package AspNetCore.HealthChecks.Npgsql
dotnet add package AspNetCore.HealthChecks.UI
dotnet add package AspNetCoreRateLimit
```

## ✅ Step 5: Test Backend

```bash
cd src/server/Api
dotnet restore
dotnet build
dotnet run
```

## Next Steps After Backend is Ready

1. Create Preskool shared library
2. Create Admissions app
3. Create ERP app
4. Integrate everything

---

**Ready to proceed?** Open `D:\Projects\ERP-New` in Cursor and let me know!

