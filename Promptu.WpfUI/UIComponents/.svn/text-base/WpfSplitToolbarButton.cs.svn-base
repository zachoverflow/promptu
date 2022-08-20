using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfSplitToolbarButton : Button, ISplitToolbarButton, IMenuButton
    {
        private const string ArrowElementName = "arrowElement";
        private UIElement arrowElement;
        private bool mouseIsOverArrowElement;
        private static readonly DependencyProperty contextMenuIsOpen = DependencyProperty.Register("ContextMenuIsOpen", typeof(bool), typeof(WpfSplitToolbarButton));
        private bool isFullButton;

        public WpfSplitToolbarButton()
        {
        }

        public event EventHandler ButtonClick;

        public string ToolTipText
        {
            get { return (string)this.ToolTip; }
            set { this.ToolTip = value; }
        }

        public bool Available
        {
            get
            {
                return this.Visibility == System.Windows.Visibility.Visible;
            }
            set
            {
                this.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.IsEnabled;
            }
            set
            {
                this.IsEnabled = value;
            }
        }

        public override void OnApplyTemplate()
        {
            if (this.arrowElement != null)
            {
                this.arrowElement.MouseEnter -= this.HandleArrowElementMouseEnter;
                this.arrowElement.MouseLeave -= this.HandleArrowElementMouseLeave;
            }

            base.OnApplyTemplate();

            this.arrowElement = (UIElement)GetTemplateChild(ArrowElementName);
            this.arrowElement.MouseEnter += this.HandleArrowElementMouseEnter;
            this.arrowElement.MouseLeave += this.HandleArrowElementMouseLeave;
        }

        public bool IsFullButton
        {
            get { return this.isFullButton; }
            set { this.isFullButton = value; }
        }

        protected override void OnClick()
        {
            if (this.mouseIsOverArrowElement || this.isFullButton)
            {
                this.OpenContextMenu();
            }
            else
            {
                this.OnButtonClick(EventArgs.Empty);
                base.OnClick();
            }
        }

        public bool ContextMenuIsOpen
        {
            get
            {
                return (bool)this.GetValue(contextMenuIsOpen);
            }

            private set
            {
                this.SetValue(contextMenuIsOpen, value);
            }
        }

        private void OpenContextMenu()
        {
            if (this.ContextMenu != null && this.ContextMenu.HasItems)
            {
                this.ContextMenu.VerticalOffset = 0;
                this.ContextMenu.HorizontalOffset = 0;

                this.ContextMenu.Opened += this.HandleContextMenuOpened;
                this.ContextMenu.Closed += this.HandleContextMenuClosed;
                this.ContextMenu.IsOpen = true;
            }
        }

        private void HandleContextMenuOpened(object sender, EventArgs e)
        {
            this.ContextMenuIsOpen = true;
            ContextMenu contextMenu = (ContextMenu)sender;
            contextMenu.Opened -= this.HandleContextMenuOpened;

            Point pointToOpenAt = this.TranslatePoint(new Point(this.ActualWidth, 0), contextMenu);

            contextMenu.HorizontalOffset = pointToOpenAt.X;
            contextMenu.VerticalOffset = pointToOpenAt.Y;
        }

        private void HandleContextMenuClosed(object sender, EventArgs e)
        {
            this.ContextMenuIsOpen = false;
            ((ContextMenu)sender).Closed -= this.HandleContextMenuClosed;
            this.Focus();
        }

        private void HandleArrowElementMouseEnter(object sender, EventArgs e)
        {
            this.mouseIsOverArrowElement = true;
        }

        private void HandleArrowElementMouseLeave(object sender, EventArgs e)
        {
            this.mouseIsOverArrowElement = false;
        }

        protected virtual void OnButtonClick(EventArgs e)
        {
            EventHandler handler = this.ButtonClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
