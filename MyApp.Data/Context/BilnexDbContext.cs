using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAppCore.Entities;

namespace MyAppData.Context
{
    public class BilnexDbContext : DbContext
    {
        public BilnexDbContext(DbContextOptions<BilnexDbContext> options): base(options) { }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
} 
