namespace Core.Entities;

public sealed class User(string email, string passwordHash, string role = "Customer")
{
    public int Id { get; set; }

    public string Email { get; set; } = email;

    public string PasswordHash { get; set; } = passwordHash;

    public string Role { get; set; } = role;
}