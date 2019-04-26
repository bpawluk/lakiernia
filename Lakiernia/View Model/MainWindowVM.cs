using Lakiernia.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Lakiernia.View_Model
{
    public class MainWindowVM : ObiektEdytowalny
    {
        private double _szerokoscMenu = 200;
        private ICommand _schowajKmd;

        public double SzerokoscMenu
        {
            get => _szerokoscMenu;
            set
            {
                _szerokoscMenu = value;
                OnPropertyChanged("SzerokoscMenu");
            }
        }

        public ICommand SchowajKmd
        {
            get
            {
                if (_schowajKmd == null) _schowajKmd = new Komenda(Schowaj);
                return _schowajKmd;
            }
        }

        private void Schowaj(object parametr)
        {
            SzerokoscMenu = SzerokoscMenu == 200 ? SzerokoscMenu = 10 : SzerokoscMenu = 200;
        }
    }
}
