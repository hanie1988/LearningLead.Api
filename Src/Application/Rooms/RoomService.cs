namespace Application.Rooms;

using Core.Entities;
using Core.Interfaces;

public sealed class RoomService(IRoomRepository repo) 
{
    public async Task<Room> CreateRoomAsync(RoomCreateDto dto)
    {
        var room = new Room
        {
            HotelId = dto.HotelId,
            RoomNumber = dto.RoomNumber,
            Capacity = dto.Capacity,
            PricePerNight = dto.PricePerNight
        };

        return await repo.AddAsync(room);
    }

    public async Task<IEnumerable<Room>> GetRoomsByHotelAsync(int hotelId)
    {
        return await repo.GetByHotelAsync(hotelId);
    }
}