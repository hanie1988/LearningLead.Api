namespace Application.Queries.Rooms;

using Core.Entities;
public static class RoomFilteringExtensions
{
    public static IQueryable<Room> ApplyFilter(
        this IQueryable<Room> query,
        RoomFilterDto filter)
    {
        if (filter.HotelId.HasValue)
            query = query.Where(r => r.HotelId == filter.HotelId.Value);

        if (filter.MinCapacity.HasValue)
            query = query.Where(r => r.Capacity >= filter.MinCapacity.Value);

        if (filter.MaxCapacity.HasValue)
            query = query.Where(r => r.Capacity <= filter.MaxCapacity.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(r => r.PricePerNight >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(r => r.PricePerNight <= filter.MaxPrice.Value);

        return query;
    }
}