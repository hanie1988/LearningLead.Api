namespace Api.Controllers;

using Application.Events;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/events")]
public sealed class EventController(EventService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(EventCreateDto dto)
    {
        var created = await service.CreateAsync(dto);

        return Ok(new EventResponseDto(
            created.Id,
            created.Title,
            created.Description,
            created.EventDate,
            created.Capacity,
            created.Price
        ));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await service.GetByIdAsync(id);
        return ev is null ? NotFound() : Ok(ev);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await service.DeleteAsync(id) ? Ok() : NotFound();
    }
}