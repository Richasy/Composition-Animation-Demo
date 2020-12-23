using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CompositionAnimationDemo.Controls.Components
{
    public sealed class PersonCard : Control
    {
        private Compositor _compositor;
        private SpriteVisual _visual;
        private DropShadow _shadow;

        ImplicitAnimationCollection _shadowAnimationCollection;

        private float _shadowOffsetX = 2;
        private float _shadowOffsetY = 2;
        private float _shadowBlurRadius = 15;
        private float _shadowOpacity = 0.7f;

        public PersonCard()
        {
            this.DefaultStyleKey = typeof(PersonCard);
        }

        protected override void OnApplyTemplate()
        {
            var shadowHost = GetTemplateChild("ShadowHost") as Border;
            this.SizeChanged += OnSizeChanged;

            var hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            _compositor = hostVisual.Compositor;

            var dropShadow = _compositor.CreateDropShadow();
            _shadow = dropShadow;
            dropShadow.Color = Colors.Black;
            dropShadow.BlurRadius = _shadowBlurRadius;
            dropShadow.Offset = new Vector3(_shadowOffsetX, _shadowOffsetY, 0f);

            var shadowVisual = _compositor.CreateSpriteVisual();
            _visual = shadowVisual;
            shadowVisual.Shadow = dropShadow;

            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);

            _shadowAnimationCollection = _compositor.CreateImplicitAnimationCollection();

            var scaleAnimation = _compositor.CreateScalarKeyFrameAnimation();
            scaleAnimation.InsertExpressionKeyFrame(1f, "this.finalValue");
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(300);
            scaleAnimation.Target = "BlurRadius";
            _shadowAnimationCollection.Add("BlurRadius", scaleAnimation);

            var offsetAnimation = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.InsertExpressionKeyFrame(1f, "this.finalValue");
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(300);
            offsetAnimation.Target = "Offset";
            _shadowAnimationCollection.Add("Offset", offsetAnimation);

            var opacityAnimation = _compositor.CreateScalarKeyFrameAnimation();
            opacityAnimation.InsertExpressionKeyFrame(1f, "this.finalValue");
            opacityAnimation.Duration = TimeSpan.FromMilliseconds(300);
            opacityAnimation.Target = "Opacity";
            _shadowAnimationCollection.Add("Opacity", opacityAnimation);

            _shadow.ImplicitAnimations = _shadowAnimationCollection;
            base.OnApplyTemplate();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_visual != null)
            {
                Vector2 newSize = new Vector2((float)this.ActualWidth, (float)this.ActualHeight);
                _visual.Size = newSize;
            }
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Detail", true);
            _shadow.BlurRadius = _shadowBlurRadius * 1.5f;
            _shadow.Offset = new Vector3(_shadowOffsetX, _shadowOffsetY + 12f, 0f);
            _shadow.Opacity = 1f;
            base.OnPointerEntered(e);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            _shadow.BlurRadius = _shadowBlurRadius;
            _shadow.Offset = new Vector3(_shadowOffsetX, _shadowOffsetY, 0f);
            _shadow.Opacity = _shadowOpacity;
            base.OnPointerExited(e);
        }

        public ImageSource Avatar
        {
            get { return (ImageSource)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }

        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public string Position
        {
            get { return (string)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register("Avatar", typeof(ImageSource), typeof(PersonCard), new PropertyMetadata(null));

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(PersonCard), new PropertyMetadata(""));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(string), typeof(PersonCard), new PropertyMetadata(""));

        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(PersonCard), new PropertyMetadata(""));


    }
}
