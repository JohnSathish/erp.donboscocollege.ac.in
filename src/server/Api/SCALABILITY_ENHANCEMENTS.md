# Scalability Enhancements Implementation Guide

## Quick Implementation Checklist

### 1. Database Connection Pooling
**File**: `src/server/Infrastructure/DependencyInjection.cs`
```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
    }));
```

**Connection String** (appsettings.json):
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=...;Port=5432;Database=...;Pooling=true;MinPoolSize=10;MaxPoolSize=200;Connection Lifetime=300;Command Timeout=30"
}
```

### 2. Response Compression
**File**: `src/server/Api/Program.cs`
```csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

// In app configuration:
app.UseResponseCompression();
```

### 3. API Rate Limiting
**Package**: `AspNetCoreRateLimit`
```bash
dotnet add package AspNetCoreRateLimit
```

### 4. Health Checks
**File**: `src/server/Api/Program.cs`
```csharp
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString)
    .AddCheck<ApiHealthCheck>("api");

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
```

### 5. Request Queuing
**Package**: `Hangfire` or `Quartz.NET`
- Queue email sending
- Queue SMS sending
- Queue document processing

