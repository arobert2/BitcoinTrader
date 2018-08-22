using BTCTrader.DataScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCTrader.DataScraperService
{
    public class CoinIntervalModelBuilder
    {
        public CoinIntervalModelBuilder()
        {

        }

        public CoinIntervalModel Build(List<CoinModel> lcm, MarketTimes mt)
        {
            var cim = new CoinIntervalModel();
            cim.IntervalStamp = DateTime.Now;
            cim.Interval = (int)mt;
            cim.High = GetHigh(lcm);
            cim.Low = GetLow(lcm);
            cim.LastHigh = lcm[lcm.Count - 1].Result.Ask;
            if (lcm[lcm.Count - 1].Result.Bid >= cim.LastLow)
                cim.LastLow = lcm[lcm.Count - 1].Result.Bid;
            else
                cim.LastLow = cim.LastLow;
            cim.Average = GetAverage(lcm);
            return cim;

        }

        private double GetHigh(List<CoinModel> lcm)
        {
            double High = double.MinValue;
            foreach(var cm in lcm)
            {
                if (cm.Result.Last > High)
                    High = cm.Result.Last;
            }
            return High;
        }

        private double GetLow(List<CoinModel> lcm)
        {
            double Low = double.MaxValue;
            foreach(var cm in lcm)
            {
                if (cm.Result.Last < Low)
                    Low = cm.Result.Last;
            }
            return Low;
        }

        private double GetAverage(List<CoinModel> lcm)
        {
            double avg = 0;
            foreach (var cm in lcm)
                avg += cm.Result.Last;
            avg = avg / lcm.Count;
            return avg;
        }
    }
}
