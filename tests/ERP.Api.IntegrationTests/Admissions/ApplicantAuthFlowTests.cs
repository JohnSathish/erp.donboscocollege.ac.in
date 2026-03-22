using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ERP.Api.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace ERP.Api.IntegrationTests.Admissions;

public class ApplicantAuthFlowTests(AdmissionsApiFactory factory) : IClassFixture<AdmissionsApiFactory>
{
    private readonly AdmissionsApiFactory _factory = factory;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task Login_ReturnsTokens_AndRequiresPasswordChange()
    {
        var password = "Temp@123!";
        var account = await _factory.CreateApplicantAccountAsync(
            "APP-5001",
            "Login Test",
            "login.test@example.com",
            "9000000001",
            "SHIFT - II",
            password,
            mustChangePassword: true);

        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("api/auth/applicants/login", new LoginRequest(account.Email, password));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions);
        payload.Should().NotBeNull();
        payload!.Token.Should().NotBeNullOrWhiteSpace();
        payload.RefreshToken.Should().NotBeNullOrWhiteSpace();
        payload.MustChangePassword.Should().BeTrue();
        payload.UniqueId.Should().Be(account.UniqueId);
    }

    [Fact]
    public async Task RefreshToken_ReturnsRotatedTokens()
    {
        var password = "Temp@456!";
        var account = await _factory.CreateApplicantAccountAsync(
            "APP-5002",
            "Refresh Test",
            "refresh.test@example.com",
            "9000000002",
            "SHIFT - II",
            password,
            mustChangePassword: false);

        var client = _factory.CreateClient();

        var login = await LoginAsync(client, account.Email, password);

        var refreshResponse = await client.PostAsJsonAsync("api/auth/applicants/refresh", new RefreshRequest(login.RefreshToken));
        refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await refreshResponse.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions);
        payload.Should().NotBeNull();
        payload!.Token.Should().NotBe(login.Token);
        payload.RefreshToken.Should().NotBe(login.RefreshToken);
        payload.MustChangePassword.Should().BeFalse();
    }

    [Fact]
    public async Task ChangePassword_ClearsMustChangeFlag_AndRotatesTokens()
    {
        var initialPassword = "Temp@789!";
        var newPassword = "NewPass@123!";

        var account = await _factory.CreateApplicantAccountAsync(
            "APP-5003",
            "Password Change Test",
            "changepass.test@example.com",
            "9000000003",
            "SHIFT - II",
            initialPassword,
            mustChangePassword: true);

        var client = _factory.CreateClient();
        var login = await LoginAsync(client, account.Email, initialPassword);

        var changeRequest = new ChangePasswordRequest(initialPassword, newPassword);
        using var message = new HttpRequestMessage(HttpMethod.Post, "api/auth/applicants/change-password")
        {
            Content = JsonContent.Create(changeRequest)
        };
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", login.Token);

        var changeResponse = await client.SendAsync(message);
        changeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await changeResponse.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions);
        payload.Should().NotBeNull();
        payload!.MustChangePassword.Should().BeFalse();

        // Old password should no longer work
        var oldPasswordResponse = await client.PostAsJsonAsync("api/auth/applicants/login", new LoginRequest(account.Email, initialPassword));
        oldPasswordResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        // New password should work
        var newLogin = await LoginAsync(client, account.Email, newPassword);
        newLogin.MustChangePassword.Should().BeFalse();
    }

    [Fact]
    public async Task Login_LocksOut_AfterMaxFailedAttempts()
    {
        var password = "Temp@321!";
        var account = await _factory.CreateApplicantAccountAsync(
            "APP-5004",
            "Lockout Test",
            "lockout.test@example.com",
            "9000000004",
            "SHIFT - II",
            password,
            mustChangePassword: false);

        var client = _factory.CreateClient();

        for (var i = 0; i < 3; i++)
        {
            var response = await client.PostAsJsonAsync("api/auth/applicants/login", new LoginRequest(account.Email, "WrongPassword!"));
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        var lockedResponse = await client.PostAsJsonAsync("api/auth/applicants/login", new LoginRequest(account.Email, password));
        lockedResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static async Task<LoginResponse> LoginAsync(HttpClient client, string username, string password)
    {
        var response = await client.PostAsJsonAsync("api/auth/applicants/login", new LoginRequest(username, password));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions))!;
    }

    private sealed record LoginRequest(string Username, string Password);

    private sealed record RefreshRequest(string RefreshToken);

    private sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);

    private sealed record LoginResponse(
        string Token,
        DateTime ExpiresAtUtc,
        string RefreshToken,
        DateTime RefreshTokenExpiresAtUtc,
        string UniqueId,
        string Email,
        string FullName,
        bool MustChangePassword);
}

