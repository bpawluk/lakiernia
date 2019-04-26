using Lakiernia.Utils;
using System;

namespace Lakiernia.Model
{
    public class Pozycja : ObiektEdytowalny
    {
        private long _id;
        private long _idZamowienia;
        private Material _material;
        private Farba _farba;
        private decimal _cena;
        private uint _rabat;
        private uint _vat;
        private uint _liczba;

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

        public long IDZamowienia
        {
            get
            {
                return _idZamowienia;
            }
            set
            {
                _idZamowienia = value;
                OnPropertyChanged("IDZamowienia");
            }
        }

        public Material Material
        {
            get
            {
                return _material;
            }
            set
            {
                _material = value;
                OnPropertyChanged("Material","Nazwa","Dlugosc","Szerokosc");
            }
        }

        public Farba Farba
        {
            get
            {
                return _farba;
            }
            set
            {
                _farba = value;
                OnPropertyChanged("Farba","Kolor","Producent","Ilosc");
            }
        }

        public decimal Cena
        {
            get
            {
                return _cena;
            }
            set
            {
                _cena = value;
                OnPropertyChanged("Cena");
            }
        }

        public uint Rabat
        {
            get
            {
                return _rabat;
            }
            set
            {
                _rabat = value;
                OnPropertyChanged("Rabat");
            }
        }

        public uint VAT
        {
            get
            {
                return _vat;
            }
            set
            {
                _vat = value;
                OnPropertyChanged("VAT");
            }
        }

        public uint Liczba
        {
            get
            {
                return _liczba;
            }
            set
            {
                _liczba = value;
                OnPropertyChanged("Liczba");
            }
        }

        public Pozycja()
        {
            _id = -1;
            _idZamowienia = -1;
            _material = null;
            _farba = null;
            _cena = 0m;
            _rabat = 0;
            _vat = 0;
            _liczba = 0;
        }

        public Pozycja(long idz, Material m, Farba f, decimal c, uint r, uint v, uint l)
        {
            _id = -1;
            _idZamowienia = idz;
            _material = m;
            _farba = f;
            _cena = c;
            _rabat = r;
            _vat = v;
            _liczba = l;
        }

        public Pozycja(long id, long idz, Material m, Farba f, decimal c, uint r, uint v, uint l)
        {
            _id = id;
            _idZamowienia = idz;
            _material = m;
            _farba = f;
            _cena = c;
            _rabat = r;
            _vat = v;
            _liczba = l;
        }

        public Pozycja(Pozycja inna)
        {
            _id = inna.ID;
            _idZamowienia = inna.IDZamowienia;
            _material = new Material(inna.Material);
            _farba = new Farba(inna.Farba);
            _cena = inna.Cena;
            _rabat = inna.Rabat;
            _vat = inna._vat;
            _liczba = inna.Liczba;
        }

        public void Kopiuj(Pozycja inna)
        {
            ID = inna.ID;
            IDZamowienia = inna.IDZamowienia;
            Material = new Material(inna.Material);
            Farba = new Farba(inna.Farba);
            Cena = inna.Cena;
            Rabat = inna.Rabat;
            VAT = inna.VAT;
            Liczba = inna.Liczba;
        }
    }
}
