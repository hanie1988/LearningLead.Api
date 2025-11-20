namespace Application.Events;

public sealed record EventCreateDto(
    string Title,
    string Description,
    DateTime EventDate,
    int Capacity,
    decimal Price
);

public readonly record struct EventResponseDto(
    int Id,
    string Title,
    string Description,
    DateTime EventDate,
    int Capacity,
    decimal Price
);