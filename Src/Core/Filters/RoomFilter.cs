namespace Core.Filters;
public sealed class RoomFilter
{
    public int? HotelId {get; init;}
    public int? MinCapacity {get; init;}
    public int? MaxCapacity {get; init;}
    public decimal? MinPrice {get; init;}
    public decimal? MaxPrice {get; init;}
    public string? SortBy {get; init;}
    public string SortDirection {get; init;}
    public int Page {get; init;}
    public int PageSize {get; init;}
}