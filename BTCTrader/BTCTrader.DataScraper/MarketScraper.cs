using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BTCTrader.DataScraper
{
    public enum MarketTimes{One, Three, Five, Fifteen, Thirty}
    public class MarketScraper
    {
        public string Market { get; set; }
        /// <summary>
        /// Index in minutes 1, 3, 5, 15, 30
        /// </summary>
        public Dictionary<MarketTimes, List<CoinModel>> Intervals { get; set; }

        private HttpClient HttpClient { get; set; } = new HttpClient();

        // Time trigger events
        public event EventHandler OneMinuteComplete;
        public event EventHandler ThreeMinuteComplete;
        public event EventHandler FiveMinuteComplete;
        public event EventHandler FifteenMinuteComplete;
        public event EventHandler ThirtyMinuteComplete;

        // Times in Miliseconds
        private const int TEN_SECONDS = 10000;
        private const int ONE_MINUTE = 60000;
        private const int THREE_MINUTES = ONE_MINUTE * 3;
        private const int FIVE_MINUTES = ONE_MINUTE * 5;
        private const int FIFTEEN_MINUTES = ONE_MINUTE * 15;
        private const int THIRTY_MINUTES = ONE_MINUTE * 30;


        public MarketScraper()
        {
            Intervals.Add(MarketTimes.One, new List<CoinModel>());
            Intervals.Add(MarketTimes.Three, new List<CoinModel>());
            Intervals.Add(MarketTimes.Five, new List<CoinModel>());
            Intervals.Add(MarketTimes.Fifteen, new List<CoinModel>());
            Intervals.Add(MarketTimes.Thirty, new List<CoinModel>());

            Timer scrapetimer = new Timer(TEN_SECONDS);
            scrapetimer.Elapsed += (e, args) => { ScrapeMarket(); };

            Timer OneMinuteTimer = new Timer(ONE_MINUTE);
            OneMinuteTimer.Elapsed += (o,args) => OneMinuteComplete?.Invoke(this, args);

            Timer ThreeMinuteTimer = new Timer(THREE_MINUTES);
            ThreeMinuteTimer.Elapsed += (o, args) => ThreeMinuteComplete?.Invoke(this, args);

            Timer FiveMinuteTimer = new Timer(FIVE_MINUTES);
            FiveMinuteTimer.Elapsed += (o, args) => FiveMinuteComplete?.Invoke(this, args);

            Timer FifteenMinuteTimer = new Timer(FIFTEEN_MINUTES);
            FifteenMinuteTimer.Elapsed += (o, args) => FifteenMinuteComplete?.Invoke(this, args);

            Timer ThirtyMinuteTimer = new Timer(THIRTY_MINUTES);
            ThirtyMinuteTimer.Elapsed += (o, args) => ThreeMinuteComplete?.Invoke(this, args);
        }

        /// <summary>
        /// Scrape the market
        /// </summary>
        /// <returns>Coin model of market</returns>
        public async void ScrapeMarket()
        {
            CoinModel cm = null;
            HttpResponseMessage resp = await HttpClient.GetAsync("https://bittrex.com/api/v1.1/public/getticker?market=" + Market);
            if (resp.IsSuccessStatusCode)
            {
                string r = await resp.Content.ReadAsStringAsync();
                cm = JsonConvert.DeserializeObject<CoinModel>(r);
            }

            if (cm != null)
            {
                Intervals[MarketTimes.One].Add(cm);
                Intervals[MarketTimes.Three].Add(cm);
                Intervals[MarketTimes.Five].Add(cm);
                Intervals[MarketTimes.Fifteen].Add(cm);
                Intervals[MarketTimes.Thirty].Add(cm);
            }
        }
    }
}
