using CompositionAnimationDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
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
        public ClockCirclePanel()
        {
            this.InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            TimeSpan targetTs = TimeSpan.FromMinutes(1);
            vm.TargetSpan = targetTs;
            vm.InitClockScalarCollection(targetTs);
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
            vm.CheckCurrentTime(_ts);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _ts = TimeSpan.FromSeconds(0);
            _timer.Start();
            VisualStateManager.GoToState(this, nameof(StartState), false);
        }

        private void ItemsRepeater_ElementPrepared(Microsoft.UI.Xaml.Controls.ItemsRepeater sender, Microsoft.UI.Xaml.Controls.ItemsRepeaterElementPreparedEventArgs args)
        {

        }

        private void ItemsRepeater_ElementClearing(Microsoft.UI.Xaml.Controls.ItemsRepeater sender, Microsoft.UI.Xaml.Controls.ItemsRepeaterElementClearingEventArgs args)
        {

        }

        private void ItemsRepeater_ElementIndexChanged(Microsoft.UI.Xaml.Controls.ItemsRepeater sender, Microsoft.UI.Xaml.Controls.ItemsRepeaterElementIndexChangedEventArgs args)
        {

        }
    }
}
