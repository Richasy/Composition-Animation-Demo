using CompositionAnimationDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositionAnimationDemo.ViewModels
{
    public class ClockViewModel
    {
        public ObservableCollection<ButtonGroupItemBase> ClockViewButtonCollection;

        public ObservableCollection<ClockScalar> ClockScalarCollection;

        public static ClockViewModel Current;

        public TimeSpan TargetSpan { get; set; }

        public ClockViewModel()
        {
            Current = this;
            ClockViewButtonCollection = new ObservableCollection<ButtonGroupItemBase>();
            ClockScalarCollection = new ObservableCollection<ClockScalar>();

            InitButtonCollection();
        }

        public void InitClockScalarCollection(TimeSpan span, int count = 12)
        {
            if (count < 2 || count > 30)
                throw new ArgumentException("Count range is 2 to 30");
            var step = span / count;
            ClockScalarCollection.Clear();
            for (int i = 0; i < count; i++)
            {
                var data = new ClockScalar(step.TotalSeconds * i);
                data.Index = i;
                ClockScalarCollection.Add(data);
            }
            ClockScalarCollection.First().IsCheck = true;
        }

        private void InitButtonCollection()
        {
            ClockViewButtonCollection.Add(new ButtonGroupItemBase("Ring"));
            ClockViewButtonCollection.Add(new ButtonGroupItemBase("Card"));
        }

        public ClockScalar CheckCurrentTime(TimeSpan current)
        {
            foreach (var item in ClockScalarCollection)
            {
                item.IsCheck = item.Moment <= current.TotalSeconds;
            }
            return ClockScalarCollection.Last(p => p.IsCheck);
        }
    }
}
