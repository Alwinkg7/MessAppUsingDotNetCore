using MessApplication.Interface;
using MessApplication.service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpPost("generate/{year}/{month}")]
        public async Task<IActionResult> Generate(int year, int month)
        {
            await _billingService.GenerateMonthlyBills(year, month);
            return Ok(new { message = "Billing generated successfully" });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBills(int userId)
        {
            var result = await _billingService.GetUserBills(userId);
            return Ok(result);
        }
    }
}
