using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppData.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace MyAppService.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly BilnexDbContext _context;
        private readonly IMapper _mapper;
        public PurchaseService(BilnexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Purchase> AddAsync(PurchaseCreateDto dto)
        {
            var stock = await _context.Stocks.FindAsync(dto.StockId);
            if (stock == null)
            {
                throw new Exception("Ürün bulunamadı.");
            }
            var purchase = _mapper.Map<Purchase>(dto);
            purchase.TotalCost = dto.Quantity * stock.Price;
            purchase.PurchaseDate = DateTime.Now;
            stock.Amount += dto.Quantity;
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return purchase;
        }
        public async Task<List<Purchase>> GetAllAsync()
        {
            return await _context.Purchases
                .Include(p => p.Stock)
                .ToListAsync();
        }
    }
}
