namespace Infrastructure.Repositories;

using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

public sealed class HotelRepository(AppDbContext db): IHotelRepository
{
    public async Task<Hotel> AddAsync(Hotel hotel)
    {
        db.Hotels.Add(hotel);
        await db.SaveChangesAsync();
        return hotel;
    }

    public async Task<List<Hotel>> GetAllAsync()
        => await db.Hotels.ToListAsync();

    public async Task<Hotel?> GetByIdAsync(int id)
        => await db.Hotels.FindAsync(id);

    public async Task<bool> DeleteAsync(int id)
    {
        var h = await db.Hotels.FindAsync(id);
        if (h is null) return false;

        db.Hotels.Remove(h);
        await db.SaveChangesAsync();
        return true;
    }

    public IQueryable<Hotel> Query() => db.Hotels.AsQueryable();
}