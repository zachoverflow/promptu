//-----------------------------------------------------------------------
// <copyright file="ScaledQuantity.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    internal struct ScaledQuantity
    {
        private float value;
        private float? scalePpi;

        public ScaledQuantity(float value, float? scalePpi)
        {
            this.value = value;
            this.scalePpi = scalePpi;
        }

        public float Value
        {
            get { return this.value; }
        }

        public float? ScalePpi
        {
            get { return this.scalePpi; }
        }

        public static ScaledQuantity At96Ppi(float value)
        {
            return new ScaledQuantity(value, 96);
        }

        public static ScaledQuantity At120Ppi(float value)
        {
            return new ScaledQuantity(value, 120);
        }
    }
}
