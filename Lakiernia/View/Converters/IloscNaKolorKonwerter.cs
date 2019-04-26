using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Lakiernia.View.Converters
{
    class IloscNaKolorKonwerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object odwrotnie, CultureInfo culture)
        {
            double ilosc = (double)value;
            SolidColorBrush kolor = Brushes.PaleGreen;
            if (ilosc > 5 && ilosc <= 15) kolor = Brushes.PaleGoldenrod;
            else if (ilosc <= 5) kolor = Brushes.PaleVioletRed;
            return kolor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
