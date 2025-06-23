using MyAppCore.Dtos;
using MyAppCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAppCore.Interfaces
{
    public interface IStockService
    {
        Task<List<StockDto>> GetAllAsync(); // Doğru olan bu
        Task<Stock> GetByIdAsync(int id);
        Task<Stock> AddAsync(Stock stock);
        Task UpdateAsync(StockUpdateDto dto);
        Task UpdatePriceAsync(StockPriceUpdateDto dto);
        Task DeleteAsync(int id);
        Task DeleteAllAsync();
    }
}
