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
        private readonly ISaleService _saleService;
        private readonly IPurchaseService _purchaseService;

        public AdminController(
            ICustomerService customerService,
            IStockService stockService,
            ISaleService saleService,
            IPurchaseService purchaseService)
        {
            _customerService = customerService;
            _stockService = stockService;
            _saleService = saleService;
            _purchaseService = purchaseService;
        }

        [HttpDelete("delete-all-data")]
        public async Task<IActionResult> DeleteAllData()
        {
            await _customerService.DeleteAllAsync();
            await _stockService.DeleteAllAsync();
            await _saleService.DeleteAllAsync();
            await _purchaseService.DeleteAllAsync();

            return Ok(new { message = "Tüm müşteri, stok, satış ve alış verileri silindi." });
        }
    }
}
