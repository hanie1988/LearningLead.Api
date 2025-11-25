namespace Application.Queries.Rooms;

using Core.Entities;
public static class RoomSortingExtensions
{
    public static IQueryable<Room> ApplySorting(
        this IQueryable<Room> query,
        string? sortBy,
        string sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query.OrderBy(r => r.Id);

        return sortBy.ToLower() switch
        {
            "price" => sortDirection == "desc"
                ? query.OrderByDescending(r => r.PricePerNight)
                : query.OrderBy(r => r.PricePerNight),

            "capacity" => sortDirection == "desc"
                ? query.OrderByDescending(r => r.Capacity)
                : query.OrderBy(r => r.Capacity),

            _ => query.OrderBy(r => r.Id)
        };
    }
}