﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppCore.Dtos
{
    public class StockUpdateDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }

    }
}
