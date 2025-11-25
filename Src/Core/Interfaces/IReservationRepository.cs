namespace Core.Interfaces;

using Core.Entities;

public interface IReservationRepository
{
    Task<Reservation> AddAsync(Reservation reservation);
    Task<bool> RoomIsAvailable(int roomId, DateTime checkIn, DateTime checkOut);
    Task<List<Reservation>> GetReservationsByRoom(int roomId);
    Task<Reservation?> GetByIdAsync(int id);
    Task UpdateAsync(Reservation reservation);
    IQueryable<Reservation> Query();
}