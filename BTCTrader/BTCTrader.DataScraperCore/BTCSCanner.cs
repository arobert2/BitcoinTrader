using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace BTCTrader.DataScraperCore
{
    public class MarketScanner
    {
        /// <summary>
        /// Interval in minutes
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// URL API to query
        /// </summary>
        public string MarketURL { get; set; }
        /// <summary>
        /// Event triggered when a scan is complete
        /// </summary>
        public event EventHandler ScanComplete;
        /// <summary>
        /// Create a market scanner set to query a market per interval
        /// </summary>
        /// <param name="interval">Interval in minutes</param>
        /// <param name="marketurl">URL to query from</param>
        public MarketScanner(int interval, string marketurl)
        {
            Interval = interval;
            MarketURL = marketurl;

            int pause = Math.Abs(DateTime.Now.Minute % Interval - Interval) * 60 * 1000 + Math.Abs(DateTime.Now.Second - 60) * 1000;
            Thread.Sleep(pause * 1000);

            System.Timers.Timer timer = new System.Timers.Timer(interval * 60 * 1000);
            timer.Elapsed += (o,args) => ScanMarket();
            timer.Enabled = true;

        }
        /// <summary>
        /// Scan the market
        /// </summary>
        private async void ScanMarket()
        {
            var url = MarketURL;
            var httpClient = new HttpClient();
            var resp = await httpClient.GetAsync(url);
            dynamic coinModel;
            if(resp.IsSuccessStatusCode)
            {
                var jsondata = await resp.Content.ReadAsStringAsync();
                coinModel = JsonConvert.DeserializeObject<CoinModel>(jsondata);
                ScanComplete?.Invoke(this, new MarketScannerEventArgs(coinModel));
            }
        }

    }

    public class MarketScannerEventArgs : EventArgs
    {
        public CoinModel ScanData { get; set; }

        public MarketScannerEventArgs (CoinModel cm) : base()
        {
            ScanData = cm;
        }
    }
}
