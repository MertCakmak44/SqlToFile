using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppData.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAppService.Services
{
    public class StockService : IStockService
    {
        private readonly BilnexDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<StockService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StockService(BilnexDbContext context, IMapper mapper, ILogger<StockService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private string Username => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";

        public async Task<Stock> AddAsync(Stock stock)
        {
            try
            {
                _context.Stocks.Add(stock);
                await _context.SaveChangesAsync();
                _logger.LogInformation("{Username} adlı kullanıcı yeni stok ekledi: {StockName}, Fiyat: {Price}", Username, stock.Name, stock.Price);
                return stock;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "{Username} adlı kullanıcı stok ekleme hatası yaşadı.", Username);
                if (ex.InnerException?.Message.Contains("PRIMARY KEY") == true)
                    throw new Exception("Bu ID'ye sahip stok zaten mevcut.");
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
                _logger.LogInformation("{Username} adlı kullanıcı stok sildi: ID {StockId}", Username, id);
            }
            else
            {
                _logger.LogWarning("{Username} adlı kullanıcı silinmek istenen stok bulunamadı. ID: {StockId}", Username, id);
            }
        }

        public async Task<List<StockDto>> GetAllAsync()
        {
            var stocks = await _context.Stocks.ToListAsync();
            _logger.LogInformation("{Username} adlı kullanıcı tüm stokları getirdi. Toplam: {Count}", Username, stocks.Count);
            return _mapper.Map<List<StockDto>>(stocks);
        }

        public async Task<Stock> GetByIdAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
                _logger.LogWarning("{Username} adlı kullanıcı stok bulamadı. ID: {StockId}", Username, id);
            return stock;
        }

        public async Task UpdateAsync(StockUpdateDto dto)
        {
            var stock = await _context.Stocks.FindAsync(dto.ID);
            if (stock == null)
            {
                _logger.LogWarning("{Username} adlı kullanıcı güncellenmek istenen stok bulunamadı. ID: {StockId}", Username, dto.ID);
                throw new Exception("Stok bulunamadı.");
            }

            _mapper.Map(dto, stock);
            await _context.SaveChangesAsync();
            _logger.LogInformation("{Username} adlı kullanıcı stok güncelledi: {StockName}, Yeni fiyat: {Price}", Username, stock.Name, stock.Price);
        }

        public async Task UpdatePriceAsync(StockPriceUpdateDto dto)
        {
            var stock = await _context.Stocks.FindAsync(dto.StockId);
            if (stock == null)
            {
                _logger.LogWarning("{Username} adlı kullanıcı fiyat güncellemek istedi ama ürün bulunamadı. ID: {StockId}", Username, dto.StockId);
                throw new Exception("Ürün bulunamadı.");
            }

            var oldPrice = stock.Price;
            stock.Price = dto.NewPrice;
            await _context.SaveChangesAsync();
            _logger.LogInformation("{Username} adlı kullanıcı stok fiyatı güncelledi. Ürün: {Name}, Eski Fiyat: {Old}, Yeni Fiyat: {New}",
                Username, stock.Name, oldPrice, dto.NewPrice);
        }

        public async Task DeleteAllAsync()
        {
            var allStocks = await _context.Stocks.ToListAsync();
            _context.Stocks.RemoveRange(allStocks);
            await _context.SaveChangesAsync();
            _logger.LogCritical("{Username} adlı kullanıcı tüm stok kayıtlarını sildi. Toplam: {Count}", Username, allStocks.Count);
        }
    }
}