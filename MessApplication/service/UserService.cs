using MessApplication.models.Dto;
using MessApplication.models;
using MessApplication.Interface;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;


namespace MessApplication.service
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
        {
            var qrToken = Guid.NewGuid().ToString(); // Generate a unique QR code value

            var user = new User
            {
                Name = dto.Name,
                QrCodeValue = qrToken,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RoleId = dto.RoleId,
                IsActive = true,
                RegisteredAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                QrCodeValue = user.QrCodeValue
            };
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    QrCodeValue = u.QrCodeValue
                })
                .ToListAsync();
        }

        
    }
}
