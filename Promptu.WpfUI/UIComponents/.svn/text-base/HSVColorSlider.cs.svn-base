using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class HSVColorSlider : Control
    {
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                "Color",
                typeof(Color),
                typeof(HSVColorSlider),
                new PropertyMetadata(HandleColorPropertyChanged));

        //public static readonly DependencyProperty SaturationBrushProperty =
        //    DependencyProperty.Register(
        //        "SaturationBrush",
        //        typeof(LinearGradientBrush),
        //        typeof(ColorPickerControl));

        //public static readonly DependencyProperty ValueBrushProperty =
        //    DependencyProperty.Register(
        //        "ValueBrush",
        //        typeof(LinearGradientBrush),
        //        typeof(ColorPickerControl));

        private Slider hueSlider;
        private Slider saturationSlider;
        private Slider valueSlider;
        private bool ignoreColorChanges;

        public HSVColorSlider()
        {
        }

        public Color Color
        {
            get { return (Color)this.GetValue(ColorProperty);}
            set { this.SetValue(ColorProperty, value); }
        }

        //private LinearGradientBrush ValueBrush
        //{
        //    get { return (LinearGradientBrush)this.GetValue(ValueBrushProperty); }
        //    set { this.SetValue(ValueBrushProperty, value); }
        //}

        //private LinearGradientBrush SaturationBrush
        //{
        //    get { return (LinearGradientBrush)this.GetValue(SaturationBrushProperty); }
        //    set { this.SetValue(SaturationBrushProperty, value); }
        //}

        public override void OnApplyTemplate()
        {
            DependencyObject hueSliderObject = this.GetTemplateChild("PART_HueSlider");
            DependencyObject saturationSliderObject = this.GetTemplateChild("PART_SaturationSlider");
            DependencyObject valueSliderObject = this.GetTemplateChild("PART_ValueSlider");

            this.hueSlider = hueSliderObject as Slider;
            this.saturationSlider = saturationSliderObject as Slider;
            this.valueSlider = valueSliderObject as Slider;

            if (hueSliderObject != null && this.hueSlider == null)
            {
                throw new ArgumentException("'PART_HueSlider' is not a 'Slider'.");
            }

            if (saturationSliderObject != null && this.saturationSlider == null)
            {
                throw new ArgumentException("'PART_SaturationSlider' is not a 'Slider'.");
            }

            if (valueSliderObject != null && this.valueSlider == null)
            {
                throw new ArgumentException("'PART_ValueSlider' is not a 'Slider'.");
            }

            if (this.hueSlider != null)
            {
                this.hueSlider.Minimum = 0;
                this.hueSlider.Maximum = 360;
                this.hueSlider.ValueChanged += this.HandleHueSliderValueChanged;
            }

            if (this.saturationSlider != null)
            {
                this.saturationSlider.Minimum = 0;
                this.saturationSlider.Maximum = 1;
                this.saturationSlider.ValueChanged += this.HandleSliderValueChanged;
                this.saturationSlider.Background = new LinearGradientBrush(Colors.White, Colors.Red, 0.0);
            }

            if (this.valueSlider != null)
            {
                this.valueSlider.Minimum = 0;
                this.valueSlider.Maximum = 1;
                this.valueSlider.ValueChanged += this.HandleSliderValueChanged;
                this.valueSlider.Background = new LinearGradientBrush(Colors.Black, Colors.Red, 0.0);
            }

            this.UpdateSliders(this.Color);

            base.OnApplyTemplate();
        }

        private void HandleHueSliderValueChanged(object sender, EventArgs e)
        {
            this.HandleSliderValueChanged(sender, e);

            double hue = this.hueSlider.Value;

            Color newStandard = new HSVColor(hue, 1, 1, 255).ToColor();

            ((LinearGradientBrush)this.saturationSlider.Background).GradientStops[1].Color = newStandard;
            ((LinearGradientBrush)this.valueSlider.Background).GradientStops[1].Color = newStandard;
        }

        private void HandleSliderValueChanged(object sender, EventArgs e)
        {
            if (this.ignoreColorChanges)
            {
                return;
            }

            try
            {
                this.ignoreColorChanges = true;
                this.Color = new HSVColor(
                    this.hueSlider.Value, 
                    this.saturationSlider.Value, 
                    this.valueSlider.Value, 
                    this.Color.A).ToColor();
            }
            finally
            {
                this.ignoreColorChanges = false;
            }
        }

        private void UpdateSliders(Color color)
        {
            HSVColor hsvColor = new HSVColor(color);
            try
            {
                this.ignoreColorChanges = true;

                if (this.hueSlider != null)
                {
                    if (!double.IsNaN(hsvColor.H))
                    {
                        this.hueSlider.Value = hsvColor.H;
                    }
                }

                if (this.saturationSlider != null)
                {
                    this.saturationSlider.Value = hsvColor.S;
                }

                if (this.valueSlider != null)
                {
                    this.valueSlider.Value = hsvColor.V;
                }
            }
            finally
            {
                this.ignoreColorChanges = false;
            }
        }

        private static void HandleColorPropertyChanged(
            DependencyObject obj, 
            DependencyPropertyChangedEventArgs e)
        {
            HSVColorSlider colorPicker = obj as HSVColorSlider;
            if (colorPicker != null && !colorPicker.ignoreColorChanges)
            {
                colorPicker.UpdateSliders((Color)e.NewValue);
            }
        }
    }
}
