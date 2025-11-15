namespace Application.Hotels;

public sealed record HotelCreateDto(
    string Name,
    string City,
    string Description
);

public readonly record struct HotelResponseDto(
    int Id,
    string Name,
    string City,
    string Description
);