using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppCore.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

}
