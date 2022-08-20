//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.UI;
//using System.Xml;

// HACK disabled for compilation
//namespace ZachJohnson.Promptu.UserModel.Configuration
//{
//    internal class SetupDialogSettings : WindowSettings<SetupDialog>
//    {
//        internal const int DefaultSkinSplitterDistance = 277;
//        internal const int DefaultPropertyAreaSplitterDistance = 97;
//        internal const int DefaultListSplitterDistance = 240;
//        internal const int DefaultListDescriptionHeight = 102;
//        internal const int DefaultSkinPropertyGridDescriptionAreaHeight = 59;
//        private int skinSplitterDistance;
//        private int listSplitterDistance;
//        private int skinPropertyGridDescriptionAreaHeight;
//        private int listDescriptionHeight;
//       // private TabControlSettings mainTabs;
//        private int propertyAreaSplitterDistance;
//        //private TabControlSettings listTabs;

//        public SetupDialogSettings()
//            : this(
//            null, 
//            DefaultSkinSplitterDistance,
//            DefaultPropertyAreaSplitterDistance,
//            DefaultListSplitterDistance,
//            DefaultSkinPropertyGridDescriptionAreaHeight,
//            DefaultListDescriptionHeight)
//        {
//        }

//        public SetupDialogSettings(
//            WindowSettings<SetupDialog> baseSettings, 
//            int skinSplitterDistance,
//            int propertyAreaSplitterDistance,
//            int listSplitterDistance,
//            int skinPropertyGridDescriptionAreaHeight,
//            int listDescriptionHeight)
//            : base(baseSettings)
//        {
//            this.skinSplitterDistance = skinSplitterDistance;
//            this.propertyAreaSplitterDistance = propertyAreaSplitterDistance;
//            this.listSplitterDistance = listSplitterDistance;
//            this.skinPropertyGridDescriptionAreaHeight = skinPropertyGridDescriptionAreaHeight;
//            this.listDescriptionHeight = listDescriptionHeight;

//            //if (mainTabs == null)
//            //{
//            //    this.mainTabs = new TabControlSettings();
//            //}
//            //else
//            //{
//            //    this.mainTabs = mainTabs;
//            //}

//            //if (listTabs == null)
//            //{
//            //    this.listTabs = new TabControlSettings();
//            //}
//            //else
//            //{
//            //    this.listTabs = listTabs;
//            //}

//            //this.mainTabs.SettingChanged += this.RaiseSettingChangedEvent;
//            //this.listTabs.SettingChanged += this.RaiseSettingChangedEvent;
//        }

//        public int SkinSplitterDistance
//        {
//            get
//            {
//                return this.skinSplitterDistance; 
//            }

//            set
//            {
//                if (value != this.skinSplitterDistance)
//                {
//                    this.skinSplitterDistance = value;
//                    this.OnSettingChanged(EventArgs.Empty);
//                }
//            }
//        }

//        public int PropertyAreaSplitterDistance
//        {
//            get
//            {
//                return this.propertyAreaSplitterDistance;
//            }

//            set
//            {
//                if (value != this.propertyAreaSplitterDistance)
//                {
//                    this.propertyAreaSplitterDistance = value;
//                    this.OnSettingChanged(EventArgs.Empty);
//                }
//            }
//        }

//        public int ListSplitterDistance
//        {
//            get
//            {
//                return this.listSplitterDistance;
//            }

//            set
//            {
//                if (value != this.listSplitterDistance)
//                {
//                    this.listSplitterDistance = value;
//                    this.OnSettingChanged(EventArgs.Empty);
//                }
//            }
//        }

//        public int SkinPropertyGridDescriptionAreaHeight
//        {
//            get
//            {
//                return this.skinPropertyGridDescriptionAreaHeight;
//            }

//            set
//            {
//                if (value != this.skinPropertyGridDescriptionAreaHeight)
//                {
//                    this.skinPropertyGridDescriptionAreaHeight = value;
//                    this.OnSettingChanged(EventArgs.Empty);
//                }
//            }
//        }
//        public int ListDescriptionHeight
//        {
//            get
//            {
//                return this.listDescriptionHeight;
//            }

//            set
//            {
//                if (value != this.listDescriptionHeight)
//                {
//                    this.listDescriptionHeight = value;
//                    this.OnSettingChanged(EventArgs.Empty);
//                }
//            }
//        }

//        //public TabControlSettings MainTabs
//        //{
//        //    get { return this.mainTabs; }
//        //}

//        //public TabControlSettings ListTabs
//        //{
//        //    get { return this.listTabs; }
//        //}

//        public static new SetupDialogSettings FromXml(XmlNode node)
//        {
//            if (node == null)
//            {
//                throw new ArgumentNullException("node");
//            }

//            int listSplitterDistance = DefaultListSplitterDistance;
//            int skinSplitterDistance = DefaultSkinSplitterDistance;
//            int propertyAreaSplitterDistance = DefaultPropertyAreaSplitterDistance;
//            int skinPropertyGridDescriptionAreaHeight = DefaultSkinPropertyGridDescriptionAreaHeight;
//            int listDescriptionHeight = DefaultListDescriptionHeight;
//            //TabControlSettings mainTabs = null;
//            //TabControlSettings listTabs = null;

//            WindowSettings<SetupDialog> windowSettings = WindowSettings<SetupDialog>.FromXml(node);

//            //foreach (XmlNode innerNode in node.ChildNodes)
//            //{
//            //    switch (innerNode.Name.ToUpperInvariant())
//            //    {
//            //        case "MAINTABS":
//            //            mainTabs = TabControlSettings.FromXml(innerNode);
//            //            break;
//            //        //case "LISTTABS":
//            //        //    listTabs = TabControlSettings.FromXml(innerNode);
//            //        //    break;
//            //        default:
//            //            break;
//            //    }
//            //}

