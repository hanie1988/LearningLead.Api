namespace Api.Controllers;

using Application.Events;
using Core.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/events")]
public sealed class EventController(EventRepository repo) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(EventCreateDto dto)
    {
        var ev = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            EventDate = dto.EventDate,
            Capacity = dto.Capacity,
            Price = dto.Price
        };

        var created = await repo.AddAsync(ev);

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
        => Ok(await repo.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await repo.GetByIdAsync(id);
        return ev is null ? NotFound() : Ok(ev);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repo.DeleteAsync(id);
        return deleted ? Ok() : NotFound();
    }
}