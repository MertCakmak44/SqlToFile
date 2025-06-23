using MyAppCore.Dtos;
using MyAppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppCore.Interfaces
{
    public interface ISaleService
    {
        Task<Sale> AddAsync(SaleCreateDto dto);
        Task<List<Sale>> GetAllAsync();

    }
}
