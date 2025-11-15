namespace Application.Events;

using Core.Entities;
using Core.Interfaces;

public sealed class EventService(IEventRepository repo)
{
    public async Task<Event> CreateAsync(EventCreateDto dto)
    {
        var ev = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            EventDate = dto.EventDate,
            Capacity = dto.Capacity,
            Price = dto.Price
        };

        return await repo.AddAsync(ev);
    }

    public Task<List<Event>> GetAllAsync()
        => repo.GetAllAsync();

    public Task<Event?> GetByIdAsync(int id)
        => repo.GetByIdAsync(id);

    public Task<bool> DeleteAsync(int id)
        => repo.DeleteAsync(id);
}