using Application.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;

namespace Api.Controllers;

[ApiController]
[Route("api/rooms")]
public sealed class RoomController(RoomService service) : ControllerBase
{
    [HttpPost]
    //[Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(RoomCreateDto dto)
    {
        var created = await service.CreateRoomAsync(dto);

        var response = new RoomResponseDto(
            created.Id,
            created.HotelId,
            created.RoomNumber,
            created.Capacity,
            created.PricePerNight
        );

        return Ok(response);
    }

    [HttpGet("by-hotel/{hotelId:int}")]
    public async Task<IActionResult> Get(int hotelId)
    {
        var rooms = await service.GetRoomsByHotelAsync(hotelId);
        return Ok(rooms);
    }
}