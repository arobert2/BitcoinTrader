using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCTrader.DataScraper
{
    public class CoinIntervalModel
    {
        /// <summary>
        /// Interval to record in minutes
        /// </summary>
        public int Interval { get; set; }

        public double High { get; set; }
        public double Low { get; set; }
        public double LastHigh { get; set; }
        public double LastLow { get; set; }

        public double SevenDayAvg { get; set; }
        public double ThirtyDayAvg { get; set; }

        public DateTime StartDateTime { get; set; }
    }
}
