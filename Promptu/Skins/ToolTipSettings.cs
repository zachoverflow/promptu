//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.SkinApi;
//using System.Drawing;
//using System.ComponentModel;
//using System.Windows.Forms;

//namespace ZachJohnson.Promptu.Skins
//{
//    internal class ToolTipSettings
//    {
//        private Font font;

//        public ToolTipSettings()
//        {
//            this.font = new Font("Tahoma", 8.25F);
//        }

//        [UserEditable]
//        [DisplayName("Font")]
//        [PersistValue]
//        [Category("Appearance")]
//        public Font Font
//        {
//            get
//            {
//                return this.font;
//            }

//            set
//            {
//                if (value != null)
//                {
//                    this.font = value;
//                }
//                else
//                {
//                    this.font = new Font("Tahoma", 8.25F);
//                }
//            }
//        }

//        public void ApplyTo(ITextualInformationBox box)
//        {
//            box.Font = this.Font;
//            box.Content.Font = this.Font;
//        }
//    }
//}
