using Lakiernia.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lakiernia.View_Model
{
    class PodsumowanieVM : ObiektEdytowalny
    {
        private int _zawartosc;
        private ICommand _nawigujKmd;

        public int Zawartosc
        {
            get => _zawartosc;
            set
            {
                _zawartosc = value;
                OnPropertyChanged("Zawartosc");
            }
        }
        public ICommand NawigujKmd
        {
            get
            {
                if (_nawigujKmd == null) _nawigujKmd = new Komenda(Nawiguj);
                return _nawigujKmd;
            }
        }

        public PodsumowanieVM()
        {
            Zawartosc = 0;
        }

        private void Nawiguj(object parametr)
        {
            Zawartosc = (int)parametr;
        }
    }
}
