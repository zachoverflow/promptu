using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Drawing;
using System.Reflection;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfNotifyIcon : INotifyIcon
    {
        private NotifyIcon notifyIcon;
        private WpfContextMenu wpfContextMenu;
        private UIContextMenu contextMenu;
        private NativeWindow nofityIconWindow;

        public WpfNotifyIcon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.MouseClick += this.HandleClick;

            this.contextMenu = new UIContextMenu();
            this.wpfContextMenu = (WpfContextMenu)this.contextMenu.NativeContextMenuInterface;
            this.wpfContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;

            FieldInfo windowField = notifyIcon.GetType().GetField("window", BindingFlags.NonPublic | BindingFlags.Instance);

            this.nofityIconWindow = (NativeWindow)windowField.GetValue(this.notifyIcon);
            //this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
        }

        public event ParameterlessVoid TimeToOpenPrompt;

        public string ToolTipText
        {
            get { return this.notifyIcon.Text; }
            set { this.notifyIcon.Text = value; }
        }

        public Icon Icon
        {
            get { return this.notifyIcon.Icon; }
            set { this.notifyIcon.Icon = value; }
        }

        //public INativeUICollection<IGenericMenuItem> ContextMenuItems
        //{
        //    get { return ((INativeContextMenu)this.wpfContextMenu).Items; }
        //}

        public UIContextMenu ContextMenu
        {
            get { return this.contextMenu; }
        }

        public void Show()
        {
            this.notifyIcon.Visible = true;
        }

        public void Hide()
        {
            this.notifyIcon.Visible = false;
        }

        private void HandleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.OnTimeToOpenPrompt();
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.ShowContextMenu();
            }
        }

        private void ShowContextMenu()
        {
            NativeMethods.SetForegroundWindow(this.nofityIconWindow.Handle);
            this.wpfContextMenu.IsOpen = true;
        }

        protected virtual void OnTimeToOpenPrompt()
        {
            ParameterlessVoid handler = TimeToOpenPrompt;
            if (handler != null)
            {
                handler();
            }
        }
    }
}
