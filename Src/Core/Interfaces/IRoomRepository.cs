namespace Core.Interfaces;

using Core.Entities;

public interface IRoomRepository
{
    Task<Room> AddAsync(Room room);
    Task<List<Room>> GetByHotelAsync(int hotelId);
}