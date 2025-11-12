namespace Core.Entities;

public sealed class Event
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required DateTime EventDate { get; set; }

    public required int Capacity { get; set; }

    public required decimal Price { get; set; }
}