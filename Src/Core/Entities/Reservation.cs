namespace Core.Entities;

public sealed class Reservation
{
    public int Id { get; set; }

    public required int RoomId { get; set; }

    public required int UserId { get; set; }

    public required DateTime CheckIn { get; set; }

    public required DateTime CheckOut { get; set; }

    public Room? Room { get; set; }
}