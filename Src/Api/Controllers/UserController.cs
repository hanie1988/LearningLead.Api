namespace Api.Controllers;

using Application.Users;
using Core.Entities;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
public sealed class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserCreateDto dto)
    {
        var user = await userService.RegisterAsync(dto.Email, dto.PasswordHash, dto.Role);

        return Ok(new UserResponseDto(
            user.Id,
            user.Email,
            user.Role
        ));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        UserLoginDto dto,
        [FromServices] JwtService jwtService)
    {
        var user = await userService.ValidateUserAsync(dto.Email, dto.Password);

        if (user is null)
            return Unauthorized("Invalid email or password");

        var token = jwtService.GenerateToken(user);

        return Ok(new LoginResponseDto(
            user.Id,
            user.Email,
            user.Role,
            token
        ));
    }
}