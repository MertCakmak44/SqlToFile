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
    public class PurchaseService : IPurchaseService
    {
        private readonly BilnexDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchaseService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PurchaseService(BilnexDbContext context, IMapper mapper, ILogger<PurchaseService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Purchase> AddAsync(PurchaseCreateDto dto)
        {
            var stock = await _context.Stocks.FindAsync(dto.StockId);
            if (stock == null)
            {
                _logger.LogWarning("Alım yapılmak istenen ürün bulunamadı. StockId: {StockId}", dto.StockId);
                throw new Exception("Ürün bulunamadı.");
            }

            var purchase = _mapper.Map<Purchase>(dto);
            purchase.TotalCost = dto.Amount * stock.Price;
            purchase.PurchaseDate = DateTime.Now;
            stock.Amount += dto.Amount;

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            _logger.LogInformation("{Username} adlı kullanıcı yeni alım yaptı. Ürün: {ProductName}, Adet: {Quantity}, Toplam: {TotalCost}",
                username, stock.Name, dto.Amount, purchase.TotalCost);

            return purchase;
        }

        public async Task<List<Purchase>> GetAllAsync()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Stock)
                .ToListAsync();

            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            _logger.LogInformation("{Username} adlı kullanıcı tüm alım kayıtlarını listeledi. Toplam: {Count}", username, purchases.Count);

            return purchases;
        }
        public async Task DeleteAllAsync()
        {
            var allPurchases = await _context.Purchases.ToListAsync();
            _context.Purchases.RemoveRange(allPurchases);
            await _context.SaveChangesAsync();
        }

    }
}