namespace Core.Entities;

public sealed class Hotel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string City { get; set; }

    public required string Description { get; set; }
}