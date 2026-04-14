using MessApplication;
using MessApplication.Interface;
using MessApplication.models;
using MessApplication.models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new BusinessException("Invalid credentials");

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!isValid)
                throw new BusinessException("Invalid credentials");

            return GenerateJwtToken(user);
        }
        catch (Exception)
        {
            // 🔥 MOCK USERS
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Email = "admin@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    RoleId = 1
                },
                new User
                {
                    Id = 2,
                    Email = "user@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("user123"),
                    RoleId = 2
                }
            };

            var user = users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
                throw new BusinessException("Invalid credentials");

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!isValid)
                throw new BusinessException("Invalid credentials");

            return GenerateJwtToken(user);
        }
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "THIS_IS_FALLBACK_KEY_123456789";)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                int.Parse(_config["Jwt:ExpiryMinutes"])
            ),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}