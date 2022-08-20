//-----------------------------------------------------------------------
// <copyright file="InfoBoxes.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System.Collections.Generic;
    using ZachJohnson.Promptu.Skins;

    public sealed class InfoBoxes : IEnumerable<IInfoBox>
    {
        private InformationBoxManager manager;

        internal InfoBoxes(InformationBoxManager manager)
        {
            this.manager = manager;
        }

        public IInfoBox ItemInfoBox
        {
            get { return this.manager.ItemInfoBox; }
        }

        public IInfoBox ParameterHelpBox
        {
            get { return this.manager.ParameterHelpBox; }
        }

        public IEnumerator<IInfoBox> GetEnumerator()
        {
            return this.manager.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
