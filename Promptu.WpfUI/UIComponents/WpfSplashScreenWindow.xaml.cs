using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Timers;

namespace ZachJohnson.Promptu.WpfUI
{
	/// <summary>
	/// Interaction logic for WpfSplashScreen.xaml
	/// </summary>
	internal partial class WpfSplashScreenWindow : Window
	{
        private Timer timer;
        private int? showDuration;
        private System.Threading.ManualResetEvent closingSignal;

		public WpfSplashScreenWindow(System.Threading.ManualResetEvent closingSignal)
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
            this.closingSignal = closingSignal;
		}

        public int? ShowDuration
        {
            get { return this.showDuration; }
            set { this.showDuration = value; }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            int? showDuration = this.showDuration;
            if (showDuration != null)
            {
                this.timer = new Timer(this.showDuration.Value);
                this.timer.Elapsed += HandleTimerFinished;
                this.timer.AutoReset = false;
                this.timer.Start();
            }

            this.Activate();
        }

        private void HandleTimerFinished(object sender, EventArgs e)
        {
            //this.Close();
            this.Dispatcher.BeginInvoke(new ParameterlessVoid(this.Close));
        }

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
            
        //    base.OnClosing(e);
        //}

        protected override void OnClosed(EventArgs e)
        {
            this.timer.Dispose();
            this.timer.Stop();
            this.timer.Elapsed -= this.HandleTimerFinished;
            this.closingSignal.Set();
            base.OnClosed(e);

            Dispatcher.InvokeShutdown();
        }
    }
}