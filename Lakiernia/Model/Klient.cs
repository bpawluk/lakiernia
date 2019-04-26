using Lakiernia.Utils;
using System;
using System.Collections.Generic;

namespace Lakiernia.Model
{
    public class Klient : ObiektEdytowalny
    {
        private long _id;
        private string _nazwa;
        private string _telefon;
        private string _email;
        private string _ulica;
        private string _numer;
        private string _miasto;
        private string _kod;
        private string _nip;
        private TypKlienta _typ;

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

        public string Nazwa
        {
            get
            {
                return _nazwa;
            }
            set
            {
                _nazwa = value;
                OnPropertyChanged("Nazwa");
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

        public TypKlienta Typ
        {
            get
            {
                return _typ;
            }
            set
            {
                _typ = value;
                OnPropertyChanged("Typ");
            }
        }

        public Klient()
        {
            ID = -1;
            Nazwa = "";
            Telefon = "";
            Email = "";
            Ulica = "";
            Numer = "";
            Miasto = "";
            Kod = "";
            Nip = "";
            Typ = TypKlienta.Osoba;
        }

        public Klient(string nazwa, string telefon, string email, string ulica, string numer, string miasto, string kod, string nip, TypKlienta typ)
        {
            ID = -1;
            Nazwa = nazwa;
            Telefon = telefon;
            Email = email;
            Ulica = ulica;
            Numer = numer;
            Miasto = miasto;
            Kod = kod;
            Nip = nip;
            Typ = typ;
        }

        public Klient(long id, string nazwa, string telefon, string email, string ulica, string numer, string miasto, string kod, string nip, TypKlienta typ)
        {
            ID = id;
            Nazwa = nazwa;
            Telefon = telefon;
            Email = email;
            Ulica = ulica;
            Numer = numer;
            Miasto = miasto;
            Kod = kod;
            Nip = nip;
            Typ = typ;
        }

        public Klient(Klient inny)
        {
            ID = inny.ID;
            Nazwa = inny.Nazwa;
            Telefon = inny.Telefon;
            Email = inny.Email;
            Ulica = inny.Ulica;
            Numer = inny.Numer;
            Miasto = inny.Miasto;
            Kod = inny.Kod;
            Nip = inny.Nip;
            Typ = inny.Typ;
        }
    }

    public enum TypKlienta { Osoba = 0, Firma = 1 };
}
