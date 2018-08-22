using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BTCTrader.DataScraperService
{
    static class Program
    {
        public static bool stop = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var css = new CryptoScannerService();
                css.StopController += (o, e) => { stop = true; };
                css.StartController += (o, e) => { stop = false; };
                css.TestStart(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[1];
                var ccs = new CryptoScannerService();
                ccs.StartController += (o, e) => { stop = false; };
                ccs.StopController += (o, e) => { stop = true; };
                ServicesToRun[0] = ccs;
                ServiceBase.Run(ServicesToRun);
            }


            while (!stop) ;
        }
    }
}
