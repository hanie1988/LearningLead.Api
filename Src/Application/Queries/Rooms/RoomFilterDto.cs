namespace Application.Queries.Rooms;

public sealed record RoomFilterDto(
    int? HotelId = null,
    int? MinCapacity = null,
    int? MaxCapacity = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string? SortBy = null,
    string SortDirection = "asc",
    int Page = 1,
    int PageSize = 20
);