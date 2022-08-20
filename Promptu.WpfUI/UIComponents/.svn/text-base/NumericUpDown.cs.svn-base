using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class NumericUpDown : RangeBase
    {
        private ButtonBase minusButton;
        private ButtonBase plusButton;

        public NumericUpDown()
        {
        }

        public override void OnApplyTemplate()
        {
            DependencyObject minusButtonObject = this.GetTemplateChild("PART_MinusButton");
            DependencyObject plusButtonObject = this.GetTemplateChild("PART_PlusButton");

            ButtonBase minusButton = minusButtonObject as ButtonBase;
            ButtonBase plusButton = plusButtonObject as ButtonBase;

            if (minusButtonObject != null && minusButton == null)
            {
                throw new ArgumentException("'PART_MinusButton' does not derive from ButtonBase.");
            }

            if (plusButtonObject != null && plusButton == null)
            {
                throw new ArgumentException("'PART_PlusButton' does not derive from ButtonBase.");
            }

            if (minusButton != null)
            {
                minusButton.Click += this.HandleMinusButtonClick;
            }

            if (plusButton != null)
            {
                plusButton.Click += this.HandlePlusButtonClick;
            }

            this.plusButton = plusButton;
            this.minusButton = minusButton;

            base.OnApplyTemplate();
        }

        private void HandleMinusButtonClick(object sender, EventArgs e)
        {
            this.Value -= this.LargeChange;
        }

        private void HandlePlusButtonClick(object sender, EventArgs e)
        {
            this.Value += this.LargeChange;
        }
    }
}
