namespace MessApplication.models
{
    public class MealPricing
    {
        public int Id { get; set; }
        public int MealTypeId { get; set; }
        public decimal PricePerMeal { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        // Navigation property for related meal type
        public MealType MealType { get; set; }
    }
}
