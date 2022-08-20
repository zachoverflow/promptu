//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.UI;
//using System.Windows.Forms;
//using System.Xml;
//using System.Drawing;
//using ZachJohnson.Promptu.SkinApi;
//using System.ComponentModel;

// HACK notes disabled
//namespace ZachJohnson.Promptu.UserModel.Configuration
//{
//    internal class NotesSettings : WindowSettings<Notes>
//    {
//        private static readonly Color DefaultBackgroundColor = SystemColors.Window;
//        private static readonly Color DefaultTextColor = SystemColors.WindowText;
//        private int selectionStart;
//        private int selectionLength;
//        private bool alwaysOnTop;
//        private Font font;
//        private Color background;
//        private Color textColor;

//        public NotesSettings()
//            : this(null, 0, 0, true, null, DefaultTextColor, DefaultBackgroundColor)
//        {
//        }

//        public NotesSettings(
//            WindowSettings<Notes> baseSettings, 
//            int selectionStart, 
//            int selectionLength, 
//            bool alwaysOnTop, 
//            Font font, 
//            Color textColor, 
//            Color background)
//            : base(baseSettings)
//        {
//            this.selectionStart = selectionStart;
//            this.selectionLength = selectionLength;
//            this.alwaysOnTop = alwaysOnTop;

//            this.Font = font;

//            this.background = background;
//            this.textColor = textColor;
//        }

//        [UserEditable]
//        [DisplayName("Font")]
//        [Category("Appearance")]
//        public Font Font
//        {
//            get 
//            { 
//                return this.font; 
//            }

//            set 
//            {
//                if (value == null)
//                {
//                    this.font = new Font(PromptuFonts.DefaultFont.FontFamily, 10);
//                }
//                else
//                {
//                    this.font = value;
//                }

//                this.OnSettingChanged(EventArgs.Empty);
//            }
//        }

//        [UserEditable]
//        [DisplayName("Text Color")]
//        [PersistValue]
//        [Category("Appearance")]
//        public Color TextColor
//        {
//            get
//            {
//                return this.textColor;
//            }

//            set
//            {
//                if (value == null)
//                {
//                    this.textColor = DefaultTextColor;
//                }
//                else
//                {
//                    this.textColor = value;
//                }

//                this.OnSettingChanged(EventArgs.Empty);
//            }
//        }

//        [UserEditable]
//        [DisplayName("Background Color")]
//        [Category("Appearance")]
//        public Color BackgroundColor
//        {
//            get
//            {
//                return this.background;
//            }

//            set
//            {
//                if (value == null)
//                {
//                    this.background = DefaultBackgroundColor;
//                }
//                else
//                {
//                    this.background = value;
//                }

//                this.OnSettingChanged(EventArgs.Empty);
//            }
//        }

//        public bool AlwaysOnTop
//        {
//            get 
//            { 
//                return this.alwaysOnTop; 
//            }

//            set
//            {
//                this.alwaysOnTop = value;
//                this.OnSettingChanged(EventArgs.Empty);
//            }
//        }

//        public int SelectionStart
//        {
//            get 
//            { 
//                return this.selectionStart; 
//            }

//            set 
//            { 
//                this.selectionStart = value;
//                this.OnSettingChanged(EventArgs.Empty);
//            }
//        }

//        public int SelectionLength
//        {
//            get 
//            { 
//                return this.selectionLength; 
//            }

//            set 
//            { 
//                this.selectionLength = value;
//                this.OnSettingChanged(EventArgs.Empty);
//            }
//        }

//        protected override XmlNode ToXmlCore(string nodeName, XmlDocument document)
//        {
//            XmlNode node = base.ToXmlCore(nodeName, document);

