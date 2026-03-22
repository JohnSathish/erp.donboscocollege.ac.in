using System.Net;
using System.Net.Http.Json;
using ERP.Api.IntegrationTests.Infrastructure;
using ERP.Domain.Admissions.Entities;
using FluentAssertions;

namespace ERP.Api.IntegrationTests.Admissions;

public class ApplicantStatusEndpointsTests(AdmissionsApiFactory factory) : IClassFixture<AdmissionsApiFactory>
{
    private readonly AdmissionsApiFactory _factory = factory;

    [Fact]
    public async Task UpdateApplicantStatus_ToEntranceExam_ReturnsUpdatedApplicant()
    {
        var applicant = new Applicant(
            "APP-2001",
            "Clara",
            "Fernandes",
            "clara@example.com",
            new DateOnly(2004, 2, 14),
            "BSC-PHY",
            "9000000003");

        await _factory.SeedApplicantsAsync(new[] { applicant });

        var client = _factory.CreateClient();
        var payload = new
        {
            status = "EntranceExam",
            notifyApplicant = false,
            remarks = "Please bring original documents.",
            entranceExam = new
            {
                scheduledOnUtc = DateTime.UtcNow.AddDays(5),
                venue = "Main Auditorium",
                instructions = "Report 30 minutes early."
            }
        };

        var response = await client.PostAsJsonAsync(
            $"api/admissions/applicants/{applicant.Id}/status",
            payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<ApplicantResponse>();
        body.Should().NotBeNull();
        body!.Status.Should().Be("EntranceExam");
        body.EntranceExamVenue.Should().Be("Main Auditorium");
        body.EntranceExamInstructions.Should().Contain("Report");
    }

    [Fact]
    public async Task UpdateApplicantStatus_InvalidTransition_ReturnsBadRequest()
    {
        var applicant = new Applicant(
            "APP-2002",
            "Diego",
            "Fernandez",
            "diego@example.com",
            new DateOnly(2003, 9, 9),
            "BSC-CHEM",
            "9000000004");

        await _factory.SeedApplicantsAsync(new[] { applicant });

        var client = _factory.CreateClient();
        var payload = new
        {
            status = "Submitted",
            notifyApplicant = false,
            remarks = "No-op"
        };

        var response = await client.PostAsJsonAsync(
            $"api/admissions/applicants/{applicant.Id}/status",
            payload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateApplicantStatus_EntranceExamWithoutDetails_ReturnsBadRequest()
    {
        var applicant = new Applicant(
            "APP-2003",
            "Eva",
            "Marak",
            "eva@example.com",
            new DateOnly(2004, 7, 30),
            "BA-ENG",
            "9000000005");

        await _factory.SeedApplicantsAsync(new[] { applicant });

        var client = _factory.CreateClient();
        var payload = new
        {
            status = "EntranceExam",
            notifyApplicant = true
        };

        var response = await client.PostAsJsonAsync(
            $"api/admissions/applicants/{applicant.Id}/status",
            payload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private sealed record ApplicantResponse(
        Guid Id,
        string ApplicationNumber,
        string FullName,
        string Email,
        string MobileNumber,
        string ProgramCode,
        string Status,
        DateTime StatusUpdatedOnUtc,
        string? StatusUpdatedBy,
        string? StatusRemarks,
        DateTime? EntranceExamScheduledOnUtc,
        string? EntranceExamVenue,
        string? EntranceExamInstructions);
}






