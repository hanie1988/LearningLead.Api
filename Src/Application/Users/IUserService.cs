namespace Application.Users;

using Core.Entities;

public interface IUserService
{
    Task<User> RegisterAsync(string email, string password, string role);
    Task<User?> ValidateUserAsync(string email, string password);
}