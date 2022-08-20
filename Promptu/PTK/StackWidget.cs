using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.PTK
{
    internal class StackWidget : WidgetCollectionWidget<IStackWidget>
    {
        private Orientation orientation;
        private ScaledQuantity? sizeContraOrientation;

        public StackWidget(string id, Orientation orientation)
            : base(id, Globals.GuiManager.ToolkitHost.Factory.ConstructStackWidget())
        {
            this.Orientation = orientation;
        }

        public event EventHandler OrientationChanged;

        public Orientation Orientation
        {
            get 
            { 
                return this.orientation; 
            }

            set 
            {
                if (this.Orientation != value)
                {
                    this.orientation = value;
                    this.NativeInterface.SetOrientation(value);
                    this.OnOrientationChanged(EventArgs.Empty);
                }
            }
        }

        public ScaledQuantity? SizeContraOrientation
        {
            get
            {
                return this.sizeContraOrientation;
            }

            set
            {
                this.sizeContraOrientation = value;
                if (value == null)
                {
                    this.NativeInterface.PhysicalSizeContraOrientation = null;
                }
                else
                {
                    this.NativeInterface.PhysicalSizeContraOrientation = (int)Globals.GuiManager.ToolkitHost.ConvertToPhysicalQuantity(value.Value);
                }
            }
        }

        public void SetAutoSizeBasedOn(int index, bool value)
        {
            this.NativeInterface.SetAutoSizeBasedOn(index, value);
        }

        protected virtual void OnOrientationChanged(EventArgs e)
        {
            EventHandler handler = this.OrientationChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
