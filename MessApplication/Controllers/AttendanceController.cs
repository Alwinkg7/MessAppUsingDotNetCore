using MessApplication.Interface;
using MessApplication.models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(IAttendanceService attendanceService, ILogger<AttendanceController> logger)
        {
            _attendanceService = attendanceService;
            _logger = logger;
        }

        [HttpPost("scan")]
        public async Task<IActionResult> Scan(ScanQrDto dto)
        {
            try
            {
                var result = await _attendanceService.ScanAsync(dto);
                _logger.LogInformation("Scanning complete successfully");
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Invalid input");
                throw new BusinessException("Meal already consumed");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAttendance(int userId)
        {
            var result = await _attendanceService.GetUserAttendance(userId);
            return Ok(result);
        }

        [Authorize(Roles = "1")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _attendanceService.GetDashboardStats();
            return Ok(result);
        }
    }
}
