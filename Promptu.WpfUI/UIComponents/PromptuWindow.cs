using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    public class PromptuWindow : Window
    {
        private NativeMethods.WINDOWPLACEMENT? closingWindowPlacement;

        public PromptuWindow()
        {
        }

        public bool IsSourceInitialized
        {
            get;
            private set;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            this.IsSourceInitialized = true;
            base.OnSourceInitialized(e);
        }

        public NativeMethods.WINDOWPLACEMENT? WindowPlacement
        {
            get 
            {
                NativeMethods.WINDOWPLACEMENT? placement = this.closingWindowPlacement;
                if (placement == null)
                {
                    NativeMethods.WINDOWPLACEMENT wp = new NativeMethods.WINDOWPLACEMENT();
                    IntPtr hwnd = new WindowInteropHelper(this).Handle;
                    if (hwnd == IntPtr.Zero)
                    {
                        return null;
                    }

                    NativeMethods.GetWindowPlacement(hwnd, out wp);

                    placement = wp;
                }

                return placement;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            NativeMethods.WINDOWPLACEMENT wp = new NativeMethods.WINDOWPLACEMENT();
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.GetWindowPlacement(hwnd, out wp);

            this.closingWindowPlacement = wp;
        }
    }
}
