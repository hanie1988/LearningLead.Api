namespace Api.Controllers;

using Application.Reservations;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/reservations")]
//[Authorize]
public sealed class ReservationController(ReservationService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ReservationResponseDto>> Create(ReservationCreateDto dto)
    {
        var created = await service.CreateReservationAsync(dto);

        var response = new ReservationResponseDto(
            created.Id,
            created.RoomId,
            created.UserId,
            created.CheckIn,
            created.CheckOut
        );

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var success = await service.CancelReservationAsync(id);

        if (!success)
            return NotFound("Reservation not found");

        return Ok("Reservation cancelled successfully");
    }
}