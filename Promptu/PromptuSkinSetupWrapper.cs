//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.SkinApi;
//using ZachJohnson.Promptu.UIModel.Interfaces;

//namespace ZachJohnson.Promptu
//{
//    internal class PromptuSkinSetupWrapper
//    {
//        PromptuSkin skin;

//        public PromptuSkinSetupWrapper(PromptuSkin skin)
//        {
//            if (skin == null)
//            {
//                throw new ArgumentNullException("skin");
//            }

//            this.skin = skin;
//        }

//        public event EventHandler Configure;

//        public PromptuSkin Skin
//        {
//            get { return this.skin; }
//        }

//        private void RaiseConfigure(object sender, EventArgs e)
//        {
//            this.OnConfigure(EventArgs.Empty);
//        }

//        protected virtual void OnConfigure(EventArgs e)
//        {
//            EventHandler handler = this.Configure;
//            if (handler != null)
//            {
//                handler(this, e);
//            }
//        }
//    }
//}