//            node.Attributes.Append(XmlUtilities.CreateAttribute("selectionStart", this.SelectionStart, document));
//            node.Attributes.Append(XmlUtilities.CreateAttribute("selectionLength", this.SelectionLength, document));
//            node.Attributes.Append(XmlUtilities.CreateAttribute("alwaysOnTop", this.AlwaysOnTop, document));
//            node.Attributes.Append(XmlUtilities.CreateAttribute("font", new FontConverter().ConvertToString(this.Font), document));
//            node.Attributes.Append(XmlUtilities.CreateAttribute("textColor", ColorTranslator.ToHtml(this.textColor), document));
//            node.Attributes.Append(XmlUtilities.CreateAttribute("backgroundColor", ColorTranslator.ToHtml(this.background), document));

//            return node;
//        }

//        public static new NotesSettings FromXml(XmlNode node)
//        {
//            if (node == null)
//            {
//                throw new ArgumentNullException("node");
//            }

//            int selectionStart = 0;
//            int selectionLength = 0;
//            bool alwaysOnTop = true;
//            Font font = null;
//            Color textColor = DefaultTextColor;
//            Color backgroundColor = DefaultBackgroundColor;

//            WindowSettings<Notes> windowSettings = WindowSettings<Notes>.FromXml(node);

//            foreach (XmlAttribute attribute in node.Attributes)
//            {
//                switch (attribute.Name.ToUpperInvariant())
//                {
//                    case "SELECTIONSTART":
//                        try
//                        {
//                            selectionStart = Convert.ToInt32(attribute.Value);
//                        }
//                        catch (FormatException)
//                        {
//                        }
//                        catch (OverflowException)
//                        {
//                        }

//                        break;
//                    case "SELECTIONLENGTH":
//                        try
//                        {
//                            selectionLength = Convert.ToInt32(attribute.Value);
//                        }
//                        catch (FormatException)
//                        {
//                        }
//                        catch (OverflowException)
//                        {
//                        }

//                        break;
//                    case "ALWAYSONTOP":
//                        try
//                        {
//                            alwaysOnTop = Convert.ToBoolean(attribute.Value);
//                        }
//                        catch (FormatException)
//                        {
//                        }

//                        break;
//                    case "FONT":
//                        try
//                        {
//                            font = (Font)new FontConverter().ConvertFromString(attribute.Value);
//                        }
//                        catch (NotSupportedException)
//                        {
//                        }

//                        break;
//                    case "TEXTCOLOR":
//                        try
//                        {
//                            textColor = ColorTranslator.FromHtml(attribute.Value);
//                        }
//                        catch (Exception)
//                        {
//                        }

//                        break;
//                    case "BACKGROUNDCOLOR":
//                        try
//                        {
//                            backgroundColor = ColorTranslator.FromHtml(attribute.Value);
//                        }
//                        catch (Exception)
//                        {
//                        }

//                        break;
//                    default:
//                        break;
//                }
//            }

//            return new NotesSettings(windowSettings, selectionStart, selectionLength, alwaysOnTop, font, textColor, backgroundColor);
//        }

//        protected override void ImpartToCore(Notes form)
//        {
//            base.ImpartToCore(form);

//            form.TextBox.Select(this.SelectionStart, this.SelectionLength);
//            //form.TextBox.ScrollToCaret();
//            form.AlwaysOnTop = this.AlwaysOnTop;
//            this.ImpartUserEditableTo(form);
//        }

//        public void ImpartUserEditableTo(Notes form)
//        {
//            form.TextBox.Font = this.Font;
//            form.TextBox.ForeColor = this.TextColor;
//            form.TextBox.BackColor = this.BackgroundColor;
//        }

//        protected override bool UpdateFromCore(Notes form)
//        {
//            this.RaiseSettingChanged = false;
//            this.SelectionStart = form.TextBox.SelectionStart;
//            this.SelectionLength = form.TextBox.SelectionLength;
//            this.AlwaysOnTop = form.AlwaysOnTop;
//            this.Font = (Font)form.TextBox.Font.Clone();
//            this.TextColor = form.TextBox.ForeColor;
//            this.BackgroundColor = form.TextBox.BackColor;
//            this.RaiseSettingChanged = true;

//            return true;
//        }
//    }
//}
