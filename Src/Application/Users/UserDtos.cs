namespace Application.Users;
public sealed record UserCreateDto(
    string Email,
    string PasswordHash,
    string Role = "Customer"
);

public sealed record UserResponseDto(
    int Id,
    string Email,
    string Role
);