using Lakiernia.Data_Access;
using Lakiernia.Utils;
using System;
using System.Collections.ObjectModel;

namespace Lakiernia.Model
{
    public class Zamowienie : ObiektEdytowalny
    {
        private long _id;
        private Klient _klient;
        private DateTime _dataZlozenia;
        private DateTime _dataOdbioru;
        private ObservableCollection<Pozycja> _pozycje;
        private int _czyZakonczone;

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

        public Klient Klient
        {
            get
            {
                return _klient;
            }
            set
            {
                _klient = value;
                OnPropertyChanged("Klient");
            }
        }

        public DateTime DataZlozenia
        {
            get
            {
                return _dataZlozenia;
            }
            set
            {
                _dataZlozenia = value;
                OnPropertyChanged("DataZlozenia");
            }
        }

        public DateTime DataOdbioru
        {
            get
            {
                return _dataOdbioru.Date;
            }
            set
            {
                _dataOdbioru = value;
                OnPropertyChanged("DataOdbioru");
            }
        }

        public ObservableCollection<Pozycja> Pozycje
        {
            get
            {
                return _pozycje;
            }
            set
            {
                _pozycje = value;
                OnPropertyChanged("Pozycje");
            }
        }

        public int LiczbaPozycji { get => _pozycje.Count; }

        public int CzyZakonczone
        {
            get => _czyZakonczone;
            set
            {
                _czyZakonczone = value;
                OnPropertyChanged("CzyZakonczone");
            }
        }

        public Zamowienie()
        {
            _id = -1;
            _klient = new Klient();
            _dataZlozenia = DateTime.Now;
            _dataOdbioru = DateTime.Now;
            _pozycje = new ObservableCollection<Pozycja>();
            CzyZakonczone = 0;
        }

        public Zamowienie(Klient k, long dz, long dod, int cZ = 0)
        {
            _id = -1;
            _klient = k;
            _dataZlozenia = new DateTime(dz, DateTimeKind.Utc).ToLocalTime();
            _dataOdbioru = new DateTime(dod, DateTimeKind.Utc).ToLocalTime();
            _pozycje = new ObservableCollection<Pozycja>();
            _czyZakonczone = cZ;
        }

        public Zamowienie(Klient k, DateTime dz, DateTime dod, int cZ = 0)
        {
            _id = -1;
            _klient = k;
            _dataZlozenia = dz;
            _dataOdbioru = dod;
            _pozycje = new ObservableCollection<Pozycja>();
            _czyZakonczone = cZ;
        }

        public Zamowienie(long id, Klient k, long dz, long dod, int cZ = 0)
        {
            _id = id;
            _klient = k;
            _dataZlozenia = new DateTime(dz, DateTimeKind.Utc).ToLocalTime();
            _dataOdbioru = new DateTime(dod, DateTimeKind.Utc).ToLocalTime();
            using (PozycjaDAO bd = new PozycjaDAO()) _pozycje = bd.Pobierz("IdZam = " + _id);
            _czyZakonczone = cZ;
        }

        public Zamowienie(long id, Klient k, DateTime dz, DateTime dod, int cZ = 0)
        {
            _id = id;
            _klient = k;
            _dataZlozenia = dz;
            _dataOdbioru = dod;
            using (PozycjaDAO bd = new PozycjaDAO()) _pozycje = bd.Pobierz("IdZam = " + _id);
            _czyZakonczone = cZ;
        }
    }
}
