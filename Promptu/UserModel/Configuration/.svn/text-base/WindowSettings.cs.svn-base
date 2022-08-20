using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;

namespace ZachJohnson.Promptu.UserModel.Configuration
{
    internal class WindowSettings<T> : GenericSettings where T : Form
    {
        private Point? location;
        private Size? size;
        private FormWindowState windowState = FormWindowState.Normal;

        public WindowSettings()
            : this(null)
        {
        }

        public WindowSettings(Point? location, Size? size, FormWindowState windowState)
        {
            this.location = location;
            this.size = size;
            this.windowState = windowState;
        }

        public WindowSettings(WindowSettings<T> baseSettings)
        {
            if (baseSettings != null)
            {
                this.location = baseSettings.Location;
                this.size = baseSettings.Size;
                this.windowState = baseSettings.WindowState;

            }
        }

        public Point? Location
        {
            get
            {
                return this.location;
            }

            set
            {
                if (this.location != value)
                {
                    this.location = value;
                    this.OnSettingChanged(EventArgs.Empty);
                }
            }
        }

        public FormWindowState WindowState
        {
            get
            {
                return this.windowState;
            }

            set
            {
                if (this.windowState != value)
                {
                    this.windowState = value;
                    this.OnSettingChanged(EventArgs.Empty);
                }
            }
        }

        public Size? Size
        {
            get
            {
                return this.size;
            }

            set
            {
                if (this.size != value)
                {
                    this.size = value;
                    this.OnSettingChanged(EventArgs.Empty);
                }
            }
        }

        public static WindowSettings<T> FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            int? x = null;
            int? y = null;
            int? width = null;
            int? height = null;
            FormWindowState windowState = FormWindowState.Normal;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "X":
                        x = Utilities.TryParseInt32(attribute.Value, x);
                        //try
                        //{
                        //    x = Convert.ToInt32(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}
                        //catch (OverflowException)
                        //{
                        //}

                        break;
                    case "Y":
                        y = Utilities.TryParseInt32(attribute.Value, y);
                        //try
                        //{
                        //    y = Convert.ToInt32(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}
                        //catch (OverflowException)
                        //{
                        //}

                        break;
                    case "WIDTH":
                        width = Utilities.TryParseInt32(attribute.Value, width);
                        //try
                        //{
                        //    width = Convert.ToInt32(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}
                        //catch (OverflowException)
                        //{
                        //}

                        break;
                    case "HEIGHT":
                        height = Utilities.TryParseInt32(attribute.Value, height);
                        //try
                        //{
                        //    height = Convert.ToInt32(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}
                        //catch (OverflowException)
                        //{
                        //}

                        break;
                    case "STATE":
                        try
                        {
                            windowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), attribute.Value);
                        }
                        catch (ArgumentException)
                        {
                        }

                        break;
                    default:
                        break;
                }
            }

            Point? location = null;
            Size? size = null;

            if (x != null && y != null)
            {
                location = new Point(x.Value, y.Value);
            }

            if (width != null && height != null)
            {
                size = new Size(width.Value, height.Value);
            }

            return new WindowSettings<T>(location, size, windowState);
        }

        public void ImpartTo(T form)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }
            
            if (this.location != null)
            {
                form.Location = this.location.Value;
            }

            if (this.size != null)
            {
                form.Size = this.size.Value;
            }

            form.WindowState = this.WindowState;

            this.ImpartToCore(form);
        }

        protected virtual void ImpartToCore(T form)
        {
        }

        public void UpdateFrom(T form)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }

            bool anythingChanged = false;

            if (this.location != form.Location)
            {
                this.location = form.Location;
                anythingChanged = true;
            }

            if (this.size != form.Size)
            {
                this.size = form.Size;
                anythingChanged = true;
            }

            if (this.windowState != form.WindowState)
            {
                this.windowState = form.WindowState;
                anythingChanged = true;
            }

            if (UpdateFromCore(form))
            {
                anythingChanged = true;
            }

            if (anythingChanged)
            {
                this.OnSettingChanged(EventArgs.Empty);
            }
        }

        protected virtual bool UpdateFromCore(T form)
        {
            return false;
        }

        protected override XmlNode ToXmlCore(string nodeName, XmlDocument document)
        {
            XmlNode node = document.CreateElement(nodeName);

            if (this.location != null)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("x", this.location.Value.X, document));
                node.Attributes.Append(XmlUtilities.CreateAttribute("y", this.location.Value.Y, document));
            }

            if (this.size != null)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("width", this.size.Value.Width, document));
                node.Attributes.Append(XmlUtilities.CreateAttribute("height", this.size.Value.Height, document));
            }

            node.Attributes.Append(XmlUtilities.CreateAttribute("state", this.windowState, document));

            return node;
        }
    }
}
