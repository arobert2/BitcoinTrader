using BTCTrader.DataScraper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTCTrader.DataScraperService
{
    public partial class CryptoScannerService : ServiceBase
    {
        EventLog Log = new EventLog();

        const string SOURCE     =   "CryptoScanner";
        const string LOG_NAME   =   "ScrapperLogger";

        private event EventHandler StartController;
        private event EventHandler StopController;

        private MarketController marketController;

        public CryptoScannerService()
        {
            //Markets
            marketController = new MarketController("btc-bch");

            InitializeComponent();
            Log = new EventLog();
            if (!EventLog.SourceExists(SOURCE))
            {
                EventLog.CreateEventSource(SOURCE, LOG_NAME);
            }
            Log.Source = SOURCE;
            Log.Log = LOG_NAME;
            StartController += (o,args) => marketController.Start();
            StopController += (o, args) => marketController.Stop();
        }

        protected override void OnStart(string[] args)
        {
            Log.WriteEntry("CryptoScanner Started");
            var seconds = -DateTime.Now.Second + 60;
            Log.WriteEntry("Synchonizing Time");
            Thread.Sleep(seconds * 1000);
            StartController.Invoke(this, new EventArgs());
        }

        protected override void OnStop()
        {
            Log.WriteEntry("CryptoScanner Stopped");
            StopController.Invoke(this, new EventArgs());
        }
    }
}
