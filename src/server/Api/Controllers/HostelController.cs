using ERP.Api.Extensions;
using ERP.Application.Hostel.Commands.AllocateRoom;
using ERP.Application.Hostel.Commands.CreateHostelRoom;
using ERP.Application.Hostel.Commands.UpdateHostelRoom;
using ERP.Application.Hostel.Commands.VacateRoom;
using ERP.Application.Hostel.Queries.GetHostelRoom;
using ERP.Application.Hostel.Queries.ListHostelRooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HostelController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<HostelController> _logger;

    public HostelController(IMediator mediator, ILogger<HostelController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("rooms")]
    public async Task<ActionResult<ListHostelRoomsResult>> ListHostelRooms(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? blockName = null,
        [FromQuery] string? roomType = null,
        [FromQuery] string? status = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListHostelRoomsQuery(
            page,
            pageSize,
            blockName,
            roomType,
            status != null && Enum.TryParse<ERP.Domain.Hostel.Entities.RoomStatus>(status, true, out var parsedStatus) ? parsedStatus : null,
            searchTerm);

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("rooms/{id}")]
    public async Task<ActionResult<GetHostelRoomResult>> GetHostelRoom(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetHostelRoomQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("rooms")]
    public async Task<ActionResult<Guid>> CreateHostelRoom(
        [FromBody] CreateHostelRoomRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateHostelRoomCommand(
                request.RoomNumber,
                request.BlockName,
                request.FloorNumber,
                request.Capacity,
                request.RoomType,
                request.MonthlyRent,
                request.Facilities,
                request.Notes,
                this.GetCurrentUserId());

            var roomId = await _mediator.Send(command, cancellationToken);
            return Ok(roomId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating hostel room");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("rooms/{id}")]
    public async Task<ActionResult> UpdateHostelRoom(
        Guid id,
        [FromBody] UpdateHostelRoomRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateHostelRoomCommand(
                id,
                request.BlockName,
                request.FloorNumber,
                request.Capacity,
                request.RoomType,
                request.MonthlyRent,
                request.Facilities,
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
            _logger.LogError(ex, "Error updating hostel room {RoomId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("rooms/allocate")]
    public async Task<ActionResult<Guid>> AllocateRoom(
        [FromBody] AllocateRoomRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new AllocateRoomCommand(
                request.RoomId,
                request.StudentId,
                request.AllocationDate,
                request.MonthlyRent,
                request.BedNumber,
                request.Remarks,
                this.GetCurrentUserId());

            var allocationId = await _mediator.Send(command, cancellationToken);
            return Ok(allocationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error allocating room");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("rooms/vacate")]
    public async Task<IActionResult> VacateRoom(
        [FromBody] VacateRoomRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new VacateRoomCommand(
                request.AllocationId,
                request.VacatedDate,
                this.GetCurrentUserId(),
                request.Remarks);

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error vacating room");
            return BadRequest(new { message = ex.Message });
        }
    }
}

// Request DTOs
public record CreateHostelRoomRequest(
    string RoomNumber,
    string BlockName,
    string FloorNumber,
    int Capacity,
    string RoomType,
    decimal? MonthlyRent = null,
    string? Facilities = null,
    string? Notes = null,
    string? CreatedBy = null);

public record UpdateHostelRoomRequest(
    string? BlockName = null,
    string? FloorNumber = null,
    int? Capacity = null,
    string? RoomType = null,
    decimal? MonthlyRent = null,
    string? Facilities = null,
    string? Notes = null);

public record AllocateRoomRequest(
    Guid RoomId,
    Guid StudentId,
    DateTime AllocationDate,
    decimal? MonthlyRent = null,
    string? BedNumber = null,
    string? Remarks = null,
    string? AllocatedBy = null);

public record VacateRoomRequest(
    Guid AllocationId,
    DateTime VacatedDate,
    string? VacatedBy = null,
    string? Remarks = null);

