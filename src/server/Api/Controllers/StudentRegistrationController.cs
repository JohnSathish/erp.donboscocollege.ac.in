using System.IO;
using ERP.Application.Admissions.Commands.RegisterStudentApplicant;
using ERP.Application.Common.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/registration/student")]
public class StudentRegistrationController(IMediator mediator, ILogger<StudentRegistrationController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<RegisterStudentApplicantResponse>> RegisterStudentApplicant(
        [FromForm] RegisterStudentApplicantRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ProfilePhoto is null || request.ProfilePhoto.Length == 0)
        {
            return BadRequest(new { message = "Profile photo is required." });
        }

        await using var stream = new MemoryStream();
        await request.ProfilePhoto.CopyToAsync(stream, cancellationToken);
        var profilePhoto = new UploadedFileDto(
            request.ProfilePhoto.FileName,
            request.ProfilePhoto.ContentType,
            stream.ToArray());

        var command = new RegisterStudentApplicantCommand(
            request.FullName,
            request.DateOfBirth,
            request.Gender,
            request.Email,
            request.MobileNumber,
            profilePhoto);

        try
        {
            var result = await mediator.Send(command, cancellationToken);
            var response = new RegisterStudentApplicantResponse(result.UniqueId, result.TemporaryPassword);
            return Created(string.Empty, response);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Registration failed due to invalid operation. Email: {Email}", request.Email);
            return Conflict(new { message = ex.Message });
        }
        catch (DbUpdateException ex)
        {
            // Check if it's a PostgreSQL duplicate key violation
            if (ex.InnerException is Npgsql.PostgresException pgEx && pgEx.SqlState == "23505")
            {
                logger.LogWarning(
                    "Duplicate key violation during registration. Constraint: {ConstraintName}, Email: {Email}",
                    pgEx.ConstraintName,
                    request.Email);
                
                // Check which unique constraint was violated
                if (pgEx.ConstraintName?.Contains("Email", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return Conflict(new { message = "An account with this email address already exists. Please use a different email or try logging in." });
                }
                if (pgEx.ConstraintName?.Contains("Mobile", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return Conflict(new { message = "An account with this mobile number already exists. Please use a different mobile number." });
                }
                if (pgEx.ConstraintName?.Contains("UniqueId", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return Conflict(new { message = "A registration error occurred. Please try again." });
                }
                // Generic duplicate key error
                return Conflict(new { message = "This information is already registered. Please use different details or try logging in." });
            }
            
            logger.LogError(ex, "Database error during registration. Email: {Email}", request.Email);
            return StatusCode(500, new { message = "We were unable to process your registration. Please try again later." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during registration. Email: {Email}, ErrorType: {ErrorType}", request.Email, ex.GetType().Name);
            return StatusCode(500, new { message = "We were unable to process your registration at this time. Please try again in a few minutes." });
        }
    }
}

public sealed record RegisterStudentApplicantRequest(
    string FullName,
    DateOnly DateOfBirth,
    string Gender,
    string Email,
    string MobileNumber,
    IFormFile ProfilePhoto);

public sealed record RegisterStudentApplicantResponse(string UniqueId, string TemporaryPassword);

