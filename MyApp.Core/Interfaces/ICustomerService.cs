using MyAppCore.Dtos;
using MyAppCore.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace MyAppCore.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer> AddAsync(Customer customer);
        Task DeleteAsync(int id);
        Task<Customer> GetByIdAsync(int id);
        Task UpdateAsync(CustomerUpdateDto customerDto);

    }
}
