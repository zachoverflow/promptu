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

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    internal partial class ColorPicker : UserControl
    {
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                "Color",
                typeof(Color),
                typeof(ColorPicker));

        //private static readonly DependencyProperty DropDownHeightProperty =
        //    DependencyProperty.Register(
        //        "DropDownHeight",
        //        typeof(double),
        //        typeof(ColorPicker),
        //        new PropertyMetadata(HandleDropDownHeightChanged));

        public ColorPicker()
        {
            InitializeComponent();

            //this.Loaded += this.HandleLoaded;
        }

        public Color Color
        {
            get { return (Color)this.GetValue(ColorProperty); }
            set { this.SetValue(ColorProperty, value); }
        }

        //private double DropDownHeight
        //{
        //    get { return (double)this.GetValue(DropDownHeightProperty); }
        //    set { this.SetValue(DropDownHeightProperty, value); }
        //}

        //private void HandleLoaded(object sender, EventArgs e)
        //{
        //    object dropdown = ((FrameworkTemplate)this.comboBox.Template).FindName("
        //}

        //private static void HandleDropDownHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        //{
        //    double newValue = (double)e.NewValue;

        //    if (newValue != double.NaN)
        //    {
        //        ((ColorPicker)obj).DropDownHeight = double.NaN;
        //    }
        //}
    }
}
