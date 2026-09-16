using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniCMS.Application.DTOs;
using MiniCMS.Application.Interfaces;
using MiniCMS.Infrastructure.Data;

namespace MiniCMS.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.Trim().ToLower());

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        var jwtKey = _configuration["Jwt:Key"] ?? "MiniCMS_Secret_Key_For_Jwt_Token_Signing_2026_CEP_So_Tuyen";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "MiniCMS_Api";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "MiniCMS_Client";
        var expiryHours = int.TryParse(_configuration["Jwt:ExpiryHours"], out var hours) ? hours : 8;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtKey);
        var expiresAt = DateTime.UtcNow.AddHours(expiryHours);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = expiresAt,
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new LoginResponseDto
        {
            Token = tokenHandler.WriteToken(token),
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role,
            ExpiresAt = expiresAt
        };
    }
}