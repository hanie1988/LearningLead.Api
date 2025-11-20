namespace Api.Controllers;

using Application.Hotels;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/hotels")]
[Authorize]
public sealed class HotelController(HotelService service) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(HotelCreateDto dto)
    {
        var created = await service.CreateAsync(dto);

        return Ok(new HotelResponseDto(
            created.Id,
            created.Name,
            created.City,
            created.Description
        ));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var hotel = await service.GetByIdAsync(id);
        return hotel is null ? NotFound() : Ok(hotel);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.DeleteAsync(id);
        return deleted ? Ok() : NotFound();
    }
}