using Microsoft.AspNetCore.Mvc;
using MiniCMS.Application.DTOs;
using MiniCMS.Application.Interfaces;

namespace MiniCMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu." });
        }

        var response = await _authService.AuthenticateAsync(request);
        if (response == null)
        {
            return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác." });
        }

        return Ok(response);
    }
}