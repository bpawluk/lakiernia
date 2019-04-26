using Lakiernia.Utils;
using System;
using System.Collections.Generic;

namespace Lakiernia.Model
{
    public class Material : ObiektEdytowalny
    {
        private long _id;
        private string _nazwa;
        private uint _dlugosc;
        private uint _szerokosc;

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

        public uint Dlugosc
        {
            get
            {
                return _dlugosc;
            }
            set
            {
                _dlugosc = value;
                OnPropertyChanged("Dlugosc");
            }
        }

        public uint Szerokosc
        {
            get
            {
                return _szerokosc;
            }
            set
            {
                _szerokosc = value;
                OnPropertyChanged("Szerokosc");
            }
        }

        public Material()
        {
            ID = -1;
            Nazwa = "";
            Dlugosc = 0;
            Szerokosc = 0;
        }

        public Material(string nazwa, uint dlugosc, uint szerokosc)
        {
            ID = -1;
            Nazwa = nazwa;
            Dlugosc = dlugosc;
            Szerokosc = szerokosc;
        }

        public Material(long id, string nazwa, uint dlugosc, uint szerokosc)
        {
            ID = id;
            Nazwa = nazwa;
            Dlugosc = dlugosc;
            Szerokosc = szerokosc;
        }

        public Material(Material inny)
        {
            ID = inny.ID;
            Nazwa = inny.Nazwa;
            Dlugosc = inny.Dlugosc;
            Szerokosc = inny.Szerokosc;
        }

        public void kopiuj(Material inny)
        {
            ID = inny.ID;
            Nazwa = inny.Nazwa;
            Dlugosc = inny.Dlugosc;
            Szerokosc = inny.Szerokosc;
        }
    }
}
