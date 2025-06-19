using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;

namespace SqlToFile.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockService.GetAllAsync();
            return Ok(stocks);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Stock stock)
        {
            try
            {
                var created = await _stockService.AddAsync(stock);
                return Ok(created);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StockUpdateDto updatedStock)
        {
            updatedStock.ID = id;
            await _stockService.UpdateAsync(updatedStock);
            return Ok(new { message = "Stok güncellendi." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _stockService.DeleteAsync(id);
            return Ok();
        }

    }
}
