namespace MessApplication.models.Dto
{
    public class BillingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalMealsConsumed { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
