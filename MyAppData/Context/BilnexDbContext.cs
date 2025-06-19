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
        public BilnexDbContext(DbContextOptions<BilnexDbContext> options) : base(options) { }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }









        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Customer>().Property(c => c.Id).HasColumnName("Id");
            modelBuilder.Entity<Customer>().Property(c => c.Name).HasColumnName("Name");
            modelBuilder.Entity<Customer>().Property(c => c.Sname).HasColumnName("Sname");

            modelBuilder.Entity<Stock>().ToTable("Stocks");
            modelBuilder.Entity<Stock>().Property(s => s.ID).HasColumnName("ID");
            modelBuilder.Entity<Stock>().Property(s => s.Name).HasColumnName("Name");
            modelBuilder.Entity<Stock>().Property(s => s.Price).HasColumnName("Price");

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("Id");
            modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("Username");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("Password");
        }
    }
}