//            foreach (XmlAttribute attribute in node.Attributes)
//            {
//                switch (attribute.Name.ToUpperInvariant())
//                {
//                    case "LISTSPLITTERDISTANCE":
//                        try
//                        {
//                            listSplitterDistance = Convert.ToInt32(attribute.Value);
//                        }
//                        catch (FormatException)
//                        {
//                        }
//                        catch (OverflowException)
//                        {
//                        }

//                        break;
//                    case "SKINSPLITTERDISTANCE":
//                        try
//                        {
//                            skinSplitterDistance = Convert.ToInt32(attribute.Value);
//                        }
//                        catch (FormatException)
//                        {
//                        }
//                        catch (OverflowException)
//                        {
//                        }

//                        break;
//                    case "PROPERTYAREASPLITTERDISTANCE":
//                        try
//                        {
//                            propertyAreaSplitterDistance = Convert.ToInt32(attribute.Value);
//                        }
//                        catch (FormatException)
//                        {
//                        }
//                        catch (OverflowException)
//                        {
//                        }

//                        break;
//                    case "LISTDESCRIPTIONHEIGHT":
//                        try
//                        {
//                            listDescriptionHeight = Convert.ToInt32(attribute.Value);
//                        }
//                        catch (FormatException)
//                        {
//                        }
//                        catch (OverflowException)
//                        {
//                        }

//                        break;
//                    case "SKINPROPERTYGRIDDESCRIPTIONAREAHEIGHT":
//                        try
//                        {
//                            skinPropertyGridDescriptionAreaHeight = Convert.ToInt32(attribute.Value);
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

//            return new SetupDialogSettings(
//                windowSettings,
//                skinSplitterDistance,
//                propertyAreaSplitterDistance,
//                listSplitterDistance,
//                skinPropertyGridDescriptionAreaHeight,
//                listDescriptionHeight);
//                //listTabs);
//        }

//        protected override XmlNode ToXmlCore(string nodeName, XmlDocument document)
//        {
//            XmlNode node = base.ToXmlCore(nodeName, document);

//            node.Attributes.Append(XmlUtilities.CreateAttribute("listSplitterDistance", this.listSplitterDistance, document));
//            node.Attributes.Append(XmlUtilities.CreateAttribute("skinSplitterDistance", this.skinSplitterDistance, document));
//            node.Attributes.Append(XmlUtilities.CreateAttribute("propertyAreaSplitterDistance", this.propertyAreaSplitterDistance, document));
//            node.Attributes.Append(XmlUtilities.CreateAttribute("listDescriptionHeight", this.listDescriptionHeight, document));
//            node.Attributes.Append(XmlUtilities.CreateAttribute("skinPropertyGridDescriptionAreaHeight", this.skinPropertyGridDescriptionAreaHeight, document));

//            //node.AppendChild(this.mainTabs.ToXml("MainTabs", document));
//            //node.AppendChild(this.listTabs.ToXml("ListTabs", document));

//            return node;
//        }

//        protected override void ImpartToCore(SetupDialog form)
//        {
//            form.SkinSplitterDistance = this.SkinSplitterDistance;
//            form.PropertyAreaSplitterDistance = this.PropertyAreaSplitterDistance;
//            form.ListSplitterDistance = this.ListSplitterDistance;
//            form.ListSelector.ListDescriptionSplitContainer.ReverseSplitterDistance = this.listDescriptionHeight;
//            form.ItemPropertyGrid.DescriptionAreaHeight = this.skinPropertyGridDescriptionAreaHeight;

//            //this.MainTabs.ImpartTo(form.MainTabs);
//            //this.ListTabs.ImpartTo(form.ListTabs);
//        }

//        protected override bool UpdateFromCore(SetupDialog form)
//        {
//            bool anythingChanged = false;

//            if (form.SkinSplitterDistance != this.SkinSplitterDistance)
//            {
//                this.skinSplitterDistance = form.SkinSplitterDistance;
//                anythingChanged = true;
//            }

//            if (form.PropertyAreaSplitterDistance != this.PropertyAreaSplitterDistance)
//            {
//                this.propertyAreaSplitterDistance = form.PropertyAreaSplitterDistance;
//                anythingChanged = true;
//            }

//            if (form.ListSplitterDistance != this.ListSplitterDistance)
//            {
//                this.listSplitterDistance = form.ListSplitterDistance;
//                anythingChanged = true;
//            }

//            if (form.ListSelector.ListDescriptionSplitContainer.ReverseSplitterDistance != this.listDescriptionHeight)
//            {
//                this.listDescriptionHeight = form.ListSelector.ListDescriptionSplitContainer.ReverseSplitterDistance;
//                anythingChanged = true;
//            }

//            if (form.ItemPropertyGrid.DescriptionAreaHeight != this.skinPropertyGridDescriptionAreaHeight)
//            {
//                this.skinPropertyGridDescriptionAreaHeight = form.ItemPropertyGrid.DescriptionAreaHeight;
//                anythingChanged = true;
//            }

//            this.RaiseSettingChanged = false;

//            //if (this.MainTabs.UpdateFrom(form.MainTabs))
//            //{
//            //    anythingChanged = true;
//            //}

//            //if (this.ListTabs.UpdateFrom(form.ListTabs))
//            //{
//            //    anythingChanged = true;
//            //}

//            this.RaiseSettingChanged = true;

//            return anythingChanged;
//        }
//    }
//}
