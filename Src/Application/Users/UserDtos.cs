namespace Application.Users;

//Request Dto
public sealed record UserCreateDto(
    string Email,
    string PasswordHash,
    string Role = "Customer"
);

//Response Dto
public readonly record struct UserResponseDto(
    int Id,
    string Email,
    string Role
);