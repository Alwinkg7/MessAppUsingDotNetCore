namespace MessApplication.models.Dto
{
    public class UserAttendanceDto
    {
        public object Today { get; set; }
        public List<AttendanceHistoryDto> History { get; set; }
    }

    public class AttendanceHistoryDto
    {
        public DateTime Date { get; set; }
        public string MealType { get; set; }
        public DateTime Time { get; set; }
    }
}
