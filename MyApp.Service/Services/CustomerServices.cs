using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppData.Context;

namespace MyAppService.Services
{
    public class CustomerServices : ICustomerService
    {
        private readonly BilnexDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerServices> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerServices(BilnexDbContext context, IMapper mapper, ILogger<CustomerServices> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
                _logger.LogInformation("{Username} adlı kullanıcı yeni müşteri ekledi: {Name} {Sname}", username, customer.Name, customer.Sname);

                return customer;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Müşteri eklenirken hata oluştu: {@Customer}", customer);

                if (ex.InnerException?.Message.Contains("PRIMARY KEY") == true)
                    throw new Exception("Bu ID'ye sahip müşteri zaten mevcut.");

                throw new Exception("Veritabanı hatası oluştu: " + ex.InnerException?.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                _logger.LogInformation("{Username} adlı kullanıcı müşteri sildi. ID: {Id}", username, id);
            }
            else
            {
                _logger.LogWarning("{Username} adlı kullanıcı silinmek istenen müşteri bulunamadı. ID: {Id}", username, id);
            }
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            _logger.LogInformation("{Username} adlı kullanıcı tüm müşterileri listeledi. Toplam: {Count}", username, customers.Count);
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";

            if (customer == null)
                _logger.LogWarning("{Username} adlı kullanıcı ID ile müşteri bulamadı. ID: {Id}", username, id);
            else
                _logger.LogInformation("{Username} adlı kullanıcı ID ile müşteri getirdi. ID: {Id}", username, id);

            return customer;
        }

        public async Task UpdateAsync(CustomerUpdateDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.Id);
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";

            if (customer == null)
            {
                _logger.LogWarning("{Username} adlı kullanıcı güncellenmek istenen müşteriyi bulamadı. ID: {Id}", username, dto.Id);
                throw new Exception("Müşteri bulunamadı.");
            }

            _mapper.Map(dto, customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation("{Username} adlı kullanıcı müşteri güncelledi: {@Customer}", username, customer);
        }

        public async Task DeleteAllAsync()
        {
            var allCustomers = await _context.Customers.ToListAsync();
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";

            _context.Customers.RemoveRange(allCustomers);
            await _context.SaveChangesAsync();
            _logger.LogInformation("{Username} adlı kullanıcı tüm müşteri kayıtlarını sildi.", username);
        }
    }
}
