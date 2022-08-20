//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;
//using System.Xml;

//namespace ZachJohnson.Promptu.UserModel.Configuration
//{
//    internal class TabControlSettings : GenericSettings
//    {
//        internal const int DefaultSelectedTab = 0;
//        private int selectedTab;

//        public TabControlSettings()
//            : this(DefaultSelectedTab)
//        {
//        }

//        public TabControlSettings(int selectedTab)
//        {
//            this.selectedTab = selectedTab;
//        }

//        public int SelectedTab
//        {
//            get
//            {
//                return this.selectedTab;
//            }

//            set
//            {
//                if (value != this.selectedTab)
//                {
//                    this.selectedTab = value;
//                    this.OnSettingChanged(EventArgs.Empty);
//                }
//            }
//        }

//        public void ImpartTo(TabControl tabControl)
//        {
//            if (tabControl == null)
//            {
//                throw new ArgumentNullException("tabControl");
//            }

//            int setValue = this.selectedTab;
//            if (this.selectedTab < 0)
//            {
//                setValue = 0;
//            }
//            else if (setValue >= tabControl.TabCount)
//            {
//                setValue = tabControl.TabCount - 1;
//            }

//            tabControl.SelectedIndex = setValue;
//        }

//        public static TabControlSettings FromXml(XmlNode node)
//        {
//            if (node == null)
//            {
//                throw new ArgumentNullException("node");
//            }

//            int selectedTab = DefaultSelectedTab;

//            foreach (XmlAttribute attribute in node.Attributes)
//            {
//                switch (attribute.Name.ToUpperInvariant())
//                {
//                    case "SELECTEDTAB":
//                        try
//                        {
//                            selectedTab = Convert.ToInt32(attribute.Value);
//                        }
//                        catch (FormatException)
//                        {
//                        }
//                        catch (OverflowException)
//                        {
//                        }

//                        break;
//                    default:
//                        break;
//                }
//            }

//            return new TabControlSettings(selectedTab);
//        }

//        protected override XmlNode ToXmlCore(string nodeName, XmlDocument document)
//        {
//            XmlNode node = document.CreateElement(nodeName);

//            node.Attributes.Append(XmlUtilities.CreateAttribute("selectedTab", this.selectedTab, document));

//            return node;
//        } 

//        public bool UpdateFrom(TabControl tabControl)
//        {
//            if (tabControl == null)
//            {
//                throw new ArgumentNullException("tabControl");
//            }

//            bool anythingChanged = false;

//            if (tabControl.SelectedIndex != this.SelectedTab)
//            {
//                this.selectedTab = tabControl.SelectedIndex;
//                anythingChanged = true;
//            }

//            if (anythingChanged)
//            {
//                this.OnSettingChanged(EventArgs.Empty);
//            }

//            return anythingChanged;
//        }
//    }
//}
