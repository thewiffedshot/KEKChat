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

        private Timer _timer1;
        private Timer _timer2;
        private DateTime _lastRun1 = DateTime.Now.AddDays(-1);
        private DateTime _lastRun2 = DateTime.Now.AddDays(-1);

        protected override void OnStart(string[] args)
        {
            _timer1 = new Timer(10 * 60 * 1000); // every 10 minutes

            ElapsedEventHandler handler1 = new System.Timers.ElapsedEventHandler(timer1_Elapsed);

            _timer1.Elapsed += handler1;

            // TODO: Find why service timeouts when memes are downloaded for >1 time
            //handler1.Invoke(_timer1, null);

            _timer1.Start();


            _timer2 = new Timer(10 * 60 * 1000); // every 10 minutes

            ElapsedEventHandler handler2 = new System.Timers.ElapsedEventHandler(timer2_Elapsed);

            _timer2.Elapsed += handler2;

            handler2.Invoke(_timer1, null);

            _timer2.Start();
        }


        private async void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_lastRun1.Date < DateTime.Now.Date)
            {
                _timer1.Stop();

                string savepath = AppDomain.CurrentDomain.BaseDirectory + "..\\Memes\\";

                MemeScraper scraper = new MemeScraper(savepath, new string[] { "me_irl", "memes" }, 10);

                await scraper.GetMemesFromSubsAsync();

                _lastRun1 = DateTime.Now;
                _timer1.Start();
            }
        }

        private void timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Date - _lastRun2.Date > TimeSpan.FromHours(2))
            {
                _timer2.Stop();

                OrderOrganizer.Organize();

                _lastRun2 = DateTime.Now;
                _timer2.Start();
            }
        }

        protected override void OnStop()
        {
        }
    }
}
