using Lakiernia.View;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Lakiernia.Utils
{
    public abstract class ObiektEdytowalny : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(params string[] nazwyWłasnosci)
        {
            foreach(string wlasnosc in nazwyWłasnosci)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(wlasnosc));
            }
        }
    }
}
