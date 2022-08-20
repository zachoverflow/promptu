//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;

//namespace ZachJohnson.Promptu.UI
//{
//    internal class StyledListBox : ListBox
//    {
//        private bool doStyling = true;

//        public StyledListBox()
//        {
//        }

//        public bool DoStyling
//        {
//            get 
//            { 
//                return this.doStyling; 
//            }

//            set
//            {
//                this.doStyling = value;
//                if (value)
//                {
//                    this.UpdateStyle();
//                }
//            }
//        }

//        protected override void OnHandleCreated(EventArgs e)
//        {
//            base.OnHandleCreated(e);
//            this.UpdateStyle();
//        }

//        public void UpdateStyle()
//        {
//            if (this.doStyling)
//            {
//                NativeMethods.SetWindowTheme(this.Handle, "explorer", null);
//            }
//        }
//    }
//}
