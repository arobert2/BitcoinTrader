using BTCTrader.DataScraper;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace BTCTrader.DataScraperService
{

    public class MarketScraper
    {
        private HttpClient HttpClient { get; set; } = new HttpClient();

        string Market { get; set; }

        public MarketScraper(string market)
        {
            Market = market;
        }

        /// <summary>
        /// Scrape the market
        /// </summary>
        /// <returns>Coin model of market</returns>
        public async Task<CoinModel> ScrapeMarket()
        {
            dynamic cm = null;
            var url = "https://bittrex.com/api/v1.1/public/getticker?market=" + Market;
            HttpResponseMessage resp = await HttpClient.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                string r = await resp.Content.ReadAsStringAsync();
                cm = JsonConvert.DeserializeObject<CoinModel>(r);
            }

            return cm;
        }
    }
}
