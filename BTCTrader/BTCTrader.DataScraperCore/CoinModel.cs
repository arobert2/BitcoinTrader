﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCTrader.DataScraperCore
{
    public class CoinModel
    {
        public DateTime Timestamp { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public CoinPrice Result { get; set; }

        public CoinModel()
        {
            Timestamp = DateTime.Now;
        }
    }

    public class CoinPrice
    {
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double Last { get; set; }
    }
}
