namespace Infrastructure.Repositories;

using Core.Entities;
using Infrastructure.Data;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

public sealed class ReservationRepository(AppDbContext db) : IReservationRepository
{
    public async Task<Reservation> AddAsync(Reservation reservation)
    {
        db.Reservations.Add(reservation);
        await db.SaveChangesAsync();
        return reservation;
    }

    public async Task<bool> RoomIsAvailable(int roomId, DateTime checkIn, DateTime checkOut)
    {
        return !await db.Reservations.AnyAsync(r =>
            r.RoomId == roomId &&
            checkIn < r.CheckOut &&
            checkOut > r.CheckIn &&
            !r.IsCancelled
        );
    }

    public async Task<List<Reservation>> GetReservationsByRoom(int roomId)
       => await db.Reservations.Where(r => r.RoomId == roomId).ToListAsync();

    public async Task<Reservation?> GetByIdAsync(int id)
        => await db.Reservations.FindAsync(id);

    public async Task UpdateAsync(Reservation reservation)
    {
        db.Reservations.Update(reservation);
        await db.SaveChangesAsync();
    }

}
