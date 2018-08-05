using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BTCTrader.DataScraper
{
    public class MarketScraper
    {
        public string Market { get; set; }

        private HttpClient HttpClient { get; set; } = new HttpClient();
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
