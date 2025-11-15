namespace Core.Entities;

public sealed class Room
{
    public int Id { get; set; }

    public required int HotelId { get; set; }

    public required string RoomNumber { get; set; }

    public required int Capacity { get; set; }

    public required decimal PricePerNight { get; set; }

    public Hotel? Hotel { get; set; }
}