using Microsoft.Identity.Client;
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
    public interface IStockService
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock> AddAsync(Stock stock);
        Task DeleteAsync(int id);
        Task<Stock> GetByIdAsync(int id);
        Task UpdateAsync(StockUpdateDto stockDto);
        Task DeleteAllAsync();




    }
}
