using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppCore.Interfaces;


namespace SqlToFile.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Customer customer)
        {
            try
            {
                var created = await _customerService.AddAsync(customer);
                return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
            }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateDto updatedCustomer)
        {
            var existingCustomer = await _customerService.GetByIdAsync(id);
            if (existingCustomer == null)
                return NotFound(new { message = "Müşteri bulunamadı." });

            updatedCustomer.Id = id; // ID'yi DTO'ya geçiriyoruz
            await _customerService.UpdateAsync(updatedCustomer);
            return Ok(new { message = "Müşteri güncellendi." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteAsync(id);
            return NoContent();
        }
    }
}