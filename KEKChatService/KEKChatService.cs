using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace KEKChatService
{
    public partial class KEKChatService : ServiceBase
    {
        public KEKChatService()
        {
            InitializeComponent();
        }

        private Timer _timer;
        private DateTime _lastRun = DateTime.Now.AddDays(-1);

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(10 * 60 * 1000); // every 10 minutes

            var handler = new System.Timers.ElapsedEventHandler(timer_Elapsed);

            _timer.Elapsed += handler;

            handler.Invoke(this, null);

            _timer.Start();
        }


        private async void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_lastRun.Date < DateTime.Now.Date)
            {
                _timer.Stop();

                string savepath = AppDomain.CurrentDomain.BaseDirectory + "..\\Memes\\";

                MemeScraper scraper = new MemeScraper(savepath, new string[] { "me_irl", "memes" }, 10);

                await scraper.GetMemesFromSubsAsync();

                _lastRun = DateTime.Now;
                _timer.Start();
            }
        }

        protected override void OnStop()
        {
        }
    }
}
