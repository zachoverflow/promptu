//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;
//using System.IO;

//namespace ZachJohnson.Promptu
//{
//    internal static class SystemEvents
//    {
//        public delegate void StaticEventHandler(EventArgs e);
//        private static SystemEventListener eventListener = SystemEventListener.GetInstance();

//        public static bool On
//        {
//            get { return eventListener.On; }
//            set { eventListener.On = value; }
//        }

//        public static event StaticEventHandler ResumeFromSleep;

//        private static void OnResumeFromSleep(EventArgs e)
//        {
//            StaticEventHandler handler = ResumeFromSleep;
//            if (handler != null)
//            {
//                handler(e);
//            }
//        }

//        private class SystemEventListener : NativeWindow
//        {
//            private static SystemEventListener instance;
//            private bool on;

//            private SystemEventListener()
//            {
//            }

//            internal bool On
//            {
//                get { return this.on; }
//                set
//                {
//                    if (value)
//                    {
//                        if (!this.on)
//                        {
//                            this.CreateHandle(new CreateParams());
//                        }
//                    }
//                    else
//                    {
//                        if (this.on)
//                        {
//                            this.DestroyHandle();
//                        }
//                    }

//                    this.on = value;
//                }
//            }

//            public static SystemEventListener GetInstance()
//            {
//                if (instance == null)
//                {
//                    instance = new SystemEventListener();
//                }

//                return instance;
//            }

//            protected override void WndProc(ref Message m)
//            {
//                //if (m.Msg == (int)WindowsMessages.WM_QUERYENDSESSION)
//                //{
//                //    while (true)
//                //    {
//                //    }
//                //}

//                if (m.Msg == 0x218) // WM_POWERBROADCAST
//                {
//                    //m.Result = (IntPtr)0x424D5144;
//                    Log.RecordMessage("Recieved WM_POWERBROADCAST.  wParam = " + m.WParam.ToString());
//                    switch ((int)m.WParam)
//                    {
//                        case 0x7: // PBT_APMRESUMESUSPEND
//                            Log.RecordMessage("SystemEvents is raising OnResumeFromSleep.");
//                            OnResumeFromSleep(EventArgs.Empty);
//                            break;
//                        default:
//                            break;
//                    }
//                }

//                base.WndProc(ref m);
//            }
//        }
//    }
//}
