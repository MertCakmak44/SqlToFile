using MyAppCore.Dtos;
using MyAppCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAppCore.Interfaces
{
    public interface ISaleService
    {
        Task<Sale> AddAsync(SaleCreateDto dto);
        Task<List<Sale>> GetAllAsync();  // eski kullanım
        Task DeleteAllAsync();
        Task<List<SaleDto>> GetAllDetailedAsync(); // yeni DTO ile listeleme
    }
}
