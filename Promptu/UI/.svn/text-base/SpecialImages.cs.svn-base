//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using System.Drawing.Drawing2D;
//using System.Reflection;

//namespace ZachJohnson.Promptu.UI
//{
//    internal static class SpecialImages
//    {
//        private static readonly ReleaseType releaseType = ReleaseType.Release;
//        private static readonly Color fontColor = Color.Black;//Color.FromArgb(184, 176, 24);
//        private static readonly PointF versionOrigin = new Point(233, 79);//new Point(460, 79);
//        private const int versionFontSize = 19;
//        private static readonly PointF copyrightOrigin = new Point(233, 130);
//        private const int copyrightFontSize = 19;
//        private static readonly PointF stateReverseOrigin = new Point(591, 170);
//        private const int stateFontSize = 45;

//        public static Image AboutImage
//        {
//            get
//            {
//                Image image = (Image)Images.AboutBox.Clone();
//                DrawAboutInfoOn(image);
//                return image;
//            }
//        }

//        private static void DrawAboutInfoOn(Image baseImage)
//        {
//            FontFamily fontFamily = PromptuFonts.DefaultFont.FontFamily;
//            StringFormat stringFormat = new StringFormat();
//            Graphics g = Graphics.FromImage(baseImage);
//            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
//            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
//            //GraphicsPath text = new GraphicsPath();
//            Version promptuVersion = Assembly.GetExecutingAssembly().GetName().Version;
//            SolidBrush brush = new SolidBrush(fontColor);
//            //text.AddString(
//            //    String.Format(Localization.Promptu.VersionFormat, promptuVersion.Major, promptuVersion.MajorRevision),
//            //    fontFamily,
//            //    (int)FontStyle.Regular,
//            //    versionFontSize,
//            //    versionOrigin,
//            //    stringFormat);
//            g.DrawString(
//                String.Format(Localization.Promptu.VersionFormat, promptuVersion.Major, promptuVersion.Minor, promptuVersion.Build, promptuVersion.Revision),
//                new Font(fontFamily, versionFontSize),
//                brush,
//                versionOrigin);

//            int year = 2009;
//            DateTime now = DateTime.Now;
//            if (now.Year > year)
//            {
//                year = now.Year;
//            }

//            //text.AddString(
//            //    String.Format(Localization.Promptu.CopyrightFormat, year),
//            //    fontFamily,
//            //    (int)FontStyle.Regular,
//            //    copyrightFontSize,
//            //    copyrightOrigin,
//            //    stringFormat);
//            g.DrawString(
//                String.Format(Localization.Promptu.CopyrightFormat, year),
//                new Font(fontFamily, copyrightFontSize),
//                brush,
//                copyrightOrigin);

//            string releaseState = null;

//            switch (releaseType)
//            {
//                case ReleaseType.Alpha:
//                    releaseState = Localization.Promptu.Alpha;
//                    break;
//                case ReleaseType.Beta:
//                    releaseState = Localization.Promptu.Beta;
//                    break;
//                default:
//                    break;
//            }

//            if (releaseState != null)
//            {
//                SizeF size = g.MeasureString(releaseState, new Font(fontFamily, stateFontSize));
//                PointF sizeOrigin = new PointF(stateReverseOrigin.X - size.Width, stateReverseOrigin.Y);
//                g.DrawString(
//                releaseState,
//                new Font(fontFamily, stateFontSize),
//                brush,
//                sizeOrigin);
//                //text.AddString(
//                //    releaseState,
//                //    fontFamily,
//                //    (int)FontStyle.Regular,
//                //    stateFontSize,
//                //    sizeOrigin,
//                //    stringFormat);
//            }

//            //g.FillPath(new SolidBrush(fontColor), text);
//        }
//    }
//}
