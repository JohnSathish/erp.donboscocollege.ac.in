using ERP.Api.Extensions;
using ERP.Application.Transport.Commands.CreateVehicle;
using ERP.Application.Transport.Commands.UpdateVehicle;
using ERP.Application.Transport.Queries.GetVehicle;
using ERP.Application.Transport.Queries.ListVehicles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransportController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TransportController> _logger;

    public TransportController(IMediator mediator, ILogger<TransportController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("vehicles")]
    public async Task<ActionResult<ListVehiclesResult>> ListVehicles(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? vehicleType = null,
        [FromQuery] string? status = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListVehiclesQuery(
            page,
            pageSize,
            vehicleType,
            status != null && Enum.TryParse<ERP.Domain.Transport.Entities.VehicleStatus>(status, true, out var parsedStatus) ? parsedStatus : null,
            searchTerm);

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("vehicles/{id}")]
    public async Task<ActionResult<GetVehicleResult>> GetVehicle(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetVehicleQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("vehicles")]
    public async Task<ActionResult<Guid>> CreateVehicle(
        [FromBody] CreateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateVehicleCommand(
                request.VehicleNumber,
                request.VehicleType,
                request.Make,
                request.Model,
                request.Capacity,
                request.Color,
                request.Year,
                request.RegistrationNumber,
                request.RegistrationExpiryDate,
                request.InsuranceNumber,
                request.InsuranceExpiryDate,
                request.DriverName,
                request.DriverContactNumber,
                request.Route,
                request.Notes,
                this.GetCurrentUserId());

            var vehicleId = await _mediator.Send(command, cancellationToken);
            return Ok(vehicleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vehicle");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("vehicles/{id}")]
    public async Task<ActionResult> UpdateVehicle(
        Guid id,
        [FromBody] UpdateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateVehicleCommand(
                id,
                request.VehicleType,
                request.Make,
                request.Model,
                request.Capacity,
                request.Color,
                request.Year,
                request.RegistrationNumber,
                request.RegistrationExpiryDate,
                request.InsuranceNumber,
                request.InsuranceExpiryDate,
                request.DriverName,
                request.DriverContactNumber,
                request.Route,
                request.Notes,
                this.GetCurrentUserId());

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vehicle {VehicleId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }
}

// Request DTOs
public record CreateVehicleRequest(
    string VehicleNumber,
    string VehicleType,
    string Make,
    string Model,
    int Capacity,
    string? Color = null,
    int? Year = null,
    string? RegistrationNumber = null,
    DateTime? RegistrationExpiryDate = null,
    string? InsuranceNumber = null,
    DateTime? InsuranceExpiryDate = null,
    string? DriverName = null,
    string? DriverContactNumber = null,
    string? Route = null,
    string? Notes = null,
    string? CreatedBy = null);

public record UpdateVehicleRequest(
    string? VehicleType = null,
    string? Make = null,
    string? Model = null,
    int? Capacity = null,
    string? Color = null,
    int? Year = null,
    string? RegistrationNumber = null,
    DateTime? RegistrationExpiryDate = null,
    string? InsuranceNumber = null,
    DateTime? InsuranceExpiryDate = null,
    string? DriverName = null,
    string? DriverContactNumber = null,
    string? Route = null,
    string? Notes = null);

