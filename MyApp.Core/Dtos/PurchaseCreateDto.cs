using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppCore.Dtos
{
    public class PurchaseCreateDto
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
    }

}
