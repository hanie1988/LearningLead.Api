namespace Core.Entities;

public sealed class Ticket
{
    public int Id { get; set; }

    public required int EventId { get; set; }

    public required int UserId { get; set; }

    public Event? Event { get; set; }

    public User? User { get; set; }
}