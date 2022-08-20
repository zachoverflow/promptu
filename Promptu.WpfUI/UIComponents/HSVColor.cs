using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

// some code from http://www.dev102.com/2009/07/23/changing-brush-brightness-in-wpfsilverlight/
// some code from http://www.codeproject.com/KB/WPF/WpfColorConversions.aspx
namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class HSVColor
    {
        public double H { get; set; }
        public double S { get; set; }
        public double V { get; set; }
        public byte A { get; set; }

        public HSVColor(double h, double s, double v, byte a)
        {
            this.H = h;
            this.S = s;
            this.V = v;
            this.A = a;
        }

        public HSVColor(Color rgbColor)
        {
            /* Hue values range between 0 and 360. All 
             * other values range between 0 and 1. */

            // Create HSB color object
            //var hsbColor = new HsbColor();

            // Get RGB color component values
            var r = (int)rgbColor.R;
            var g = (int)rgbColor.G;
            var b = (int)rgbColor.B;
            var a = rgbColor.A;

            // Get min, max, and delta values
            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);
            double delta = max - min;

            /* Black (max = 0) is a special case. We 
             * simply set HSB values to zero and exit. */

            // Black: Set HSB and return
            if (max == 0.0)
            {
                this.H = 0.0;
                this.S = 0.0;
                this.V = 0.0;
                this.A = a;
            }
            else
            {

                /* Now we process the normal case. */

                // Set HSB Alpha value
                //var alpha = (double)a;
                this.A = a;//alpha / 255;

                // Set HSB Hue value
                if (r == max) this.H = (g - b) / delta;
                else if (g == max) this.H = 2 + (b - r) / delta;
                else if (b == max) this.H = 4 + (r - g) / delta;
                this.H *= 60;
                if (this.H < 0.0) this.H += 360;

                // Set other HSB values
                this.S = delta / max;
                this.V = max / 255;
            }

            // Set return value
            //return this;

            //// preserve alpha
            //this.A = rgbColor.A;

            //// convert R, G, B to numbers from 0 to 1

            //double r = rgbColor.R / 255d;
            //double g = rgbColor.G / 255d;
            //double b = rgbColor.B / 255d;

            //double max = Math.Max(r, Math.Max(g, b));
            //double min = Math.Min(r, Math.Min(g, b));

            //// hue
            //if (max == min)
            //{
            //    this.H = 0;
            //}
            //else if (max == r)
            //{
            //    this.H = (60 * (g - b) / (max - min) + 360) % 360;
            //}
            //else if (max == g)
            //{
            //    this.H = 60 * (b - r) / (max - min) + 120;
            //}
            //else
            //{
            //    this.H = 60 * (r - g) / (max - min) + 240;
            //}

            //// saturation
            //if (max == 0)
            //{
            //    this.S = 0;
            //}
            //else
            //{
            //    this.S = 1 - min / max;
            //}

            //// value
            //this.V = max;
        }

        public HSVColor Clone()
        {
            return new HSVColor(this.H, this.S, this.V, this.A);
        }

        public void Clamp()
        {
            if (this.H < 0)
            {
                this.H = 0;
            }
            else if (this.H > 360)
            {
                this.H = 360;
            }

            if (this.S < 0)
            {
                this.S = 0;
            }
            else if (this.S > 1)
            {
                this.S = 1;
            }

            if (this.V < 0)
            {
                this.V = 0;
            }
            else if (this.V > 1)
            {
                this.V = 1;
            }
        }

        public Color ToColor()
        {
            return ColorFromAhsb(this);
            //Color result = new Color();
            //result.A = this.A;
            //int hi = (int)Math.Floor(this.H / 60) % 6;
            //double f = this.H / 60 - Math.Floor(this.H / 60);
            //double p = this.V * (1 - this.S);
            //double q = this.V * (1 - f * this.S);
            //double t = this.V * (1 - (1 - f) * this.S);

            //switch (hi)
            //{
            //    case 0:
            //        result.R = (byte)(this.V );
            //        result.G = (byte)(t);
            //        result.B = (byte)(p);
            //        break;
            //    case 1:
            //        result.R = (byte)(q);
            //        result.G = (byte)(this.V);
            //        result.B = (byte)(p);
            //        break;
            //    case 2:
            //        result.R = (byte)(p);
            //        result.G = (byte)(this.V);
            //        result.B = (byte)(q);
            //        break;
            //    case 3:
            //        result.R = (byte)(p);
            //        result.G = (byte)(q);
            //        result.B = (byte)(this.V);
            //        break;
            //    case 4:
            //        result.R = (byte)(t);
            //        result.G = (byte)(p);
            //        result.B = (byte)(this.V);
            //        break;
            //    case 5:
            //        result.R = (byte)(this.V);
            //        result.G = (byte)(p);
            //        result.B = (byte)(q);
            //        break;

            //}

            //return result;
        }

        private static Color ColorFromAhsb(HSVColor hsbColor)
        {
            // Initialize
            var rgbColor = new Color();

            /* Gray (zero saturation) is a special case.We simply
             * set RGB values to HSB Brightness value and exit. */

            // Gray: Set RGB and return
            if (hsbColor.S == 0.0)
            {
                rgbColor.A = hsbColor.A;
                rgbColor.R = (byte)(hsbColor.V * 255);
                rgbColor.G = (byte)(hsbColor.V * 255);
                rgbColor.B = (byte)(hsbColor.V * 255);
                return rgbColor;
            }

            /* Now we process the normal case. */

            var h = (hsbColor.H == 360) ? 0 : hsbColor.H / 60;
            var i = (int)(Math.Truncate(h));
            var f = h - i;

            var p = hsbColor.V * (1.0 - hsbColor.S);
            var q = hsbColor.V * (1.0 - (hsbColor.S * f));
            var t = hsbColor.V * (1.0 - (hsbColor.S * (1.0 - f)));

            double r, g, b;
            switch (i)
            {
                case 0:
                    r = hsbColor.V;
                    g = t;
                    b = p;
                    break;

                case 1:
                    r = q;
                    g = hsbColor.V;
                    b = p;
                    break;

                case 2:
                    r = p;
                    g = hsbColor.V;
                    b = t;
                    break;

                case 3:
                    r = p;
                    g = q;
                    b = hsbColor.V;
                    break;

                case 4:
                    r = t;
                    g = p;
                    b = hsbColor.V;
                    break;

                default:
                    r = hsbColor.V;
                    g = p;
                    b = q;
                    break;
            }

            // Set WPF Color object
            rgbColor.A = hsbColor.A;
            rgbColor.R = (byte)(r * 255);
            rgbColor.G = (byte)(g * 255);
            rgbColor.B = (byte)(b * 255);

            // Set return value
            return rgbColor;
        }

        //private static Color ColorFromAhsb(byte a, double h, double s, double b)
        //{
        //    //if (0 > a || 255 < a)
        //    //{
        //    //    throw new ArgumentOutOfRangeException("a", a,
        //    //      Properties.Resources.InvalidAlpha);
        //    //}
        //    //if (0f > h || 360f < h)
        //    //{
        //    //    throw new ArgumentOutOfRangeException("h", h,
        //    //      Properties.Resources.InvalidHue);
        //    //}
        //    //if (0f > s || 1f < s)
        //    //{
        //    //    throw new ArgumentOutOfRangeException("s", s,
        //    //      Properties.Resources.InvalidSaturation);
        //    //}
        //    //if (0f > b || 1f < b)
        //    //{
        //    //    throw new ArgumentOutOfRangeException("b", b,
        //    //      Properties.Resources.InvalidBrightness);
        //    //}

        //    if (0 == s)
        //    {
        //        return Color.FromArgb(a, Convert.ToByte(b * 255),
        //          Convert.ToByte(b * 255), Convert.ToByte(b * 255));
        //    }

        //    double fMax, fMid, fMin;
        //    byte iSextant, iMax, iMid, iMin;

        //    if (0.5 < b)
        //    {
        //        fMax = b - (b * s) + s;
        //        fMin = b + (b * s) - s;
        //    }
        //    else
        //    {
        //        fMax = b + (b * s);
        //        fMin = b - (b * s);
        //    }

        //    iSextant = (byte)Math.Floor(h / 60f);
        //    if (300f <= h)
        //    {
        //        h -= 360f;
        //    }
        //    h /= 60f;
        //    h -= 2f * Math.Floor(((iSextant + 1f) % 6f) / 2f);
        //    if (0 == iSextant % 2)
        //    {
        //        fMid = h * (fMax - fMin) + fMin;
        //    }
        //    else
        //    {
        //        fMid = fMin - h * (fMax - fMin);
        //    }

        //    iMax = Convert.ToByte(fMax * 255);
        //    iMid = Convert.ToByte(fMid * 255);
        //    iMin = Convert.ToByte(fMin * 255);

        //    switch (iSextant)
        //    {
        //        case 1:
        //            return Color.FromArgb(a, iMid, iMax, iMin);
        //        case 2:
        //            return Color.FromArgb(a, iMin, iMax, iMid);
        //        case 3:
        //            return Color.FromArgb(a, iMin, iMid, iMax);
        //        case 4:
        //            return Color.FromArgb(a, iMid, iMin, iMax);
        //        case 5:
        //            return Color.FromArgb(a, iMax, iMin, iMid);
        //        default:
        //            return Color.FromArgb(a, iMax, iMid, iMin);
        //    }
        //}

    }
}
