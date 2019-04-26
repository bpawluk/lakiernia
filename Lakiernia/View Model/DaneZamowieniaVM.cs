using Lakiernia.Data_Access;
using Lakiernia.Model;
using Lakiernia.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lakiernia.View_Model
{
    public class DaneZamowieniaVM : ObiektEdytowalny
    {
        public event RoutedEventHandler Powrot;

        private Zamowienie _zamowienie;
        private Pozycja _edytowanaPozycja;
        private Pozycja _wybranaPozycja;
        private ObservableCollection<Klient> _klienci;
        private ObservableCollection<Farba> _farby;
        private readonly string _tekstZachecajacy = "Szukaj...";
        private string _szukanyKlient;
        private string _szukanaFarba;
        private bool _dodawanieKlienta = false;
        private bool _dodawanieFarby = false;
        private ICommand _dodajKlientaKmd;
        private ICommand _dodajFarbeKmd;
        private ICommand _utworzFarbeKmd;
        private ICommand _anulujTworzenieFarbyKmd;
        private ICommand _utworzZamowienieKmd;
        private ICommand _utworzPozycjeKmd;
        private ICommand _zapiszPozycjeKmd;
        private ICommand _nowaPozycjaKmd;
        private ICommand _usunPozycjeKmd;
        private ICommand _zapiszKmd;
        private ICommand _anulujKmd;

        public string TekstZachecajacy { get => _tekstZachecajacy; }

        public string SzukanyKlient
        {
            get
            {
                return _szukanyKlient;
            }
            set
            {
                _szukanyKlient = value;
                OnPropertyChanged("SzukanyKlient");
            }
        }

        public string SzukanaFarba
        {
            get
            {
                return _szukanaFarba;
            }
            set
            {
                _szukanaFarba = value;
                OnPropertyChanged("SzukanaFarba");
            }
        }

        public Klient WybranyKlient
        {
            get
            {
                return _zamowienie.Klient;
            }
            set
            {
                _zamowienie.Klient = value;
                OnPropertyChanged("WybranyKlient");
            }
        }

        public int Typ
        {
            get
            {

                return _zamowienie.Klient.Typ == TypKlienta.Osoba ? 0 : 1;
            }
            set
            {
                _zamowienie.Klient.Typ = value == 0 ? TypKlienta.Osoba : TypKlienta.Firma;
                OnPropertyChanged("Typ");
            }
        }

        public Farba WybranaFarba
        {
            get
            {
                return _edytowanaPozycja.Farba;
            }
            set
            {
                _edytowanaPozycja.Farba = value;
                OnPropertyChanged("WybranaFarba");
            }
        }

        public Pozycja WybranaPozycja
        {
            get
            {
                return _wybranaPozycja;
            }
            set
            {
                _wybranaPozycja = value;
                OnPropertyChanged("WybranaPozycja");
            }
        }

        public Pozycja EdytowanaPozycja
        {
            get
            {
                return _edytowanaPozycja;
            }
            set
            {
                _edytowanaPozycja = value;
                OnPropertyChanged("EdytowanaPozycja");
            }
        }

        public ObservableCollection<Klient> Klienci
        {
            get
            {
                return _klienci;
            }
            private set
            {
                _klienci = value;
                OnPropertyChanged("Klienci, WybranyKlient");
            }
        }

        public ObservableCollection<Farba> Farby
        {
            get
            {
                return _farby;
            }
            private set
            {
                _farby = value;
                OnPropertyChanged("Farby, WybranaFarba");
            }
        }

        public ObservableCollection<Pozycja> Pozycje
        {
            get
            {
                return _zamowienie.Pozycje;
            }
            private set
            {
                _zamowienie.Pozycje = value;
                OnPropertyChanged("Pozycje, WybranaPozycja");
            }
        }

        public DateTime DataZlozenia
        {
            get
            {
                return _zamowienie.DataZlozenia;
            }
            set
            {
                _zamowienie.DataZlozenia = value;
                OnPropertyChanged("DataZlozenia");
            }
        }

        public DateTime DataOdbioru
        {
            get
            {
                return _zamowienie.DataOdbioru;
            }
            set
            {
                _zamowienie.DataOdbioru = value;
                OnPropertyChanged("DataOdbioru");
            }
        }

        public Zamowienie Zamowienie
        {
            get
            {
                return _zamowienie;
            }
            private set
            {
                _zamowienie = value;
                OnPropertyChanged("Zamowienie", "WybranyKlient", "DataZlozenia", "DataOdbioru");
            }
        }

        public bool DodawanieKlienta
        {
            get
            {
                return _dodawanieKlienta;
            }
            set
            {
                _dodawanieKlienta = value;
                OnPropertyChanged("DodawanieKlienta");
            }
        }

        public bool DodawanieFarby
        {
            get
            {
                return _dodawanieFarby;
            }
            set
            {
                _dodawanieFarby = value;
                OnPropertyChanged("DodawanieFarby");
            }
        }

        public ICommand DodajKlientaKmd
        {
            get
            {
                if (_dodajKlientaKmd == null) _dodajKlientaKmd = new Komenda(DodajKlienta);
                return _dodajKlientaKmd;
            }
        }

        public ICommand DodajFarbeKmd
        {
            get
            {
                if (_dodajFarbeKmd == null) _dodajFarbeKmd = new Komenda(DodajFarbe);
                return _dodajFarbeKmd;
            }
        }

        public ICommand UtworzFarbeKmd
        {
            get
            {
                if (_utworzFarbeKmd == null) _utworzFarbeKmd = new Komenda(UtworzFarbe);
                return _utworzFarbeKmd;
            }
        }

        public ICommand AnulujTworzenieFarbyKmd
        {
            get
            {
                if (_anulujTworzenieFarbyKmd == null) _anulujTworzenieFarbyKmd = new Komenda(AnulujTworzenieFarby);
                return _anulujTworzenieFarbyKmd;
            }
        }

        public ICommand UtworzZamowienieKmd
        {
            get
            {
                if (_utworzZamowienieKmd == null) _utworzZamowienieKmd = new Komenda(UtworzZamowienie, p => (WybranyKlient != null && WybranyKlient.ID > 0));
                return _utworzZamowienieKmd;
            }
        }

        public ICommand UtworzPozycjeKmd
        {
            get
            {
                if (_utworzPozycjeKmd == null) _utworzPozycjeKmd = new Komenda(UtworzPozycje);
                return _utworzPozycjeKmd;
            }
        }

        public ICommand ZapiszPozycjeKmd
        {
            get
            {
                if (_zapiszPozycjeKmd == null) _zapiszPozycjeKmd = new Komenda(ZapiszPozycje);
                return _zapiszPozycjeKmd;
            }
        }

        public ICommand NowaPozycjaKmd
        {
            get
            {
                if (_nowaPozycjaKmd == null) _nowaPozycjaKmd = new Komenda(NowaPozycja);
                return _nowaPozycjaKmd;
            }
        }

        public ICommand UsunPozycjeKmd
        {
            get
            {
                if (_usunPozycjeKmd == null) _usunPozycjeKmd = new Komenda(UsunPozycje);
                return _usunPozycjeKmd;
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

        public ICommand AnulujKmd
        {
            get
            {
                if (_anulujKmd == null) _anulujKmd = new Komenda(Anuluj);
                return _anulujKmd;
            }
        }

        public DaneZamowieniaVM(Zamowienie z)
        {
            Zamowienie = z ?? new Zamowienie();
            EdytowanaPozycja = new Pozycja() { IDZamowienia = _zamowienie.ID, Material = new Material() };
            using (KlientDAO bd = new KlientDAO()) Klienci = bd.Pobierz();
            using (FarbaDAO bd = new FarbaDAO()) Farby = bd.Pobierz();
            PropertyChanged += Odswiez;
            PropertyChanged += WybranoPozycje;
            _szukanyKlient = _tekstZachecajacy;
            _szukanaFarba = _tekstZachecajacy;
        }

        private void DodajKlienta(object parametr)
        {
            WybranyKlient = new Klient();
            OnPropertyChanged("Typ");
            DodawanieKlienta = true;
        }

        private void DodajFarbe(object parametr)
        {
            WybranaFarba = new Farba();
            DodawanieFarby = true;
        }

        private void UtworzFarbe(object parametr)
        {
            Farby.Add(WybranaFarba);
            using (FarbaDAO bd = new FarbaDAO()) WybranaFarba.ID = bd.Dodaj(WybranaFarba);
            DodawanieFarby = false;
        }

        private void AnulujTworzenieFarby(object parametr)
        {
            WybranaFarba = null;
            DodawanieFarby = false;
        }

        private void UtworzZamowienie(object parametr)
        {
            using (ZamowienieDAO bd = new ZamowienieDAO()) _zamowienie.ID = bd.Dodaj(_zamowienie);
            OnPropertyChanged("WybranaPozycja");
        }

        private void UtworzPozycje(object parametr)
        {
            DodajMaterial(_edytowanaPozycja);
            using (PozycjaDAO bd = new PozycjaDAO()) _edytowanaPozycja.ID = bd.Dodaj(_edytowanaPozycja);
            Pozycje.Add(_edytowanaPozycja);
            WybranaPozycja = null;
        }

        private void ZapiszPozycje(object parametr)
        {
            DodajMaterial(_edytowanaPozycja);
            WybranaPozycja.Kopiuj(_edytowanaPozycja);
            using (PozycjaDAO bd = new PozycjaDAO()) bd.Edytuj(_edytowanaPozycja);
        }

        private void NowaPozycja(object parametr)
        {
            WybranaPozycja = null;
        }

        private void UsunPozycje(object parametr)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć wybrany element?", "USUWANIE", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using (PozycjaDAO bd = new PozycjaDAO())
                {
                    if (bd.Usun(WybranaPozycja)) Pozycje.Remove(WybranaPozycja);
                    else MessageBox.Show("Element, który starasz się usunąć, jest powiązany z innymi elementami." +
                                         "\nNajpierw usuń wszystkie powiązane elementy.", "BŁĄD!");
                }
                WybranaPozycja = null;
            }
        }

        private void Zapisz(object parametr)
        {
            if (DodawanieKlienta)
            {
                Klienci.Add(WybranyKlient);
                using (KlientDAO bd = new KlientDAO()) WybranyKlient.ID = bd.Dodaj(WybranyKlient);
                DodawanieKlienta = false;
            }
            else
            {
                if (Zamowienie.ID <= 0) MessageBox.Show("Proszę utworzyć zamówienie");
                else
                {
                    using (ZamowienieDAO bd = new ZamowienieDAO()) bd.Edytuj(Zamowienie);
                    Powrot?.Invoke(this, new RoutedEventArgs());
                }
            }
        }

        private void Anuluj(object parametr)
        {
            if (DodawanieKlienta)
            {
                WybranyKlient = null;
                DodawanieKlienta = false;
            }
            else Powrot?.Invoke(this, new RoutedEventArgs());
        }

        private void Odswiez(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SzukanyKlient"))
            {
                ObservableCollection<Klient> sfiltrowani;
                using (KlientDAO bd = new KlientDAO())
                {
                    if (SzukanyKlient.Equals("")) sfiltrowani = bd.Pobierz();
                    else if (SzukanyKlient.Equals(_tekstZachecajacy)) sfiltrowani = null;
                    else sfiltrowani = bd.Pobierz("NazwaK like '%" + SzukanyKlient + "%'");

                    if (sfiltrowani != null)
                    {
                        long wybranyID = WybranyKlient?.ID ?? -1;
                        Klienci.Clear();
                        foreach (Klient klient in sfiltrowani) Klienci.Add(klient);
                        WybranyKlient = Klienci.Where(k => k.ID == wybranyID).FirstOrDefault();
                    }
                }
            }
            else if (e.PropertyName.Equals("SzukanaFarba"))
            {
                ObservableCollection<Farba> sfiltrowane;
                using (FarbaDAO bd = new FarbaDAO())
                {
                    if (SzukanaFarba.Equals("")) sfiltrowane = bd.Pobierz();
                    else if (SzukanaFarba.Equals(_tekstZachecajacy)) sfiltrowane = null;
                    else sfiltrowane = bd.Pobierz("Kolor like '%" + SzukanaFarba + "%'");

                    if (sfiltrowane != null)
                    {
                        long wybranaID = WybranaFarba?.ID ?? -1;
                        Farby.Clear();
                        foreach (Farba farba in sfiltrowane) Farby.Add(farba);
                        WybranaFarba = Farby.Where(f => f.ID == wybranaID).FirstOrDefault();
                    }
                }
            }
            else if (e.PropertyName.Equals("WybranaPozycja"))
            {
                SzukanaFarba = "";
                SzukanaFarba = _tekstZachecajacy;
            }
        }

        private void WybranoPozycje(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("WybranaPozycja"))
            {
                if (WybranaPozycja == null) EdytowanaPozycja = new Pozycja() { IDZamowienia = _zamowienie.ID, Material = new Material() };
                else EdytowanaPozycja = new Pozycja(WybranaPozycja);
                OnPropertyChanged("WybranaFarba");
            }
        }

        private void DodajMaterial(Pozycja pozycja)
        {
            using (MaterialDAO bd = new MaterialDAO())
            {
                var pasujace = bd.Pobierz("NazwaM = '" + pozycja.Material.Nazwa +
                                          "' AND Dlugosc = " + pozycja.Material.Dlugosc +
                                          " AND Szerokosc = " + pozycja.Material.Szerokosc);
                if (pasujace.Count > 0)
                {
                    pozycja.Material.ID = pasujace.FirstOrDefault().ID;
                    bd.Edytuj(pozycja.Material);
                }
                else
                {
                    pozycja.Material.ID = bd.Dodaj(pozycja.Material);
                }
            }
        }

        public void DodanoKlienta(object sender, EventArgs e)
        {
            OnPropertyChanged("SzukanyKlient");
        }
    }
}
