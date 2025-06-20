using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppCore.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public int Amount { get; set; }
        public decimal TotalPrice { get; set; }
        // Navigation properties
        public Stock Stock { get; set; }
        public Customer Customer { get; set; }
        }
}
