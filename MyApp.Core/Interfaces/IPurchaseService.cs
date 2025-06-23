using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAppCore.Entities;
using MyAppCore.Dtos;


namespace MyAppCore.Interfaces
{
    public interface IPurchaseService
    {
        Task<Purchase> AddAsync(PurchaseCreateDto dto);
        Task<List<Purchase>> GetAllAsync();


    }
}
