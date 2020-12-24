using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace CompositionAnimationDemo.Models
{
    public class BindableBase : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string properyName = "")
        {
            IAsyncAction result = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyName));
            });
        }
        public void Set<T>(ref T target, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(target, value))
            {
                return;
            }

            target = value;
            OnPropertyChanged(propertyName);
        }
    }
}
