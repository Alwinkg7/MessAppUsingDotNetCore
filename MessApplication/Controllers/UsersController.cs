using MessApplication.Interface;
using MessApplication.models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _userService = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            try
            {
                var result = await _userService.CreateUserAsync(dto);
                _logger.LogInformation("User created successfully with ID: {UserId}", result.Id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input for creating user with Name: {UserName}", dto.Name);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                _logger.LogInformation("Retrieved {UserCount} users", users.Count());
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users");
                return StatusCode(500, "An error occurred while retrieving users");
            }
        }


    }
}
