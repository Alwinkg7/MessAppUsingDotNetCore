namespace MessApplication.models.Dto
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string QrCodeValue { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
