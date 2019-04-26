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
    class NullNaBoolKonwerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object odwrotnie, CultureInfo culture)
        {
            bool wartosc = true;
            if (value == null && odwrotnie == null) wartosc = false;
            else if (value != null && odwrotnie != null) wartosc = false;
            return wartosc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
