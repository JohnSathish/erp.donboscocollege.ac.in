using ERP.Domain.Admissions.Entities;
using ERP.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace ERP.Api.IntegrationTests.Infrastructure;

public class AdmissionsApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private string? _databaseName;
    private string? _connectionString;

    public async Task InitializeAsync()
    {
        _databaseName = $"erp_integration_{Guid.NewGuid():N}";

        var baseBuilder = new NpgsqlConnectionStringBuilder(GetBaseConnectionString());
        var adminBuilder = new NpgsqlConnectionStringBuilder(baseBuilder.ConnectionString)
        {
            Database = "postgres"
        };

        _connectionString = new NpgsqlConnectionStringBuilder(baseBuilder.ConnectionString)
        {
            Database = _databaseName
        }.ConnectionString;

        await using var adminConnection = new NpgsqlConnection(adminBuilder.ConnectionString);
        await adminConnection.OpenAsync();

        await using var createDbCommand = adminConnection.CreateCommand();
        createDbCommand.CommandText = $"CREATE DATABASE \"{_databaseName}\"";
        await createDbCommand.ExecuteNonQueryAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        if (_databaseName is null)
        {
            return;
        }

        var baseBuilder = new NpgsqlConnectionStringBuilder(GetBaseConnectionString());
        baseBuilder.Database = "postgres";

        await using var adminConnection = new NpgsqlConnection(baseBuilder.ConnectionString);
        await adminConnection.OpenAsync();

        await using var terminateCommand = adminConnection.CreateCommand();
        terminateCommand.CommandText =
            $"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{_databaseName}'";
        await terminateCommand.ExecuteNonQueryAsync();

        await using var dropDbCommand = adminConnection.CreateCommand();
        dropDbCommand.CommandText = $"DROP DATABASE IF EXISTS \"{_databaseName}\"";
        await dropDbCommand.ExecuteNonQueryAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["Notifications:Email:Enabled"] = "false",
                ["Notifications:Sms:Enabled"] = "false",
                ["Security:ApplicantLoginRateLimit:MaxAttempts"] = "3",
                ["Security:ApplicantLoginRateLimit:AttemptWindow"] = "00:00:10",
                ["Security:ApplicantLoginRateLimit:LockoutDuration"] = "00:00:30"
            };

            if (_connectionString is not null)
            {
                settings["ConnectionStrings:DefaultConnection"] = _connectionString;
            }

            configBuilder.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.RemoveAll(typeof(ApplicationDbContext));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_connectionString ?? GetBaseConnectionString()));

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        });
    }

    public async Task SeedApplicantsAsync(IEnumerable<Applicant> applicants)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Applicants.RemoveRange(context.Applicants);
        await context.SaveChangesAsync();

        await context.Applicants.AddRangeAsync(applicants);
        await context.SaveChangesAsync();
    }

    public async Task<StudentApplicantAccount> CreateApplicantAccountAsync(
        string uniqueId,
        string fullName,
        string email,
        string mobileNumber,
        string shift,
        string password,
        bool mustChangePassword = true)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<StudentApplicantAccount>>();

        var account = new StudentApplicantAccount(
            uniqueId,
            fullName,
            new DateOnly(2004, 1, 1),
            "Female",
            email,
            mobileNumber,
            shift);

        var hash = hasher.HashPassword(account, password);
        account.SetPasswordHash(hash);

        if (!mustChangePassword)
        {
            account.MarkPasswordChanged();
        }

        context.StudentApplicantAccounts.Add(account);
        await context.SaveChangesAsync();

        return account;
    }

    private static string GetBaseConnectionString()
    {
        return Environment.GetEnvironmentVariable("ERP_DefaultConnection")
               ?? "Host=localhost;Port=5432;Database=erp_dev;Username=postgres;Password=john@1991js";
    }
}

