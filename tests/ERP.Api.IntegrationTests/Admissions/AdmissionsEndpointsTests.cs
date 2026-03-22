using System.Net;
using System.Net.Http.Json;
using ERP.Api.IntegrationTests.Infrastructure;
using ERP.Application.Admissions.ViewModels;
using ERP.Domain.Admissions.Entities;
using FluentAssertions;

namespace ERP.Api.IntegrationTests.Admissions;

public class AdmissionsEndpointsTests(AdmissionsApiFactory factory) : IClassFixture<AdmissionsApiFactory>
{
    private readonly AdmissionsApiFactory _factory = factory;

    [Fact]
    public async Task ListApplicants_ReturnsSeededApplicants()
    {
        // Arrange
        var applicants = new[]
        {
            new Applicant("APP-2001", "Isha", "Patel", "isha.patel@example.com", new DateOnly(2004, 3, 12), "CS101", "9100000001"),
            new Applicant("APP-2002", "Miguel", "Santos", "miguel.santos@example.com", new DateOnly(2003, 7, 6), "EE201", "9100000002")
        };

        await _factory.SeedApplicantsAsync(applicants);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/admissions/applicants");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<ApplicantDto>>();
        payload.Should().NotBeNull();
        payload.Should().HaveCount(2);

        payload!.Select(x => x.Id).Should().BeEquivalentTo(applicants.Select(a => a.Id));
    }

    [Fact]
    public async Task GetApplicantById_ReturnsExpectedApplicant()
    {
        // Arrange
        var applicant = new Applicant(
            "APP-3001",
            "Sara",
            "Nguyen",
            "sara.nguyen@example.com",
            new DateOnly(2004, 9, 30),
            "ME301",
            "9100000003");

        await _factory.SeedApplicantsAsync(new[] { applicant });

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/admissions/applicants/{applicant.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<ApplicantDto>();
        payload.Should().NotBeNull();
        payload!.Id.Should().Be(applicant.Id);
        payload.FullName.Should().Be($"{applicant.FirstName} {applicant.LastName}");
        payload.Email.Should().Be(applicant.Email);
        payload.MobileNumber.Should().Be(applicant.MobileNumber);
        payload.ProgramCode.Should().Be(applicant.ProgramCode);
    }
}

