using Lakiernia.Utils;
using System;
using System.Collections.Generic;

namespace Lakiernia.Model
{
    public class Farba : ObiektEdytowalny
    {
        private long _id;
        private string _kolor;
        private string _producent;
        private double _ilosc;

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

        public string Kolor
        {
            get
            {
                return _kolor;
            }
            set
            {
                _kolor = value;
                OnPropertyChanged("Kolor");
            }
        }

        public string Producent
        {
            get
            {
                return _producent;
            }
            set
            {
                _producent = value;
                OnPropertyChanged("Producent");
            }
        }

        public double Ilosc
        {
            get
            {
                return _ilosc;
            }
            set
            {
                _ilosc = value;
                OnPropertyChanged("Ilosc");
            }
        }

        public Farba()
        {
            ID = -1;
            Kolor = "";
            Producent = "";
            Ilosc = 0;
        }

        public Farba(string kolor, string producent, double ilosc)
        {
            ID = -1;
            Kolor = kolor;
            Producent = producent;
            Ilosc = ilosc;
        }

        public Farba(int id, string kolor, string producent, double ilosc)
        {
            ID = id;
            Kolor = kolor;
            Producent = producent;
            Ilosc = ilosc;
        }

        public Farba(Farba inna)
        {
            ID = inna.ID;
            Kolor = inna.Kolor;
            Producent = inna.Producent;
            Ilosc = inna.Ilosc;
        }

        public void Kopiuj(Farba inna)
        {
            ID = inna.ID;
            Kolor = inna.Kolor;
            Producent = inna.Producent;
            Ilosc = inna.Ilosc;
        }
    }
}
