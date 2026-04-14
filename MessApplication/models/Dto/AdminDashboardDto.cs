namespace MessApplication.models.Dto
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int MealsToday { get; set; }
        public int PendingPayments { get; set; }
        public decimal MonthlyRevenue { get; set; }
    }
}
