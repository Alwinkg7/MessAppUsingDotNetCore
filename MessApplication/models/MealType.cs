namespace MessApplication.models
{
    public class MealType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        // Navigation property for related transactions
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<MealWindow> MealWindows { get; set; }
        public ICollection<MealPricing> MealPricings { get; set; }
    }
}
