namespace MessApplication.models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string QrCodeValue { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        //Navigation property for related transactions
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<MonthlyBill> MonthlyBills { get; set; }
    }
}
