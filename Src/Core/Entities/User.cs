namespace Core.Entities;

public sealed class User(string email, string passwordHash, string role = "Customer")
{
    public int Id { get; set; }

    public required string Email { get; set; } = email;

    public required string PasswordHash { get; set; } = passwordHash;

    public required string Role { get; set; } = role;
}