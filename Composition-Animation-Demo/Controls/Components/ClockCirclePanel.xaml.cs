using CompositionAnimationDemo.Models;
using CompositionAnimationDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CompositionAnimationDemo.Controls.Components
{
    public sealed partial class ClockCirclePanel : UserControl
    {
        public ClockViewModel vm = ClockViewModel.Current;
        private DispatcherTimer _timer;
        private TimeSpan _ts;
        private Compositor _compositor;
        public ClockCirclePanel()
        {
            this.InitializeComponent();
            _compositor = Window.Current.Compositor;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            TimeSpan targetTs = TimeSpan.FromMinutes(0.5);
            vm.TargetSpan = targetTs;
            vm.InitClockScalarCollection(targetTs);
            TimeDisplay.Text = (vm.TargetSpan - _ts).ToString(@"mm\:ss");
        }

        private void Timer_Tick(object sender, object e)
        {
            if (_ts >= vm.TargetSpan)
            {
                _timer.Stop();
                VisualStateManager.GoToState(this, nameof(StopState), false);
                _ts = TimeSpan.FromSeconds(0);
            }
            else
            {
                _ts = _ts.Add(TimeSpan.FromSeconds(1));
                TimeDisplay.Text = (vm.TargetSpan - _ts).ToString(@"mm\:ss");
            }
            var data = vm.CheckCurrentTime(_ts);
            HighlightSelectedItem(data);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _ts = TimeSpan.FromSeconds(0);
            _timer.Start();
            VisualStateManager.GoToState(this, nameof(StartState), false);
        }

        private void HighlightSelectedItem(ClockScalar data)
        {
            
            var ele = ItemsRepeater.TryGetElement(data.Index);
            var visual = ElementCompositionPreview.GetElementVisual(ele);
            if (visual.Scale.X > 1)
                return;
            else
            {
                visual.CenterPoint = new System.Numerics.Vector3(visual.Size.X / 2f, visual.Size.Y / 2f, 0f);
                var animate = _compositor.CreateVector3KeyFrameAnimation();
                animate.Duration = TimeSpan.FromSeconds(0.5);
                animate.InsertKeyFrame(0f, visual.Scale);
                animate.InsertKeyFrame(1f, visual.Scale * 1.3f);
                visual.StartAnimation("Scale", animate);
            }

            for (int i = 0; i < vm.ClockScalarCollection.Count; i++)
            {
                if (data.Index != i)
                {
                    var tempEle = ItemsRepeater.TryGetElement(i);
                    var tempVisual = ElementCompositionPreview.GetElementVisual(tempEle);
                    if (tempVisual.Scale.X > 1)
                    {
                        tempVisual.CenterPoint = new System.Numerics.Vector3(tempVisual.Size.X / 2f, tempVisual.Size.Y / 2f, 0f);
                        var animate = _compositor.CreateVector3KeyFrameAnimation();
                        animate.Duration = TimeSpan.FromSeconds(0.5);
                        animate.InsertKeyFrame(0f, tempVisual.Scale);
                        animate.InsertKeyFrame(1f, new System.Numerics.Vector3(1f, 1f, 0f));
                        tempVisual.StartAnimation("Scale", animate);
                    }
                }
            }
        }
    }
}
