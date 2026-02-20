namespace MessApplication.models
{
    public class MealWindow
    {
        public int Id { get; set; }
        public int MealTypeId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; }
        public  DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        // Navigation property for related meal type
        public MealType MealType { get; set; }
    }
}
