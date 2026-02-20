namespace MessApplication.models
{
    public class MonthlyBill
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalMealsConsumed { get; set; }
        public int TotalMealsSkipped { get; set; }
        public decimal TotalAmount { get; set; }
        public BillStatus Status { get; set; }
        public DateTime GeneratedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        // Navigation property for related user
        public User User { get; set; }
    }

    public enum BillStatus
    {
        Generated = 1,
        Paid = 2,
        Overdue = 3
    }
}
