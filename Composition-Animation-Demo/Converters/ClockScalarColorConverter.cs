using CompositionAnimationDemo.Models;
using CompositionAnimationDemo.ViewModels;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CompositionAnimationDemo.Converters
{
    public class ClockScalarColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isCheck = System.Convert.ToBoolean(value);
            var color = isCheck ? Color.FromArgb(255, 63, 140, 254)
                           : Color.FromArgb(255, 20, 24, 28);
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
