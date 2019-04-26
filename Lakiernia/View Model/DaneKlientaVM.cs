using Lakiernia.Data_Access;
using Lakiernia.Model;
using Lakiernia.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lakiernia.View_Model
{
    public class DaneKlientaVM : ObiektEdytowalny
    {
        public event RoutedEventHandler Powrot;
        private Klient _edytowany;
        private ICommand _powrotKmd;
        private ICommand _zapiszKmd;

        public string Nazwa
        {
            get
            {
                return _edytowany.Nazwa;
            }
            set
            {
                _edytowany.Nazwa = value;
                OnPropertyChanged("Nazwa");
            }
        }

        public int Typ
        {
            get
            {

                return _edytowany.Typ == TypKlienta.Osoba ? 0 : 1;
            }
            set
            {
                _edytowany.Typ = value == 0 ? TypKlienta.Osoba : TypKlienta.Firma;
                OnPropertyChanged("Typ");
            }
        }

        public string Telefon
        {
            get
            {
                return _edytowany.Telefon;
            }
            set
            {
                _edytowany.Telefon = value;
                OnPropertyChanged("Telefon");
            }
        }

        public string Email
        {
            get
            {
                return _edytowany.Email;
            }
            set
            {
                _edytowany.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Ulica
        {
            get
            {
                return _edytowany.Ulica;
            }
            set
            {
                _edytowany.Ulica = value;
                OnPropertyChanged("Ulica");
            }
        }

        public string Numer
        {
            get
            {
                return _edytowany.Numer;
            }
            set
            {
                _edytowany.Numer = value;
                OnPropertyChanged("Numer");
            }
        }

        public string Miasto
        {
            get
            {
                return _edytowany.Miasto;
            }
            set
            {
                _edytowany.Miasto = value;
                OnPropertyChanged("Miasto");
            }
        }

        public string Kod
        {
            get
            {
                return _edytowany.Kod;
            }
            set
            {
                _edytowany.Kod = value;
                OnPropertyChanged("Kod");
            }
        }

        public string Nip
        {
            get
            {
                return _edytowany.Nip;
            }
            set
            {
                _edytowany.Nip = value;
                OnPropertyChanged("Nip");
            }
        }

        public ICommand PowrotKmd
        {
            get
            {
                if (_powrotKmd == null) _powrotKmd = new Komenda(Powroc);
                return _powrotKmd;
            }
        }

        public ICommand ZapiszKmd
        {
            get
            {
                if (_zapiszKmd == null) _zapiszKmd = new Komenda(Zapisz);
                return _zapiszKmd;
            }
        }

        public DaneKlientaVM(Klient klient)
        {
            _edytowany = klient ?? new Klient();
        }

        private void Powroc(object parametr)
        {
            Powrot?.Invoke(this, new RoutedEventArgs());
        }

        private void Zapisz(object parametr)
        {
            if (_edytowany.ID == -1)
            {
                using (KlientDAO bd = new KlientDAO()) _edytowany.ID = bd.Dodaj(_edytowany);
            }
            else
            {
                using (KlientDAO bd = new KlientDAO()) bd.Edytuj(_edytowany);
            }
            Powrot?.Invoke(this, new RoutedEventArgs());
        }
    }
}
