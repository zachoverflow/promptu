using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class GridLengthAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty
            = DependencyProperty.Register(
            "From",
            typeof(GridLength),
            typeof(GridLengthAnimation));

        public static readonly DependencyProperty ToProperty
            = DependencyProperty.Register(
            "To",
            typeof(GridLength),
            typeof(GridLengthAnimation));

        public GridLengthAnimation()
        {
        }

        public GridLength From
        {
            get { return (GridLength)this.GetValue(FromProperty); }
            set { this.SetValue(FromProperty, value); }
        }

        public GridLength To
        {
            get { return (GridLength)this.GetValue(ToProperty); }
            set { this.SetValue(ToProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public override Type TargetPropertyType
        {
            get { return typeof(GridLength); }
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            double fromValue = this.From.Value;
            double toValue = this.To.Value;

            if (fromValue > toValue)
            {
                return new GridLength((1 - animationClock.CurrentProgress.Value) * (fromValue - toValue) + toValue, this.To.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
            else
            {
                return new GridLength((animationClock.CurrentProgress.Value) * (toValue - fromValue) + fromValue, this.To.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
        }
    }
}
