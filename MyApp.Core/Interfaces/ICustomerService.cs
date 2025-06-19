using MyAppCore.Dtos;
using MyAppCore.Entities;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllAsync();
    Task<Customer> AddAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<Customer> GetByIdAsync(int id);
    Task UpdateAsync(CustomerUpdateDto customerDto);
    Task DeleteAllAsync();
}
