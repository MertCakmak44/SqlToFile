using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAppCore.Interfaces;
using MyAppCore.Dtos;
using System.Threading.Tasks;
using MyAppCore.Entities;


namespace SqlToFile.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService purchaseService;
        public PurchasesController(IPurchaseService purchaseService)
        {
            this.purchaseService = purchaseService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseCreateDto dto)
        {
            var purchase = await purchaseService.AddAsync(dto);
            return Ok(purchase);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purchases = await purchaseService.GetAllAsync();
            return Ok(purchases);
        }
    }
}
