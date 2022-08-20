using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Threading;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class WpfSplashScreen : ISplashScreen
    {
        private WpfSplashScreenWindow splashScreen;
        private Thread thread;
        private int? duration;
        //private ManualResetEvent closedSignal;
        private ManualResetEvent closingSignal;

        public WpfSplashScreen()
        {
        }

        //public event EventHandler Closing;

        public void Show(int? suggestedDuration, ManualResetEvent closingSignal)
        {
            if (this.splashScreen != null)
            {
                throw new ArgumentException("Splash screen is already showing.");
            }

            this.closingSignal = closingSignal;
            this.duration = suggestedDuration;
            //this.splashScreen = new WpfSplashScreenWindow(this.closingSignal);
            //this.splashScreen.ShowDuration = this.duration;
            //this.splashScreen.Show();
            this.thread = new Thread(this.ShowSplashScreen);
            this.thread.IsBackground = true;
            this.thread.SetApartmentState(ApartmentState.STA);
            this.thread.Start();
        }

        public void Close()
        {
            if (this.splashScreen != null)
            {
                //this.splashScreen.Close();
                this.splashScreen.Dispatcher.Invoke(new ParameterlessVoid(this.splashScreen.Close));
            }
        }

        private void ShowSplashScreen()
        {
            this.splashScreen = new WpfSplashScreenWindow(this.closingSignal);
            //this.splashScreen.Closing += this.HandleClosing;
            this.splashScreen.ShowDuration = this.duration;
            this.splashScreen.Show();

            System.Windows.Threading.Dispatcher.Run();
            //this.splashScreen.ShowDialog();
            //this.closingSignal.Set();
        }

        //private void HandleClosing(object sender, EventArgs e)
        //{
        //    this.OnClosing(EventArgs.Empty);
        //}

        //protected virtual void OnClosing(EventArgs e)
        //{
        //    EventHandler handler = this.Closing;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}
    }
}
