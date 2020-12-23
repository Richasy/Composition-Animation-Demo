using CompositionAnimationDemo.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace CompositionAnimationDemo.Controls.Components
{
    public sealed partial class ButtonGroup : UserControl
    {
        Compositor _compositor;
        SpriteVisual _rect;
        public ButtonGroup()
        {
            this.InitializeComponent();
        }

        public event EventHandler<IButtonGroupItem> ItemClick;

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            base.OnPointerEntered(e);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            base.OnPointerExited(e);
        }

        public ObservableCollection<ButtonGroupItemBase> ItemsSource
        {
            get { return (ObservableCollection<ButtonGroupItemBase>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<ButtonGroupItemBase>), typeof(ButtonGroup), new PropertyMetadata(null));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ButtonGroup), new PropertyMetadata(Orientation.Horizontal));

        private void ItemsRepeater_ElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            var ele = args.Element as Button;
            var context = ele.Tag as ButtonGroupItemBase;
            context.SelectionIndex = args.Index;
            ele.Click -= OnItemClick;
            ele.Click += OnItemClick;
        }
        

        private void ItemsRepeater_ElementIndexChanged(ItemsRepeater sender, ItemsRepeaterElementIndexChangedEventArgs args)
        {
            var ele = args.Element as Button;
            var context = ele.Tag as ButtonGroupItemBase;
            context.SelectionIndex = args.NewIndex;
        }

        private void ItemsRepeater_ElementClearing(ItemsRepeater sender, ItemsRepeaterElementClearingEventArgs args)
        {
            var ele = args.Element as Button;
            ele.Click -= OnItemClick;
            var context = ele.Tag as ButtonGroupItemBase;
            context.SelectionIndex = -1;
        }

        private void OnItemClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var context = btn.Tag as IButtonGroupItem;
            if (_rect != null)
            {
                _rect.Size = btn.ActualSize;
                _rect.Offset = btn.ActualOffset;
                _rect.Brush = CreateRoundedBrush(btn);
            }
            ItemClick?.Invoke(this, context);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var ele = ItemsRepeater.TryGetElement(0);

            _rect = _compositor.CreateSpriteVisual();
            _rect.Brush = CreateRoundedBrush(ele);
            _rect.Size = ele.ActualSize;

            var aniCol = _compositor.CreateImplicitAnimationCollection();
            var offsetAnimation = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.InsertExpressionKeyFrame(1f, "this.finalValue");
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(300);
            offsetAnimation.Target = "Offset";
            aniCol.Add("Offset", offsetAnimation);

            _rect.ImplicitAnimations = aniCol;

            ElementCompositionPreview.SetElementChildVisual(ThumbHost, _rect);
        }

        private CompositionBrush CreateRoundedBrush(UIElement ele)
        {
            var colorBrush = _compositor.CreateColorBrush(Color.FromArgb(255, 63, 140, 254));

            CompositionRoundedRectangleGeometry roundedRectangle = _compositor.CreateRoundedRectangleGeometry();
            roundedRectangle.Size = ele.ActualSize;
            roundedRectangle.CornerRadius = new Vector2(4, 4);

            CompositionSpriteShape spriteShape = _compositor.CreateSpriteShape(roundedRectangle);
            spriteShape.FillBrush = colorBrush;
            spriteShape.CenterPoint = new Vector2(ele.ActualSize.X / 2, ele.ActualSize.Y / 2);

            ShapeVisual spriteShapeVisual = _compositor.CreateShapeVisual();
            spriteShapeVisual.BorderMode = CompositionBorderMode.Soft;
            spriteShapeVisual.Size = ele.ActualSize;
            spriteShapeVisual.Shapes.Add(spriteShape);

            CompositionVisualSurface surface = _compositor.CreateVisualSurface();
            surface.SourceSize = ele.ActualSize;
            surface.SourceVisual = spriteShapeVisual;

            CompositionMaskBrush maskBrush = _compositor.CreateMaskBrush();
            maskBrush.Source = colorBrush;
            var surfaceBrush = _compositor.CreateSurfaceBrush(surface);
            surfaceBrush.Stretch = CompositionStretch.Fill;
            maskBrush.Mask = surfaceBrush;

            return maskBrush;
        }
    }
}
