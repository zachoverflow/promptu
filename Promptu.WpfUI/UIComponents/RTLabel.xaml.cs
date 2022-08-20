using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using ZachJohnson.Promptu.UIModel.RichText;
using RT = ZachJohnson.Promptu.UIModel.RichText;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for XmlFormatLabel.xaml
    /// </summary>
    internal partial class RTLabel : UserControl
    {
        private RTElement displayValue;
        private Dictionary<string, ImageSourceWithPadding> images = new Dictionary<string, ImageSourceWithPadding>();

        public RTLabel()
        {
            InitializeComponent();
        }

        public event EventHandler<ImageClickEventArgs> ImageClick;

        public Dictionary<string, ImageSourceWithPadding> Images
        {
            get { return this.images; }
        }

        public RTElement DisplayValue
        {
            get
            {
                return this.displayValue; 
            }

            set
            {
                this.displayValue = value;
                this.UpdateView();
            }
        }

        private void UpdateView()
        {
            this.root.Children.Clear();

            RTElement elementToRender = this.displayValue;

            if (elementToRender == null)
            {
                return;
            }

            RTGroup group = elementToRender as RTGroup;

            if (group == null)
            {
                group = new RTGroup();
                group.Children.Add(elementToRender);
            }

            this.RenderRTGroup(group, this.root);
        }

        private void RenderRTGroup(RTGroup group, StackPanel uiElement)
        {
            DockPanel currentStack = new DockPanel();
            uiElement.Children.Add(currentStack);

            foreach (RTElement element in group.Children)
            {
                Text text = element as Text;
                RT.LineBreak lineBreak;
                KeyedImage image;
                Space space;
                RTGroup childGroup;

                if (text != null)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = text.Value;

                    textBlock.TextWrapping = TextWrapping.Wrap;
                    textBlock.FontWeight = (text.Style & TextStyle.Bold) == TextStyle.Bold ? FontWeights.Bold : FontWeights.Normal;
                    textBlock.FontStyle = (text.Style & TextStyle.Italic) == TextStyle.Italic ? FontStyles.Italic : FontStyles.Normal;

                    currentStack.Children.Add(textBlock);

                    if (text.Value.Contains(Environment.NewLine))
                    {
                        currentStack = new DockPanel();
                            //currentStack.Orientation = Orientation.Horizontal;
                        uiElement.Children.Add(currentStack);
                    }

                    DockPanel.SetDock(textBlock, Dock.Left);
                }
                else if ((lineBreak = element as RT.LineBreak) != null)
                {
                    if (currentStack.Children.Count > 0)
                    {
                        currentStack = new DockPanel();
                        //currentStack.Orientation = Orientation.Horizontal;
                        uiElement.Children.Add(currentStack);
                    }
                    else
                    {
                        //TextBlock spacer = new TextBlock();
                        //spacer.Text = " ";
                        //currentStack.Children.Add(spacer);

                        currentStack = new DockPanel();
                        //currentStack.Orientation = Orientation.Horizontal;
                        uiElement.Children.Add(currentStack);
                    }
                }
                else if ((image = element as KeyedImage) != null)
                {
                    ImageSourceWithPadding imageSourceWithPadding;
                    if (this.Images.TryGetValue(image.Key, out imageSourceWithPadding) && imageSourceWithPadding != null)
                    {
                        Image wpfImage = new Image();
                        wpfImage.Source = imageSourceWithPadding.ImageSource;

                        if (imageSourceWithPadding.Padding != null)
                        {
                            wpfImage.Margin = imageSourceWithPadding.Padding;
                        }

                        DockPanel.SetDock(wpfImage, Dock.Left);
                        wpfImage.MaxHeight = wpfImage.Source.Height;
                        wpfImage.MaxWidth = wpfImage.Source.Width;
                        currentStack.Children.Add(wpfImage);

                        wpfImage.Tag = image.Key;
                        wpfImage.MouseDown += this.HandleImageClick;
                    }
                }
                else if ((space = element as Space) != null)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = space.Width;
                    rect.Height = 0;

                    DockPanel.SetDock(rect, Dock.Left);
                    currentStack.Children.Add(rect);
                }
                else if ((childGroup = element as RTGroup) != null)
                {
                    StackPanel div = new StackPanel();

                    this.RenderRTGroup(childGroup, div);

                    DockPanel.SetDock(div, Dock.Left);
                    currentStack.Children.Add(div);
                }
            }
        }

        private void HandleImageClick(object sender, EventArgs e)
        {
            this.OnImageClick(new ImageClickEventArgs(((Image)sender).Tag as string));
        }

        protected virtual void OnImageClick(ImageClickEventArgs e)
        {
            EventHandler<ImageClickEventArgs> handler = this.ImageClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
