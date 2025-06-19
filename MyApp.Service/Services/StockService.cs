using Microsoft.EntityFrameworkCore;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppData.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MyAppService.Services
{
    public class StockService : IStockService
    {
        private readonly BilnexDbContext _context;
        public StockService(BilnexDbContext context) { _context = context; }
        public async Task<Stock> AddAsync(Stock stock)
        {
            try
            {
                _context.Stocks.Add(stock);
                await _context.SaveChangesAsync();
                return stock;
            }
            catch (DbUpdateException ex)

            {
                if (ex.InnerException?.Message.Contains("PRIMARY KEY") == true)
                {
                    throw new Exception("Bu ID'ye sahip ürün zaten mevcut.");
                }

                throw new Exception("Veritabanı hatası oluştu: " + ex.InnerException?.Message);
            }

        }
        public async Task DeleteAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                await _context.SaveChangesAsync();

            }

        }
        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }
        public async Task UpdateAsync(StockUpdateDto dto)
        {
            var stock = await _context.Stocks.FindAsync(dto.ID);
            if (stock == null)
                throw new Exception("Güncellenecek stok bulunamadı.");

            stock.Name = dto.Name;
            stock.Price = dto.Price;

            await _context.SaveChangesAsync();
        }
        public async Task<Stock> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }
        public async Task DeleteAllAsync()
        {
            var allStocks = await _context.Stocks.ToListAsync();
            _context.Stocks.RemoveRange(allStocks);
            await _context.SaveChangesAsync();

        }
    }
}
