using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyAppCore.Interfaces;

namespace SqlToFile.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IStockService _stockService;
        public AdminController(ICustomerService customerService, IStockService stockService)
        {
            _customerService = customerService;
            _stockService = stockService;
        }
        [HttpDelete("delete-all-data")]
        public async Task<IActionResult> DeleteAllCustomerAndStocks()
        {
            await _customerService.DeleteAllAsync();
            await _stockService.DeleteAllAsync();

            return Ok(new { message = "Tüm müşteri ve stok verileri silindi." });

        }
    }
}
