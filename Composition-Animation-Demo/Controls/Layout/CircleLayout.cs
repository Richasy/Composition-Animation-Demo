﻿using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CompositionAnimationDemo.Controls.Layout
{
    public class CircleLayout : VirtualizingLayout
    {
        private double _radius;
        public CircleLayout()
        {

        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(CircleLayout), new PropertyMetadata(5d, new PropertyChangedCallback(OnRadiusChanged)));

        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CircleLayout;
            if (e.NewValue is double rad && rad > 0)
            {
                instance._radius = rad;
                instance.InvalidateArrange();
            }
        }

        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            base.InitializeForContextCore(context);
        }

        protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
        {
            base.UninitializeForContextCore(context);

            // clear any state
            context.LayoutState = null;
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            // Current angle, from zero.
            double angle = 0;

            // Calc each child element angle
            double childAngle = 360d / context.ItemCount;

            // Get center
            double centerX = finalSize.Width / 2;
            double centerY = finalSize.Height / 2;

            // Arrange
            for (int i = 0; i < context.ItemCount; i++)
            {
                var child = context.GetOrCreateElementAt(i);
                double radian = Math.PI * angle / 180;
                // Child element position
                double childX = Math.Cos(radian) * this._radius;
                double childY = Math.Sin(radian) * this._radius;

                child.RenderTransformOrigin = new Point(0.5, 0.5);
                var transform = new RotateTransform();
                transform.Angle = angle;

                child.RenderTransform = transform;

                double startX = centerX + childX - child.DesiredSize.Width / 2;
                double startY = centerY + childY - child.DesiredSize.Height / 2;

                child.Arrange(new Rect(startX, startY, child.DesiredSize.Width, child.DesiredSize.Height));

                angle += childAngle;
            }

            return finalSize;
        }

        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            double maxChildWidth = 0;
            for (int i = 0; i < context.ItemCount; i++)
            {
                var child = context.GetOrCreateElementAt(i);
                child.Measure(availableSize);
                maxChildWidth = Math.Max(maxChildWidth, child.DesiredSize.Width);
            }

            double designPanelWidth = (_radius * 2) + (maxChildWidth * 2);

            double actualWidth = Math.Min(designPanelWidth, availableSize.Width);
            double actualHeight = Math.Min(designPanelWidth, availableSize.Height);

            return new Size(actualWidth, actualHeight);
        }
    }
}
