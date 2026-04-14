using MessApplication.Interface;
using MessApplication.models;
using MessApplication.models.Dto;
using Microsoft.EntityFrameworkCore;

namespace MessApplication.service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ILogger<AttendanceService> _logger;
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context, ILogger<AttendanceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ScanResponseDto> ScanAsync(ScanQrDto dto)
        {
            var now = DateTime.UtcNow;
            var today = now.Date;
            var currentTime = now.TimeOfDay;

            //find user by qr code
            var user = await _context.Users.
                FirstOrDefaultAsync(u => u.QrCodeValue == dto.QrCodeValue);

            if (user == null)
            {
                _logger.LogError("Invalid QR Code");
                throw new BusinessException("Invalid QR Code");
            }

            if (!user.IsActive)
            {
                _logger.LogError("User is inactive");
                throw new BusinessException("User is inactive");
            }

            //detect active meal window
            var mealWindow = await _context.MealWindows
                .Include(m => m.MealType)
                .FirstOrDefaultAsync(m =>
                m.IsActive &&
                currentTime >= m.StartTime &&
                currentTime <= m.EndTime);

            if (mealWindow == null)
            {
                _logger.LogError("No active meal window currently available");
                throw new BusinessException("No active meal window currently available");

            }

            // check dupilcate attendance
            var alreadyScanned = await _context.Attendances
                .AnyAsync(a =>
                a.UserId == user.Id &&
                a.MealTypeId == mealWindow.MealTypeId &&
                a.AttendanceDate == today);

            if (alreadyScanned)
            {
                _logger.LogError("Meal already consumed");
                throw new BusinessException("Meal already consumed");
            }
            

            //insert new attendance for that user
            var attendance = new Attendance
            {
                UserId = user.Id,
                MealTypeId = mealWindow.MealTypeId,
                AttendanceDate = today,
                ScanTime = now,
                CreatedAt = now
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            _logger.LogInformation("attendance saved");

            return new ScanResponseDto
            {
                Message = "Meal recorded successfully",
                MealType = mealWindow.MealType.Name,
                ScanTime = now
            };

        }

        public async Task<UserAttendanceDto> GetUserAttendance(int userId)
        {
            var today = DateTime.Now.Date;

            var attendances = await _context.Attendances
                .Where(a => a.UserId == userId)
                .Include(a => a.MealType)
                .ToListAsync();

            var todayData = attendances
                .Where(a => a.AttendanceDate == today)
                .ToList();

            var todayStatus = new
            {
                breakfast = todayData.Any(a => a.MealType.Name == "Breakfast") ? "Completed" : "Pending",
                lunch = todayData.Any(a => a.MealType.Name == "Lunch") ? "Completed" : "Pending",
                dinner = todayData.Any(a => a.MealType.Name == "Dinner") ? "Completed" : "Pending"
            };

            var history = attendances
                .OrderByDescending(a => a.AttendanceDate)
                .Select(a => new AttendanceHistoryDto
                {
                    Date = a.AttendanceDate,
                    MealType = a.MealType.Name,
                    Time = a.ScanTime
                })
                .ToList();

            return new UserAttendanceDto
            {
                Today = todayStatus,
                History = history
            };
        }

        public async Task<AdminDashboardDto> GetDashboardStats()
        {
            var today = DateTime.Now.Date;
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // Total users
            var totalUsers = await _context.Users.CountAsync(u => u.IsActive);

            // Meals served today
            var mealsToday = await _context.Attendances
                .CountAsync(a => a.AttendanceDate == today);

            // Pending payments
            var pendingPayments = await _context.MonthlyBills
                .CountAsync(b => b.Status != BillStatus.Paid); // assuming 2 = Paid

            // Monthly revenue (current month only)
            var monthlyRevenue = await _context.MonthlyBills
                .Where(b => b.Month == currentMonth && b.Year == currentYear)
                .SumAsync(b => (decimal?)b.TotalAmount) ?? 0;

            return new AdminDashboardDto
            {
                TotalUsers = totalUsers,
                MealsToday = mealsToday,
                PendingPayments = pendingPayments,
                MonthlyRevenue = monthlyRevenue
            };
        }
    }
}
