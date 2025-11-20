namespace Application.Reservations;

public sealed record ReservationCreateDto(
    int RoomId,
    int UserId,
    DateTime CheckIn,
    DateTime CheckOut
);

public sealed readonly record struct ReservationResponseDto(
    int Id,
    int RoomId,
    int UserId,
    DateTime CheckIn,
    DateTime CheckOut
);