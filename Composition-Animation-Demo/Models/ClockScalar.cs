using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositionAnimationDemo.Models
{
    public class ClockScalar:BindableBase, IClockScalar
    {
        private bool _isCheck;
        public bool IsCheck
        {
            get => _isCheck;
            set => Set(ref _isCheck, value);
        }

        public double Moment { get; set; }

        public ClockScalar(){}

        public ClockScalar(double moment, bool isCheck=false)
        {
            this.Moment = moment;
            this.IsCheck = isCheck;
        }
    }
}
