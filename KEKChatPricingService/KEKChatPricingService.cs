using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace KEKChatPricingService
{
    public partial class KEKChatPricingService : ServiceBase
    {
        private Timer timer;
        private DateTime lastRun = DateTime.Now.AddHours(-2);

        public KEKChatPricingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Break();

            timer = new Timer(2 * 60 * 1000); // every 10 minutes

            ElapsedEventHandler handler = new System.Timers.ElapsedEventHandler(timer_Elapsed);

            timer.Elapsed += handler;

            handler.Invoke(timer, null);

            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            DemandElasticity.ReevaluatePrices();    

            lastRun = DateTime.Now;
            timer.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
