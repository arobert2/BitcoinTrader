using BTCTrader.DataScraper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BTCTrader.DataScraperService
{
    public partial class CryptoScannerService : ServiceBase
    {
        EventLog Log = new EventLog();

        const string SOURCE     =   "CryptoScanner";
        const string LOG_NAME   =   "ScrapperLogger";

        private MarketScraper Scraper = new MarketScraper();

        public CryptoScannerService()
        {
            InitializeComponent();
            Log = new EventLog();
            if (!EventLog.SourceExists(SOURCE))
            {
                EventLog.CreateEventSource(SOURCE, LOG_NAME);
            }
            Log.Source = SOURCE;
            Log.Log = LOG_NAME;
        }

        protected override void OnStart(string[] args)
        {
            Log.WriteEntry("CryptoScanner Started");

        }

        protected override void OnStop()
        {
            Log.WriteEntry("CryptoScanner Stopped");
        }
    }
}
