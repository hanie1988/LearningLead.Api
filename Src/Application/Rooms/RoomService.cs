namespace Application.Rooms;

using Core.Entities;
using Core.Interfaces;
using Application.Queries.Rooms;
using Core.Filters;

public sealed class RoomService(IRoomRepository repo, IRoomQueryRepository queryRepo) 
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

    public async Task<PagedResult<RoomResponseDto>> SearchAsync(
        RoomFilterDto dto,
        CancellationToken ct)
    {
        var filter = new RoomFilter {
            HotelId = dto.HotelId,
            MinCapacity = dto.MinCapacity,
            MaxCapacity = dto.MaxCapacity,
            MinPrice = dto.MinPrice,
            MaxPrice = dto.MaxPrice,
            SortBy = dto.SortBy,
            SortDirection = dto.SortDirection,
            Page = dto.Page,
            PageSize = dto.PageSize
        };

        
        var count = await queryRepo.CountAsync(filter, ct);
        var rooms = await queryRepo.SearchAsync(filter, ct);

        var items = rooms
            .Select(r => new RoomResponseDto(
                r.Id,
                r.HotelId,
                r.RoomNumber,
                r.Capacity,
                r.PricePerNight
            ))
            .ToList();

        return new PagedResult<RoomResponseDto>
        {
            Items = items,
            TotalCount = count,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

}