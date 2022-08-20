//-----------------------------------------------------------------------
// <copyright file="ObjectSettings.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Configuration
{
    using System;

    public abstract class ObjectSettings<T> : SettingsBase
    {
        public ObjectSettings()
        {
        }

        public void ImpartTo(T obj)
        {
            this.ImpartToCore(obj);
        }

        public void UpdateFrom(T obj)
        {
            bool anythingChanged = false;
            this.UpdateFrom(obj, ref anythingChanged);

            if (anythingChanged)
            {
                this.OnSettingChanged(EventArgs.Empty);
            }
        }

        public void UpdateFrom(T obj, ref bool anythingChanged)
        {
            this.UpdateFromCore(obj, ref anythingChanged);
        }

        protected abstract void ImpartToCore(T obj);

        protected abstract void UpdateFromCore(T obj, ref bool anythingChanged);
    }
}
