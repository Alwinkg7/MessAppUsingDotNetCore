using MessApplication;
using MessApplication.Interface;
using MessApplication.models;
using MessApplication.models.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class BillingService : IBillingService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public BillingService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task GenerateMonthlyBills(int year, int month)
    {
        var pricePerMeal = decimal.Parse(_config["Pricing:PricePerMeal"]);

        var yearParam = new SqlParameter("@Year", year);
        var monthParam = new SqlParameter("@Month", month);
        var priceParam = new SqlParameter("@PricePerMeal", pricePerMeal);

        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC GenerateMonthlyBills @Year, @Month, @PricePerMeal",
                yearParam,
                monthParam,
                priceParam
            );
        }
        catch (SqlException ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    public async Task<List<UserBillDto>> GetUserBills(int userId)
    {
        return await _context.MonthlyBills
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Year)
            .ThenByDescending(b => b.Month)
            .Select(b => new UserBillDto
            {
                Id = b.Id,
                Year = b.Year,
                Month = b.Month,
                TotalMealsConsumed = b.TotalMealsConsumed,
                TotalAmount = b.TotalAmount,
                Status = b.Status.ToString(),
                GeneratedAt = b.GeneratedAt
            })
            .ToListAsync();
    }
}