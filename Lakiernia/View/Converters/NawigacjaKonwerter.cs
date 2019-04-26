using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Lakiernia.View.Converters
{
    class NawigacjaKonwerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object odwrotnie, CultureInfo culture)
        {
            object wartoscZwracana;
            switch ((int)value)
            {
                case 0:
                    wartoscZwracana = new PodsumowanieFarb();
                    break;
                case 1:
                    wartoscZwracana = new PodsumowanieKlientow();
                    break;
                case 2:
                    wartoscZwracana = new PodsumowanieZamowien();
                    break;
                default:
                    wartoscZwracana = null;
                    break;
            }
            return wartoscZwracana;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
