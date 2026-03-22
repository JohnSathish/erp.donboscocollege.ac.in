using System.Net;
using ERP.Api.IntegrationTests.Infrastructure;
using ERP.Domain.Admissions.Entities;
using FluentAssertions;

namespace ERP.Api.IntegrationTests.Admissions;

public class ApplicantExportEndpointsTests(AdmissionsApiFactory factory) : IClassFixture<AdmissionsApiFactory>
{
    private readonly AdmissionsApiFactory _factory = factory;

    [Fact]
    public async Task ExportApplicants_AsCsv_ReturnsFile()
    {
        await SeedSampleApplicantsAsync();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("api/admissions/applicants/export?format=csv");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/csv");
        response.Content.Headers.ContentDisposition?.FileName.Should().EndWith(".csv");
        var content = await response.Content.ReadAsByteArrayAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ExportApplicants_AsExcel_ReturnsFile()
    {
        await SeedSampleApplicantsAsync();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("api/admissions/applicants/export?format=excel");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        response.Content.Headers.ContentDisposition?.FileName.Should().EndWith(".xlsx");
        var content = await response.Content.ReadAsByteArrayAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ExportApplicants_AsPdf_ReturnsFile()
    {
        await SeedSampleApplicantsAsync();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("api/admissions/applicants/export?format=pdf");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/pdf");
        response.Content.Headers.ContentDisposition?.FileName.Should().EndWith(".pdf");
        var content = await response.Content.ReadAsByteArrayAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ExportApplicants_InvalidFormat_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("api/admissions/applicants/export?format=xml");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task SeedSampleApplicantsAsync()
    {
        var applicants = new[]
        {
            new Applicant("APP-1001", "Alice", "Johnson", "alice@example.com", new DateOnly(2004, 5, 21), "BSC-CS", "9000000001"),
            new Applicant("APP-1002", "Ben", "Lee", "ben@example.com", new DateOnly(2003, 11, 3), "BSC-MATH", "9000000002")
        };

        await _factory.SeedApplicantsAsync(applicants);
    }
}

