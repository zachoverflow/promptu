using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Configuration;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Xml;
using ZachJohnson.Promptu.WpfUI.UIComponents;
using ZachJohnson.Promptu.Configuration;
using System.Globalization;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    public class WindowSettings<T> : ObjectSettings<T>
    {
        private NativeMethods.WINDOWPLACEMENT? placement;

        public WindowSettings()
            : this(null)
        {
        }

        public WindowSettings(NativeMethods.WINDOWPLACEMENT? placement)
        {
            this.placement = placement;
        }

        protected override void ImpartToCore(T obj)
        {
            PromptuWindow window = obj as PromptuWindow;
            if (window == null)
            {
                throw new ArgumentException("'obj' does not derive from PromptuWindow");
            }

            if (!window.IsSourceInitialized)
            {
                window.SourceInitialized += this.ImpartToLatent;
                return;
            }

            NativeMethods.WINDOWPLACEMENT? placement = this.placement;

            if (placement != null)
            {
                NativeMethods.WINDOWPLACEMENT wp = placement.Value;
                wp.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
                wp.flags = 0;
                wp.showCmd = (wp.showCmd == NativeMethods.SW_SHOWMINIMIZED ? NativeMethods.SW_SHOWNORMAL : wp.showCmd);
                IntPtr hwnd = new WindowInteropHelper(window).Handle;
                NativeMethods.SetWindowPlacement(hwnd, ref wp);
            }
        }

        protected override void UpdateFromCore(T obj, ref bool anythingChanged)
        {
            PromptuWindow window = obj as PromptuWindow;
            if (window == null)
            {
                throw new ArgumentException("Window is not a PromptuWindow.");
            }

            //NativeMethods.WINDOWPLACEMENT wp = new NativeMethods.WINDOWPLACEMENT();
            //IntPtr hwnd = new WindowInteropHelper(window).Handle;
            //if (hwnd == IntPtr.Zero)
            //{
            //    return;
            //}

            //NativeMethods.GetWindowPlacement(hwnd, out wp);

            NativeMethods.WINDOWPLACEMENT? closingPlacement = window.WindowPlacement;
            if (closingPlacement != null)
            {
                this.placement = closingPlacement;
                anythingChanged = true;
            }
        }

        protected override void ToXmlCore(XmlNode node)
        {
            NativeMethods.WINDOWPLACEMENT? placement = this.placement;

            if (placement == null)
            {
                return;
            }

            XmlUtilities.AppendAttribute(node, "state", placement.Value.showCmd);

            XmlUtilities.AppendAttribute(node, "minX", placement.Value.minPosition.x);
            XmlUtilities.AppendAttribute(node, "minY", placement.Value.minPosition.y);

            XmlUtilities.AppendAttribute(node, "maxX", placement.Value.maxPosition.x);
            XmlUtilities.AppendAttribute(node, "maxY", placement.Value.maxPosition.y);

            XmlUtilities.AppendAttribute(node, "normalLeft", placement.Value.normalPosition.Left);
            XmlUtilities.AppendAttribute(node, "normalTop", placement.Value.normalPosition.Top);
            XmlUtilities.AppendAttribute(node, "normalRight", placement.Value.normalPosition.Right);
            XmlUtilities.AppendAttribute(node, "normalBottom", placement.Value.normalPosition.Bottom);
        }

        protected override void UpdateFromCore(XmlNode node)
        {
            int? showCmd = null;
            int? minX = null;
            int? minY = null;
            int? maxX = null;
            int? maxY = null;
            int? normalLeft = null; 
            int? normalTop = null;
            int? normalRight = null;
            int? normalBottom = null;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "STATE":
                        showCmd = TryParse(attribute.Value);
                        break;
                    case "MINX":
                        minX = TryParse(attribute.Value);
                        break;
                    case "MINY":
                        minY = TryParse(attribute.Value);
                        break;
                    case "MAXX":
                        maxX = TryParse(attribute.Value);
                        break;
                    case "MAXY":
                        maxY = TryParse(attribute.Value);
                        break;
                    case "NORMALLEFT":
                        normalLeft = TryParse(attribute.Value);
                        break;
                    case "NORMALTOP":
                        normalTop = TryParse(attribute.Value);
                        break;
                    case "NORMALRIGHT":
                        normalRight = TryParse(attribute.Value);
                        break;
                    case "NORMALBOTTOM":
                        normalBottom = TryParse(attribute.Value);
                        break;
                    default:
                        break;
                }
            }

            NativeMethods.WINDOWPLACEMENT? placement = null;

            if (showCmd != null && 
                minX != null && 
                minY != null && 
                maxX != null && 
                maxY != null &&
                normalLeft != null &&
                normalTop != null && 
                normalRight != null && 
                normalBottom != null)
            {
                NativeMethods.WINDOWPLACEMENT newPlacement = new NativeMethods.WINDOWPLACEMENT();
                newPlacement.showCmd = showCmd.Value;

                NativeMethods.POINT minPos = new NativeMethods.POINT();
                minPos.x = minX.Value;
                minPos.y = minY.Value;
                newPlacement.minPosition = minPos;

                NativeMethods.POINT maxPos = new NativeMethods.POINT();
                maxPos.x = maxX.Value;
                maxPos.y = maxY.Value;
                newPlacement.maxPosition = maxPos;

                NativeMethods.RECT normalPos = new NativeMethods.RECT();
                normalPos.Left = normalLeft.Value;
                normalPos.Top = normalTop.Value;
                normalPos.Right = normalRight.Value;
                normalPos.Bottom = normalBottom.Value;
                newPlacement.normalPosition = normalPos;

                placement = newPlacement;
            }

            this.placement = placement;
        }

        private int? TryParse(string value)
        {
            try
            {
                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            try
            {
                return Convert.ToInt32(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return null;
        }

        private void ImpartToLatent(object sender, EventArgs e)
        {
            this.ImpartTo((T)sender);
        }
    }
}
