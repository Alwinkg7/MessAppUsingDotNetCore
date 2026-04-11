using MessApplication.models.Dto;

namespace MessApplication.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto dto);
    }
}
