using BTCTrader.DataScraper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BTCTrader.DataScraperService
{
    
    public class MarketScraper
    {
        private HttpClient HttpClient { get; set; } = new HttpClient();

        // Time trigger events
        public event EventHandler OneMinuteComplete;
        public event EventHandler ThreeMinuteComplete;
        public event EventHandler FiveMinuteComplete;
        public event EventHandler FifteenMinuteComplete;
        public event EventHandler ThirtyMinuteComplete;

        public MarketScraper()
        {  
        }

        /// <summary>
        /// Scrape the market
        /// </summary>
        /// <returns>Coin model of market</returns>
        public async Task<CoinModel> ScrapeMarket()
        {
            CoinModel cm = null;
            HttpResponseMessage resp = await HttpClient.GetAsync("https://bittrex.com/api/v1.1/public/getticker?market=" + Market);
            if (resp.IsSuccessStatusCode)
            {
                string r = await resp.Content.ReadAsStringAsync();
                cm = JsonConvert.DeserializeObject<CoinModel>(r);
            }

            return cm;
        }
    }
}
