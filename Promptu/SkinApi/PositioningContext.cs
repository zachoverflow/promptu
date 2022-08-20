//-----------------------------------------------------------------------
// <copyright file="PositioningContext.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    public class PositioningContext
    {
        private PromptuSkin skin;
        private PromptuSkinInstance skinInstance;
        private InfoBoxes infoBoxes;

        public PositioningContext(PromptuSkin skin, PromptuSkinInstance skinInstance, InfoBoxes infoBoxes)
        {
            this.skin = skin;
            this.infoBoxes = infoBoxes;
            this.skinInstance = skinInstance;
        }

        public PromptuSkin Skin
        {
            get { return this.skin; }
        }

        public PromptuSkinInstance SkinInstance
        {
            get { return this.skinInstance; }
        }

        public InfoBoxes InfoBoxes
        {
            get { return this.infoBoxes; }
        }
    }
}
