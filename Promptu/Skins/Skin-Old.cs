//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Reflection;
//using ZachJohnson.Promptu.SkinApi;
//using ZachJohnson.Promptu.UI;

//namespace ZachJohnson.Promptu.Skins
//{
//    [Obsolete]
//    internal class Skin : IInstanceOnDemand
//    {
//        private Type skin;
//        private string name;
//        private string description = String.Empty;
//        private string creator = String.Empty;
//        private IPrompt instance;
//        private ISuggester suggesterInstance;
//        private ToolTipSettings toolTipSettings = new ToolTipSettings();
//        private bool reloadPostSleep;

//        internal Skin(Type skin)
//        {
//            if (skin == null)
//            {
//                throw new ArgumentNullException("skin");
//            }

//            Type iface = null;
//            try
//            {
//                iface = skin.GetInterface("IPrompt");
//            }
//            catch (AmbiguousMatchException)
//            {
//            }

//            if (iface == null)
//            {
//                throw new ArgumentException("The skin does not implement the IPrompt interface.");
//            }

//            SkinNameAttribute[] foundNameAttributes = (SkinNameAttribute[])skin.GetCustomAttributes(typeof(SkinNameAttribute), false);

//            if (foundNameAttributes.Length != 1)
//            {
//                throw new ArgumentException("The skin does not implement the SkinName attribute.");
//            }

//            SkinCreatorAttribute[] foundCreatorAttributes = (SkinCreatorAttribute[])skin.GetCustomAttributes(typeof(SkinCreatorAttribute), false);

//            if (foundCreatorAttributes.Length == 1)
//            {
//                this.creator = foundCreatorAttributes[0].Creator;
//            }

//            SkinDescriptionAttribute[] foundDescriptionAttributes = (SkinDescriptionAttribute[])skin.GetCustomAttributes(typeof(SkinDescriptionAttribute), false);

//            if (foundDescriptionAttributes.Length == 1)
//            {
//                this.description = foundDescriptionAttributes[0].Description;
//            }

//            ReloadPostSleepAttribute[] foundReloadPostSleepAttributes = (ReloadPostSleepAttribute[])skin.GetCustomAttributes(typeof(ReloadPostSleepAttribute), false);

//            if (foundReloadPostSleepAttributes.Length == 1)
//            {
//                this.reloadPostSleep = foundReloadPostSleepAttributes[0].ReloadPostSleep;
//            }

//            this.name = foundNameAttributes[0].SkinName;
//            this.skin = skin;
//        }

//        public string Name
//        {
//            get { return this.name; }
//        }

//        public string Creator
//        {
//            get { return this.creator; }
//        }

//        public string Description
//        {
//            get { return this.description; }
//        }

//        public bool ReloadPostSleep
//        {
//            get { return this.reloadPostSleep; }
//        }

//        public void ClearInstance()
//        {
//            this.instance = null;
//        }

//        object IInstanceOnDemand.GetInstance()
//        {
//            return this.GetInstance();
//        }

//        public ISuggester GetSuggester()
//        {
//            ISuggester suggester = this.GetInstance().Suggester;
//            if (suggester == null)
//            {
//                if (this.suggesterInstance == null)
//                {
//                    this.suggesterInstance = new Default.DefaultSuggester();
//                }

//                return this.suggesterInstance;
//            }

//            return suggester;
//        }

//        public ToolTipSettings ToolTipSettings
//        {
//            get { return this.toolTipSettings; }
//        }

//        public IPrompt GetInstance()
//        {
//            if (this.instance == null)
//            {
//                Type iface = null;
//                try
//                {
//                    iface = this.skin.GetInterface("IPrompt");
//                }
//                catch (AmbiguousMatchException ex)
//                {
//                    throw new LoadException(ex.Message, ex);
//                }

//                if (iface != null)
//                {
//                    object pluginInstance = null;
//                    try
//                    {
//                        pluginInstance = Activator.CreateInstance(this.skin);
//                    }
//                    catch (TargetInvocationException ex)
//                    {
//                        throw new LoadException(ex.Message, ex);
//                    }
//                    catch (MissingMethodException ex)
//                    {
//                        throw new LoadException(ex.Message, ex);
//                    }
//                    catch (MemberAccessException ex)
//                    {
//                        throw new LoadException(ex.Message, ex);
//                    }

//                    if (pluginInstance != null)
//                    {
//                        this.instance = (IPrompt)pluginInstance;
//                        this.instance.ClosePrompt();
//                        return this.instance;
//                    }
//                    else
//                    {
//                        throw new LoadException("Could not create an instance of the skin.");
//                    }
//                }
//                else
//                {
//                    throw new LoadException("The skin does not implement the IPlugin interface.");
//                }
//            }
//            else
//            {
//                return this.instance;
//            }
//        }

//        protected virtual void WrapAnyExceptionInSkinException(ParameterlessVoid action)
//        {
//            try
//            {
//                action.Invoke();
//            }
//            catch (Exception ex)
//            {
//                if (ex is System.Threading.ThreadAbortException || ex is SkinException)
//                {
//                    throw;
//                }

//                throw new SkinException(ex.Message, ex);
//            }
//        }

//        public override string ToString()
//        {
//            return this.name;
//        }
//    }
//}
