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


namespace MyAppService.Services
{
    public class SaleService : ISaleService
    {
        private readonly BilnexDbContext _context;
        private readonly IMapper _mapper;
        public SaleService(BilnexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Sale> AddAsync(SaleCreateDto dto)
        {
            var stock =await _context.Stocks.FindAsync(dto.StockId);
            if (stock ==null || stock.Amount<dto.Amount)
            {
                throw new Exception("Yeterli stok yok.");
            }
            var sale = _mapper.Map<Sale>(dto);
            sale.TotalPrice = dto.Amount * stock.Price;
            sale.SaleDate = DateTime.Now;

            stock.Amount -= dto.Amount;
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }
        public async Task<List<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Stock)
                .ToListAsync();
        }
    }
}
