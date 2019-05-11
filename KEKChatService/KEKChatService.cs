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

        private Timer timer1;
        private Timer timer2;
        private Timer timer3;
        private DateTime lastRun1 = DateTime.Now.AddDays(-1);
        private DateTime lastRun2 = DateTime.Now.AddDays(-1);
        private DateTime lastRun3 = DateTime.Now.AddDays(-1);

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer(10 * 60 * 1000); // every 10 minutes

            ElapsedEventHandler handler1 = new System.Timers.ElapsedEventHandler(timer1_Elapsed);

            timer1.Elapsed += handler1;

            // TODO: Find why service timeouts when memes are downloaded for >1 time
            handler1.Invoke(timer1, null);

            timer1.Start();

            timer3 = new Timer(2 * 60 * 1000); // every 2 minutes

            ElapsedEventHandler handler3 = new System.Timers.ElapsedEventHandler(timer3_Elapsed);

            timer3.Elapsed += handler3;

            handler3.Invoke(timer3, null);

            timer3.Start();

            timer2 = new Timer(10 * 60 * 1000); // every 10 minutes

            ElapsedEventHandler handler2 = new System.Timers.ElapsedEventHandler(timer2_Elapsed);

            timer2.Elapsed += handler2;

            handler2.Invoke(timer2, null);

            timer2.Start();
        }

        private void timer3_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer3.Stop();

            DemandElasticity.ReevaluatePrices();

            lastRun3 = DateTime.Now;
            timer3.Start();
        }

        private async void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (lastRun1.Date < DateTime.Now.Date)
            {
                timer1.Stop();

                string savepath = AppDomain.CurrentDomain.BaseDirectory + "..\\Memes\\";

                MemeScraper scraper = new MemeScraper(savepath, new string[] { "me_irl", "memes" }, 10);

                await scraper.GetMemesFromSubsAsync();

                lastRun1 = DateTime.Now;
                timer1.Start();
            }
        }

        private void timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now - lastRun2 > TimeSpan.FromHours(2))
            {
                timer2.Stop();

                OrderOrganizer.Organize();

                lastRun2 = DateTime.Now;
                timer2.Start();
            }
        }

        protected override void OnStop()
        {
        }
    }
}
