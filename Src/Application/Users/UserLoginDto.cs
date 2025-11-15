namespace Application.Users;

public sealed record UserLoginDto(
    string Email,
    string Password
);

public sealed record LoginResponseDto(
    int UserId,
    string Email,
    string Role,
    string Token
);