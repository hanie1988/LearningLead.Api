namespace Core.Interfaces;

using Core.Entities;

public interface IHotelRepository
{
    Task<Hotel> AddAsync(Hotel hotel);
    Task<List<Hotel>> GetAllAsync();
    Task<Hotel?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}