using Microsoft.EntityFrameworkCore;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppData.Context;
using AutoMapper;

namespace MyAppService.Services
{
    public class CustomerServices : ICustomerService
    {
        private readonly BilnexDbContext _context;
        private readonly IMapper _mapper;

        public CustomerServices(BilnexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("PRIMARY KEY") == true)
                    throw new Exception("Bu ID'ye sahip müşteri zaten mevcut.");

                throw new Exception("Veritabanı hatası oluştu: " + ex.InnerException?.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task UpdateAsync(CustomerUpdateDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.Id);
            if (customer == null)
                throw new Exception("Müşteri bulunamadı.");

            // DTO'dan mevcut varlık güncelleniyor
            _mapper.Map(dto, customer);

            // Güncelleme işlemi
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            var allCustomers = await _context.Customers.ToListAsync();
            _context.Customers.RemoveRange(allCustomers);
            await _context.SaveChangesAsync();
        }
    }
}
