using Microsoft.EntityFrameworkCore;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppData.Context;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;


namespace MyAppService.Services
{
    public class CustomerServices : ICustomerService
    {
        
        private readonly BilnexDbContext _context;
        public CustomerServices(BilnexDbContext context)
        {
            _context = context;

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
                {
                    throw new Exception("Bu ID'ye sahip müşteri zaten mevcut.");
                }

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
        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();

        }
        public async Task UpdateAsync(CustomerUpdateDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.Id);
            if (customer == null)
                throw new Exception("Müşteri bulunamadı.");

            customer.Name = dto.Name;
            customer.Sname = dto.Sname;

            await _context.SaveChangesAsync();
        }
        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }
    }
}
