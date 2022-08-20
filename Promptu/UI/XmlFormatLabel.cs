using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ZachJohnson.Promptu.UI
{
    internal class XmlFormatLabel : Control
    {
        private XmlDocument document;
        private static readonly SizeF InfiniteSizeF = new SizeF(float.PositiveInfinity, float.PositiveInfinity);
        //private Graphics lastGraphics = null;
        private Bitmap buffer = null;
        private bool bufferInvalid = true;
        private List<Bitmap> images = new List<Bitmap>();
        private Dictionary<string, Rectangle> drawnImages = new Dictionary<string, Rectangle>();

        public XmlFormatLabel()
        {
            //this.Font = new Font("Tahoma", this.Font.Size);
        }

        public List<Bitmap> Images
        {
            get { return this.images; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.bufferInvalid)
            {
                this.UpdateBuffer();
            }

            if (this.buffer != null)
            {
                e.Graphics.DrawImage(this.buffer, Point.Empty);
            }

            //this.DrawOnGraphics(e.Graphics, new Size(this.Size.Width - this.Padding.Horizontal, -1));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.bufferInvalid = true;
            base.OnSizeChanged(e);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            this.bufferInvalid = true;
            base.OnPaddingChanged(e);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            this.bufferInvalid = true;
            base.OnBackColorChanged(e);
        }

        private void UpdateBuffer()
        {
            if (this.buffer != null)
            {
                this.buffer.Dispose();
            }

            this.buffer = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(this.buffer);

            g.Clear(this.BackColor);
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            this.DrawOnGraphics(g, new Size(this.Size.Width - this.Padding.Right, -1));

            g.Dispose();

            this.bufferInvalid = false;
        }

        public string HitTest(Point point)
        {
            foreach (KeyValuePair<string, Rectangle> entry in this.drawnImages)
            {
                if (entry.Value.Contains(point))
                {
                    return entry.Key;
                }
            }

            return null;
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.document = new XmlDocument();
                if (value != null)
                {
                    this.document.LoadXml(String.Format("<text>{0}</text>", value));
                }

                this.bufferInvalid = true;
            }
        }

        public static string GenerateXmlTag(string text, bool bold)
        {
            return GenerateXmlTag(text, bold, false);
        }

        public static string GenerateXmlImageTag(int index, string id, Padding margin)
        {
            return string.Format("<img index=\"{0}\" id=\"{1}\" margin=\"{2}\" />", index, EscapeXml(id), new PaddingConverter().ConvertToString(margin));
        }

        public static string GenerateXmlTag(string text)
        {
            return GenerateXmlTag(text, false);
        }

        public static string GenerateXmlTag(string text, bool bold, bool flow)
        {
            if (bold)
            {
                return string.Format("<b v=\"{0}\"{1}/>", EscapeXml(text), flow ? " flow=\"true\"" : String.Empty);
            }
            else
            {
                return string.Format("<t v=\"{0}\"{1}/>", EscapeXml(text), flow ? " flow=\"true\"" : String.Empty);
            }
        }

        private static string EscapeXml(string text)
        {
            return text.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        private void DrawOnGraphics(Graphics g, Size proposedSize)
        {
            if (this.document != null)
            {
                XmlRenderContext context = new XmlRenderContext(g, proposedSize, true);
                //this.GetPreferredSize(Size.Empty);
                //context.SizeLeft = proposedSize;
                this.drawnImages.Clear();
                //lastGraphics = g;
                context.Origin = new PointF(this.Padding.Left, this.Padding.Top);

                this.DrawNode(this.document, this.Font, new SolidBrush(this.ForeColor), context);

                //Size s = this.GetPreferredSize(proposedSize);
                //g.DrawRectangle(Pens.Brown, 0, 0, s.Width, s.Height);
                //context.Graphics.DrawRectangle(Pens.Blue, this.Padding.Left, this.Padding.Top, context.LargestWidth, context.Origin.Y + context.LargestHeightThisRow - this.Padding.Top);
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        //private static SizeF MeasureString(Graphics g, string text, Font font, SizeF bounds, StringFormat stringFormat)
        //{
        //    CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, text.Length) };

        //    stringFormat = (StringFormat)stringFormat.Clone();
        //    stringFormat.SetMeasurableCharacterRanges(ranges);

        //    Region[] regions = g.MeasureCharacterRanges(text, font, new RectangleF(new PointF(), bounds), stringFormat);

        //    return regions[0].GetBounds(g).Size + new Size(1, 1);
        //}

        private static SizeF MeasureString(Graphics g, string text, Font font, SizeF bounds, StringFormat stringFormat)
        {
            return g.MeasureString(text, font, bounds, stringFormat) + new Size(1, 1);
            //CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, text.Length) };

            //stringFormat = (StringFormat)stringFormat.Clone();
            //stringFormat.SetMeasurableCharacterRanges(ranges);

            //Region[] regions = g.MeasureCharacterRanges(text, font, new RectangleF(new PointF(), bounds), stringFormat);

            //return regions[0].GetBounds(g).Size;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (this.document != null)
            {
                int width = proposedSize.Width;
                int height = proposedSize.Height;

                if (width <= 0)
                {
                    width = -1;
                }
                else
                {
                    width -= this.Padding.Right;
                    if (width < 0)
                    {
                        width = 0;
                    }
                }

                if (height <= 0)
                {
                    height = -1;
                }
                else
                {
                    height -= this.Padding.Bottom;
                    if (height < 0)
                    {
                        height = 0;
                    }
                }

                proposedSize = new Size(width, height);

                //bool dispose;
                Graphics g;
                //if (this.lastGraphics != null)
                //{
                //    g = lastGraphics;
                //    dispose = false;
                //}
                //else
                //{
                    g = this.CreateGraphics();
                    //dispose = true;
                //}
                //Graphics g = this.CreateGraphics();
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                XmlRenderContext context = new XmlRenderContext(g, proposedSize, false);
                context.Origin = new PointF(this.Padding.Left, this.Padding.Top);

                this.DrawNode(this.document, this.Font, new SolidBrush(this.ForeColor), context);

                if (!context.LastRenderWasBr)
                {
                    if (context.WidthThisRow > context.LargestWidth)
                    {
                        context.LargestWidth = context.WidthThisRow;
                    }

                    context.WidthThisRow = 0;
                }

                //if (dispose)
                //{
                    g.Dispose();
                //}

                return new Size((int)Math.Ceiling(context.LargestWidth) + this.Padding.Horizontal, (int)Math.Ceiling(context.Origin.Y + context.LargestHeightThisRow) + this.Padding.Bottom);
            }

            return base.GetPreferredSize(proposedSize);
        }

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    base.OnMouseDown(e);
        //    string id = this.HitTest(e.Location);
        //    if (id != null)
        //    {
        //        MessageBox.Show("You hit:" + id);
        //    }
        //}

        //private void SizeNode(XmlNode node, Font currentFont, SolidBrush brush, XmlRenderContext context)
        //{
        //    float? oldDivX = null;
        //    context.LastRenderWasBr = false;
        //    switch (node.Name.ToUpperInvariant())
        //    {
        //        case "B":
        //            currentFont = new Font(currentFont, currentFont.Style | FontStyle.Bold);
        //            break;
        //        case "DIV":
        //            oldDivX = context.CurrentDivX;
        //            context.CurrentDivX = context.Origin.X;
        //            break;
        //        case "BR":
        //            context.LastRenderWasBr = true;
        //            context.Origin = new PointF(this.Padding.Left, context.Origin.Y + context.LargestHeightThisRow);
        //            context.LargestHeightThisRow = 0;

        //            if (context.WidthThisRow > context.LargestWidth)
        //            {
        //                context.LargestWidth = context.WidthThisRow;
        //            }

        //            context.WidthThisRow = 0;

        //            return;
        //        default:
        //            break;
        //    }

        //    if (node.Attributes != null)
        //    {
        //        foreach (XmlAttribute atrribute in node.Attributes)
        //        {
        //            if (atrribute.Name.ToUpperInvariant() == "V")
        //            {
        //                StringFormat stringFormat = (StringFormat)StringFormat.GenericTypographic.Clone();
        //                SizeF sizeFLeft = context.SizeFLeft;
        //                SizeF space = MeasureString(context.Graphics, Unescape(atrribute.Value), currentFont, sizeFLeft, stringFormat);

        //                context.WidthThisRow += (int)space.Width;

        //                int widthLeft = context.SizeLeft.Width - (int)space.Width;
        //                int heightLeft = context.SizeLeft.Height - (int)space.Height;
        //                if (context.SizeLeft.Width < 0)
        //                {
        //                    widthLeft = context.SizeLeft.Width;
        //                }
        //                else if (widthLeft < 0)
        //                {
        //                    widthLeft = 0;
        //                }

        //                if (context.SizeLeft.Height < 0)
        //                {
        //                    heightLeft = context.SizeLeft.Height;
        //                }
        //                else if (heightLeft < 0)
        //                {
        //                    heightLeft = 0;
        //                }

        //                context.SizeLeft = new Size(widthLeft, heightLeft);
        //                context.Origin = new PointF(context.Origin.X + space.Width, context.Origin.Y);

        //                if (context.LargestHeightThisRow < space.Height)
        //                {
        //                    context.LargestHeightThisRow = (int)space.Height;
        //                }


        //            }
        //        }
        //    }

        //    foreach (XmlNode innerNode in node.ChildNodes)
        //    {
        //        this.SizeNode(innerNode, currentFont, brush, context);
        //    }

        //    if (oldDivX != null)
        //    {
        //        context.Origin = new PointF(oldDivX.Value, context.Origin.Y);
        //    }
        //}

        private static string Unescape(string s)
        {
            return s.Replace("#br!", Environment.NewLine);
        }

        private void DrawNode(XmlNode node, Font currentFont, SolidBrush brush, XmlRenderContext context)
        {
            float? oldDivX = null;
            context.LastRenderWasBr = false;
            bool flow = false;

            if (node.Attributes != null)
            {
                foreach (XmlAttribute atrribute in node.Attributes)
                {
                    if (atrribute.Name.ToUpperInvariant() == "FLOW")
                    {
                        try
                        {
                            flow = Convert.ToBoolean(atrribute.Value);
                        }
                        catch (FormatException)
                        {
                        }
                    }
                }
            }

            switch (node.Name.ToUpperInvariant())
            {
                //case "FLOW":
                //    flow = true;
                //    break;
                case "B":
                    currentFont = new Font(currentFont, currentFont.Style | FontStyle.Bold);
                    break;
                case "DIV":
                    oldDivX = context.CurrentDivX;
                    context.CurrentDivX = context.Origin.X;
                    break;
                case "BR":
                    //context.Origin = new PointF(this.Padding.Left, context.Origin.Y + context.LargestHeightThisRow);
                    //context.LargestHeightThisRow = 0;
                    //context.LastRenderWasBr = true;

                    context.LastRenderWasBr = true;
                    context.Origin = new PointF(this.Padding.Left + context.CurrentDivX, context.Origin.Y + context.LargestHeightThisRow);
                    context.LargestHeightThisRow = 0;

                    if (context.WidthThisRow > context.LargestWidth)
                    {
                        context.LargestWidth = context.WidthThisRow;
                    }

                    context.WidthThisRow = context.CurrentDivX;

                    return;
                case "IMG":
                    int imageIndex = -1;
                    string id = null;
                    Padding margin = new Padding(0);

                    if (node.Attributes != null)
                    {
                        foreach (XmlAttribute atrribute in node.Attributes)
                        {
                            switch (atrribute.Name.ToUpperInvariant())
                            {
                                case "ID":
                                    id = atrribute.Value;
                                    break;
                                case "INDEX":
                                    try
                                    {
                                        imageIndex = Convert.ToInt32(atrribute.Value);
                                    }
                                    catch (FormatException)
                                    {
                                    }
                                    catch (OverflowException)
                                    {
                                    }

                                    break;
                                case "MARGIN":
                                    try
                                    {
                                        margin = (Padding)new PaddingConverter().ConvertFromString(atrribute.Value);
                                    }
                                    catch (NotSupportedException)
                                    {
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (imageIndex >= 0 && imageIndex < this.images.Count)
                    {
                        Bitmap bitmap = this.images[imageIndex];
                        if (bitmap != null)
                        {
                            SizeF sizeFLeft = context.CalculateSizeFLeft();
                            SizeF space = new SizeF(bitmap.Width + margin.Horizontal, bitmap.Height + margin.Vertical);

                            bool mustFlow = sizeFLeft.Width < 0 || sizeFLeft.Height < 0;
                            if ((flow || mustFlow) && sizeFLeft != InfiniteSizeF)
                            {
                                context.LastRenderWasBr = true;
                                context.Origin = new PointF(this.Padding.Left + context.CurrentDivX, context.Origin.Y + context.LargestHeightThisRow);
                                context.LargestHeightThisRow = 0;

                                if (context.WidthThisRow > context.LargestWidth)
                                {
                                    context.LargestWidth = context.WidthThisRow;
                                }

                                context.WidthThisRow = context.CurrentDivX;

                                sizeFLeft = context.CalculateSizeFLeft();
                            }

                            if (context.ActuallyRender)
                            {
                                Point renderOrigin = new Point((int)(context.Origin.X + margin.Left), (int)(context.Origin.Y + margin.Top));
                                context.Graphics.DrawImage(bitmap, renderOrigin.X, renderOrigin.Y, bitmap.Width, bitmap.Height);
                                if (id != null)
                                {
                                    this.drawnImages.Add(id, new Rectangle(renderOrigin, bitmap.Size));
                                }
                            }

                            context.WidthThisRow += space.Width;

                            context.Origin = new PointF(context.Origin.X + space.Width, context.Origin.Y);

                            if (context.LargestHeightThisRow < space.Height)
                            {
                                context.LargestHeightThisRow = space.Height;
                            }
                        }
                    }

                    return;
                default:
                    break;
            }

            if (node.Attributes != null)
            {
                foreach (XmlAttribute atrribute in node.Attributes)
                {
                    if (atrribute.Name.ToUpperInvariant() == "V")
                    {
                        string value = Unescape(atrribute.Value);
                        StringFormat stringFormat = (StringFormat)StringFormat.GenericTypographic.Clone();
                        SizeF sizeFLeft = context.CalculateSizeFLeft();
                        SizeF space = MeasureString(context.Graphics, value, currentFont, sizeFLeft, stringFormat);
                        bool mustFlow = sizeFLeft.Width < 0 || sizeFLeft.Height < 0;
                        if ((flow || mustFlow) && sizeFLeft != InfiniteSizeF)
                        {
                            SizeF preferredSpace = MeasureString(context.Graphics, value, currentFont, InfiniteSizeF, stringFormat);

                            //float compareWidth = space.Width;

                            //if (context.ActuallyRender)
                            //{
                            //    compareWidth += 3;
                            //    MessageBox.Show(
                            //}

                            if (preferredSpace.Width > space.Width || mustFlow)
                            {
                                context.LastRenderWasBr = true;
                                context.Origin = new PointF(this.Padding.Left + context.CurrentDivX, context.Origin.Y + context.LargestHeightThisRow);
                                context.LargestHeightThisRow = 0;

                                if (context.WidthThisRow > context.LargestWidth)
                                {
                                    context.LargestWidth = context.WidthThisRow;
                                }

                                context.WidthThisRow = context.CurrentDivX;

                                sizeFLeft = context.CalculateSizeFLeft();

                                space = MeasureString(context.Graphics, value, currentFont, sizeFLeft, stringFormat);
                            }
                        }

                        if (context.ActuallyRender)
                        {
                            context.Graphics.DrawString(value, currentFont, brush, new RectangleF(context.Origin.X, context.Origin.Y, space.Width, space.Height), stringFormat);
                            //context.Graphics.DrawRectangle(Pens.Red, context.Origin.X, context.Origin.Y, space.Width, space.Height);
                        }

                        context.WidthThisRow += space.Width;

                        //int widthLeft = context.SizeLeft.Width - (int)space.Width;
                        //int heightLeft = context.SizeLeft.Height - (int)space.Height;
                        //if (context.SizeLeft.Width < 0)
                        //{
                        //    widthLeft = context.SizeLeft.Width;
                        //}
                        //else if (widthLeft < 0)
                        //{
                        //    widthLeft = 0;
                        //}

                        //if (context.SizeLeft.Height < 0)
                        //{
                        //    heightLeft = context.SizeLeft.Height;
                        //}
                        //else if (heightLeft < 0)
                        //{
                        //    heightLeft = 0;
                        //}

                        //context.SizeLeft = new Size(widthLeft, heightLeft);
                        context.Origin = new PointF(context.Origin.X + space.Width, context.Origin.Y);

                        if (context.LargestHeightThisRow < space.Height)
                        {
                            context.LargestHeightThisRow = space.Height;
                        }
                    }
                }
            }

            foreach (XmlNode innerNode in node.ChildNodes)
            {
                this.DrawNode(innerNode, currentFont, brush, context);
            }

            if (oldDivX != null)
            {
                context.CurrentDivX = oldDivX.Value;
            }
        }
    }
}