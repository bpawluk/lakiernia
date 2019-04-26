using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Lakiernia.View.Converters
{
    class NullNaWidocznoscKonwerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object odwrotnie, CultureInfo culture)
        {
            Visibility widocznosc = Visibility.Visible;
            if (value == null && odwrotnie == null) widocznosc = Visibility.Collapsed;
            else if (value != null && odwrotnie != null) widocznosc = Visibility.Collapsed;
            return widocznosc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
