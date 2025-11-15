namespace Infrastructure.Repositories;

using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public sealed class EventRepository(AppDbContext db) : IEventRepository
{
    public async Task<Event> AddAsync(Event ev)
    {
        db.Events.Add(ev);
        await db.SaveChangesAsync();
        return ev;
    }

    public async Task<List<Event>> GetAllAsync()
        => await db.Events.ToListAsync();

    public async Task<Event?> GetByIdAsync(int id)
        => await db.Events.FindAsync(id);

    public async Task<bool> DeleteAsync(int id)
    {
        var ev = await db.Events.FindAsync(id);
        if (ev is null) return false;

        db.Events.Remove(ev);
        await db.SaveChangesAsync();
        return true;
    }
}