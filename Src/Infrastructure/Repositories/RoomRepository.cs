namespace Infrastructure.Repositories;

using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

public sealed class RoomRepository(AppDbContext db): IRoomRepository
{
    public async Task<Room> AddAsync(Room room)
    {
        db.Rooms.Add(room);
        await db.SaveChangesAsync();
        return room;
    }

    public async Task<List<Room>> GetByHotelAsync(int hotelId)
        => await db.Rooms.Where(r => r.HotelId == hotelId).ToListAsync();

}