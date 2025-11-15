namespace Core.Interfaces;

using Core.Entities;

public interface IEventRepository
{
    Task<Event> AddAsync(Event ev);
    Task<List<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}