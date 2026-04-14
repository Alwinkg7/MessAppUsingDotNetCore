using MessApplication.Interface;
using MessApplication.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> GetAllBills()
        {
            var result = await _billingService.GetAllBills();
            return Ok(result);
        }

        [Authorize(Roles = "1")]
        [HttpPut("pay/{billId}")]
        public async Task<IActionResult> MarkAsPaid(int billId)
        {
            try
            {
                await _billingService.MarkBillAsPaid(billId);
                return Ok(new { message = "Payment marked as paid" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
