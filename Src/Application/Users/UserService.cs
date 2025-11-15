using Core.Entities;
using Core.Interfaces;

namespace Application.Users;

public sealed class UserService(IUserRepository userRepository)
    : IUserService
{
    public async Task<User> RegisterAsync(string email, string password, string role)
    {
        var existing = await userRepository.GetByEmailAsync(email);
        if (existing is not null)
            throw new InvalidOperationException("Email already exists.");

        var hash = PasswordHasher.Hash(password);

        var user = new User(email, hash, role);

        return await userRepository.AddAsync(user);
    }

    public async Task<User?> ValidateUserAsync(string email, string password)
    {
        var user = await userRepository.GetByEmailAsync(email);
        if (user is null)
            return null;

        var hash = PasswordHasher.Hash(password);

        return hash == user.PasswordHash ? user : null;
    }
}