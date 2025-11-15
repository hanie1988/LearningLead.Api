namespace Api.Controllers;

using Application.Users;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/hotels")]
public sealed class UserController(IUserService userService) : ControllerBase
{
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