namespace MessApplication.models
{
    public class Attendance
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int MealTypeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime ScanTime { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public MealType MealType { get; set; }
    }
}
