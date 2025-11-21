namespace Application.Rooms;

public sealed record RoomCreateDto(
    int HotelId,
    string RoomNumber,
    int Capacity,
    decimal PricePerNight
);

public readonly record struct RoomResponseDto(
    int Id,
    int HotelId,
    string RoomNumber,
    int Capacity,
    decimal PricePerNight
);