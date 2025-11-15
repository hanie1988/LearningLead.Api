namespace Application.Hotels;

using Core.Entities;
using Core.Interfaces;

public sealed class HotelService(IHotelRepository repo)
{
    public async Task<Hotel> CreateAsync(HotelCreateDto dto)
    {
        // Business logic belongs here
        var hotel = new Hotel
        {
            Name = dto.Name,
            City = dto.City,
            Description = dto.Description
        };

        return await repo.AddAsync(hotel);
    }

    public Task<List<Hotel>> GetAllAsync()
        => repo.GetAllAsync();

    public Task<Hotel?> GetByIdAsync(int id)
        => repo.GetByIdAsync(id);

    public Task<bool> DeleteAsync(int id)
        => repo.DeleteAsync(id);
}