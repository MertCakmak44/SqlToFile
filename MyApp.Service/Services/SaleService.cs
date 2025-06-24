using MyAppData.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppCore.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace MyAppService.Services
{
    public class SaleService : ISaleService
    {
        private readonly BilnexDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SaleService(BilnexDbContext context, IMapper mapper, ILogger<SaleService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Sale> AddAsync(SaleCreateDto dto)
        {
            var stock = await _context.Stocks.FindAsync(dto.StockId);
            if (stock == null)
            {
                _logger.LogWarning("Satış işlemi başarısız: Ürün bulunamadı. StockId: {StockId}", dto.StockId);
                throw new Exception("Ürün bulunamadı.");
            }

            if (stock.Amount < dto.Amount)
            {
                _logger.LogWarning("Yetersiz stok: {StockName} için istenen: {Requested}, mevcut: {Available}",
                    stock.Name, dto.Amount, stock.Amount);
                throw new Exception("Yeterli stok yok.");
            }

            var sale = _mapper.Map<Sale>(dto);
            sale.TotalPrice = dto.Amount * stock.Price;
            sale.SaleDate = DateTime.Now;
            stock.Amount -= dto.Amount;

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            _logger.LogInformation("{Username} adlı kullanıcı satış kaydı oluşturdu. Ürün: {Product}, MüşteriId: {CustomerId}, Adet: {Amount}, Toplam: {TotalPrice}",
                username, stock.Name, dto.CustomerId, dto.Amount, sale.TotalPrice);

            return sale;
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            var sales = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Stock)
                .ToListAsync();

            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            _logger.LogInformation("{Username} adlı kullanıcı tüm satış kayıtlarını listeledi. Toplam kayıt: {Count}", username, sales.Count);

            return sales;
        }
    }
}