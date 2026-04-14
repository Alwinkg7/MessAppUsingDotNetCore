using MessApplication.models.Dto;

namespace MessApplication.Interface
{
    public interface IBillingService
    {
        Task GenerateMonthlyBills(int year, int month);
        Task<List<UserBillDto>> GetUserBills(int userId);
        Task<List<BillingDto>> GetAllBills();
        Task MarkBillAsPaid(int billId);
    }
}
