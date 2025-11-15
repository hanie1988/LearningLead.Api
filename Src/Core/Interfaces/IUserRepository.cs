namespace Core.Interfaces;

using Core.Entities;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}