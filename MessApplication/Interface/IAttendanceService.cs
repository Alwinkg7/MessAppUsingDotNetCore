using MessApplication.models.Dto;

namespace MessApplication.Interface
{
    public interface IAttendanceService
    {
        Task<ScanResponseDto> ScanAsync(ScanQrDto dto);
        Task<UserAttendanceDto> GetUserAttendance(int userId);
        Task<AdminDashboardDto> GetDashboardStats();
    }
}
