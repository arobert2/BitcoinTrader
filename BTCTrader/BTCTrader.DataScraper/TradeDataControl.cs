using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCTrader.DataScraper
{
    
    public class TradeDataControl
    {
        private static 

        public TradeDataControl()
        {
            if(Intervals != null)
            {
                
            }
        }

        public void Add(CoinModel cm)
        {
            lock (Intervals)
            {
                Intervals[MarketTimes.One].Add(cm);
                Intervals[MarketTimes.Three].Add(cm);
                Intervals[MarketTimes.Five].Add(cm);
                Intervals[MarketTimes.Fifteen].Add(cm);
                Intervals[MarketTimes.Thirty].Add(cm);
            }
        }

        private void Clear(MarketTimes mt)
        {
            lock(Intervals)
            {
                Intervals[mt] = new List<CoinModel>();
            }
        }

        public List<CoinModel> GetInterval(MarketTimes mt)
        {
            return Intervals[mt];
        }

        public IntervalData CompleteInterval(MarketTimes mt)
        {
            var interdata = new IntervalData();
            lock (Intervals)
            {
                interdata.DataPoints = Intervals[mt];
                Clear(mt);
            }
            interdata.Interval = mt;
            interdata.IntervalStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            return interdata;
        }
    }

    public class IntervalData
    {
        public DateTime IntervalStamp { get; set; }
        public MarketTimes Interval { get; set; }
        public List<CoinModel> DataPoints { get; set; }
    }
}
