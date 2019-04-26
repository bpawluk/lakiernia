using Lakiernia.Utils;

namespace Lakiernia.Model
{
    public class Pracodawca : ObiektEdytowalny
    {
        private long _id;
        private string _imie;
        private string _nazwisko;
        private string _firma;
        private string _ulica;
        private string _numer;
        private string _miasto;
        private string _kod;
        private string _nip;
        private string _telefon;
        private string _email;
        private string _bank;
        private string _konto;

        public long ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public string Imie
        {
            get
            {
                return _imie;
            }
            set
            {
                _imie = value;
                OnPropertyChanged("Imie");
            }
        }

        public string Nazwisko
        {
            get
            {
                return _nazwisko;
            }
            set
            {
                _nazwisko = value;
                OnPropertyChanged("Nazwisko");
            }
        }

        public string Firma
        {
            get
            {
                return _firma;
            }
            set
            {
                _firma = value;
                OnPropertyChanged("Firma");
            }
        }

        public string Ulica
        {
            get
            {
                return _ulica;
            }
            set
            {
                _ulica = value;
                OnPropertyChanged("Ulica");
            }
        }

        public string Numer
        {
            get
            {
                return _numer;
            }
            set
            {
                _numer = value;
                OnPropertyChanged("Numer");
            }
        }
        public string Miasto
        {
            get
            {
                return _miasto;
            }
            set
            {
                _miasto = value;
                OnPropertyChanged("Miasto");
            }
        }
        public string Kod
        {
            get
            {
                return _kod;
            }
            set
            {
                _kod = value;
                OnPropertyChanged("Kod");
            }
        }

        public string Adres
        {
            get
            {
                return _ulica + " " + _numer + " " + _kod + " " + _miasto;
            }
        }

        public string Nip
        {
            get
            {
                return _nip;
            }
            set
            {
                _nip = value;
                OnPropertyChanged("Nip");
            }
        }

        public string Telefon
        {
            get
            {
                return _telefon;
            }
            set
            {
                _telefon = value;
                OnPropertyChanged("Telefon");
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Bank
        {
            get
            {
                return _bank;
            }
            set
            {
                _bank = value;
                OnPropertyChanged("Bank");
            }
        }

        public string Konto
        {
            get
            {
                return _konto;
            }
            set
            {
                _konto = value;
                OnPropertyChanged("Konto");
            }
        }

        public Pracodawca()
        {
            ID = -1;
            Imie = "";
            Nazwisko = "";
            Firma = "";
            Telefon = "";
            Email = "";
            Ulica = "";
            Numer = "";
            Miasto = "";
            Kod = "";
            Nip = "";
            Bank = "";
            Konto = "";
        }

        public Pracodawca(long id, string im, string na, string fi, string ul, string nu, string mi, string kod, string ni, string te, string em, string ba, string kon)
        {
            ID = id;
            Imie = im;
            Nazwisko = na;
            Firma = fi;
            Telefon = te;
            Email = em;
            Ulica = ul;
            Numer = nu;
            Miasto = mi;
            Kod = kod;
            Nip = ni;
            Bank = ba;
            Konto = kon;
        }
    }
}
