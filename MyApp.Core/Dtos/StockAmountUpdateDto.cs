using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppCore.Dtos
{
    public class StockAmountUpdateDto
    {
        public int Id { get; set; }
        public int Added { get; set; }
        public int Removed { get; set; }
    }
}
