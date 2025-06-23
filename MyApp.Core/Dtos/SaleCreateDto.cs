using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppCore.Dtos
{
    public class SaleCreateDto
    {
        public int CustomerId { get; set; }
        public int StockId { get; set; }
        public int Amount { get; set; }
    }
}
