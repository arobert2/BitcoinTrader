using BTCTrader.DataScraper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BTCTrader.DataScraperService
{
    public enum MarketTimes { One, Three, Five, Fifteen, Thirty }
    public class MarketController
    {
        /// <summary>
        /// Market string.
        /// </summary>
        public string Market { get; set; }

        /// <summary>
        /// Market Data
        /// </summary>
        private Dictionary<MarketTimes, List<CoinModel>> Intervals { get; set; } =
        new Dictionary<MarketTimes, List<CoinModel>>()
        {
            { MarketTimes.One, new List<CoinModel>() },
            { MarketTimes.Three, new List<CoinModel>() },
            { MarketTimes.Five, new List<CoinModel>() },
            { MarketTimes.Fifteen, new List<CoinModel>() },
            { MarketTimes.Thirty, new List<CoinModel>() }
        };

        // Times in Miliseconds
        private const int TEN_SECONDS = 10000;
        private const int ONE_MINUTE = 60000;
        private const int THREE_MINUTES = ONE_MINUTE * 3;
        private const int FIVE_MINUTES = ONE_MINUTE * 5;
        private const int FIFTEEN_MINUTES = ONE_MINUTE * 15;
        private const int THIRTY_MINUTES = ONE_MINUTE * 30;

        // Utilities
        private MarketScraper scraper;
        private CoinIntervalModelBuilder cimBuilder;
        private DatabaseWriter dbWriter;

        // Interval timers
        private Timer scrapetimer = new Timer(TEN_SECONDS);
        private Timer OneMinuteTimer = new Timer(ONE_MINUTE);
        private Timer ThreeMinuteTimer = new Timer(THREE_MINUTES);
        private Timer FiveMinuteTimer = new Timer(FIVE_MINUTES);
        private Timer FifteenMinuteTimer = new Timer(FIFTEEN_MINUTES);
        private Timer ThirtyMinuteTimer = new Timer(THIRTY_MINUTES);

        public MarketController(string market)
        {
            Market = market;
            scraper = new MarketScraper(Market);
            cimBuilder = new CoinIntervalModelBuilder();
            dbWriter = new DatabaseWriter(ConfigurationManager.AppSettings["ConnectionString"], Market);

            scrapetimer.Elapsed += async (e, args) => {
                var sr = scraper.ScrapeMarket().ContinueWith(res => Add(res.Result));
                //if (sr != null)
                    //Add(sr);
            };
            OneMinuteTimer.Elapsed += (e, args) => CompleteInterval(MarketTimes.One);
            ThreeMinuteTimer.Elapsed += (e, args) => CompleteInterval(MarketTimes.Three);
            FiveMinuteTimer.Elapsed += (e, args) => CompleteInterval(MarketTimes.Five);
            FifteenMinuteTimer.Elapsed += (e, args) => CompleteInterval(MarketTimes.Fifteen);
            ThirtyMinuteTimer.Elapsed += (e, args) => CompleteInterval(MarketTimes.Thirty);
        }

        private void CompleteInterval(MarketTimes mt)
        {
            var lcm = Intervals[mt];
            var coinInt = cimBuilder.Build(lcm, mt);
            dbWriter.Create(coinInt, mt);
            Clear(mt);
        }

        /// <summary>
        /// Add a CoinModel data point to an Interval List
        /// </summary>
        /// <param name="cm">CoinModel</param>
        public bool Add(CoinModel cm)
        {
            lock (Intervals)
            {
                Intervals[MarketTimes.One].Add(cm);
                Intervals[MarketTimes.Three].Add(cm);
                Intervals[MarketTimes.Five].Add(cm);
                Intervals[MarketTimes.Fifteen].Add(cm);
                Intervals[MarketTimes.Thirty].Add(cm);
            }

            return true;
        }

        public void Clear(MarketTimes mt)
        {
            lock(Intervals)
            {
                Intervals[mt].Clear();
            }
        }

        /// <summary>
        /// Start the timers
        /// </summary>
        public void Start()
        {
            scrapetimer.Start();
            OneMinuteTimer.Start();
            ThreeMinuteTimer.Start();
            FiveMinuteTimer.Start();
            FifteenMinuteTimer.Start();
            ThirtyMinuteTimer.Start();
        }

        /// <summary>
        /// Stop the timers
        /// </summary>
        public void Stop()
        {
            scrapetimer.Stop();
            OneMinuteTimer.Stop();
            ThreeMinuteTimer.Stop();
            FiveMinuteTimer.Stop();
            FifteenMinuteTimer.Stop();
            ThirtyMinuteTimer.Stop();
        }
    }
}
