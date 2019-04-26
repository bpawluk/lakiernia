using Lakiernia.Utils;
using Lakiernia.Model;
using Lakiernia.Data_Access;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using System.Windows;

namespace Lakiernia.View_Model
{
    public class ZamowieniaVM : ObiektEdytowalny
    {
        public event EventHandler<ObiektEdytowalnyEventArgs> OtworzDaneZamowienia;

        private ObservableCollection<Zamowienie> _zamowienia;
        private Zamowienie _wybraneZamowienie;
        private bool _czyArchiwum = false;
        private ICommand _noweZamowienieKmd;
        private ICommand _fakturaKmd;
        private ICommand _edytujZamowienieKmd;
        private ICommand _usunZamowienieKmd;
        private ICommand _archiwizujKmd;
        private ICommand _otworzArchiwumKmd;
        private ICommand _otworzAktualneKmd;

        public ObservableCollection<Zamowienie> Zamowienia
        {
            get
            {
                return _zamowienia;
            }
            private set
            {
                _zamowienia = value;
                OnPropertyChanged("Zamowienia", "WybraneZamowienie");
            }
        }

        public Zamowienie WybraneZamowienie
        {
            get
            {
                return _wybraneZamowienie;
            }
            set
            {
                _wybraneZamowienie = value;
                OnPropertyChanged("WybraneZamowienie");
            }
        }

        public bool CzyArchiwum { get => _czyArchiwum; set { _czyArchiwum = value; OnPropertyChanged("CzyArchiwum"); } }

        public ICommand NoweZamowienieKmd
        {
            get
            {
                if (_noweZamowienieKmd == null) _noweZamowienieKmd = new Komenda(NoweZamowienie);
                return _noweZamowienieKmd;
            }
        }

        public ICommand FakturaKmd
        {
            get
            {
                if (_fakturaKmd == null) _fakturaKmd = new Komenda(Faktura, CzyWybrano);
                return _fakturaKmd;
            }
        }

        public ICommand EdytujZamowienieKmd
        {
            get
            {
                if (_edytujZamowienieKmd == null) _edytujZamowienieKmd = new Komenda(EdytujZamowienie, CzyWybrano);
                return _edytujZamowienieKmd;
            }
        }

        public ICommand UsunZamowienieKmd
        {
            get
            {
                if (_usunZamowienieKmd == null) _usunZamowienieKmd = new Komenda(UsunZamowienie, CzyWybrano);
                return _usunZamowienieKmd;
            }
        }

        public ICommand ArchiwizujKmd
        {
            get
            {
                if (_archiwizujKmd == null) _archiwizujKmd = new Komenda(Archiwizuj, CzyWybrano);
                return _archiwizujKmd;
            }
        }

        public ICommand OtworzArchiwumKmd
        {
            get
            {
                if (_otworzArchiwumKmd == null) _otworzArchiwumKmd = new Komenda(OtworzArchiwum);
                return _otworzArchiwumKmd;
            }
        }

        public ICommand OtworzAktualneKmd
        {
            get
            {
                if (_otworzAktualneKmd == null) _otworzAktualneKmd = new Komenda(OtworzAktualne);
                return _otworzAktualneKmd;
            }
        }

        public ZamowieniaVM()
        {
            using (ZamowienieDAO bd = new ZamowienieDAO()) Zamowienia = bd.Pobierz("CzyZakonczone = 0");
            _wybraneZamowienie = null;
        }

        private bool CzyWybrano(object parametr)
        {
            return WybraneZamowienie != null && WybraneZamowienie.ID > 0;
        }

        private void NoweZamowienie(object parametr)
        {
            OtworzDaneZamowienia?.Invoke(this, new ObiektEdytowalnyEventArgs(new Zamowienie()));
        }

        private void Faktura(object parametr)
        {
            GeneratorFaktur.generuj(_wybraneZamowienie);
        }

        private void EdytujZamowienie(object parametr)
        {
            if (_wybraneZamowienie == null) MessageBox.Show("Wybierz zamówienie do edycji");
            else OtworzDaneZamowienia?.Invoke(this, new ObiektEdytowalnyEventArgs(_wybraneZamowienie ?? new Zamowienie()));
        }

        private void Archiwizuj(object parametr)
        {
            if (_wybraneZamowienie == null) MessageBox.Show("Wybierz zamówienie do archiwizacji");
            else
            {
                WybraneZamowienie.CzyZakonczone = (int)parametr;
                using (ZamowienieDAO bd = new ZamowienieDAO()) bd.Edytuj(WybraneZamowienie);
                Zamowienia.Remove(WybraneZamowienie);
                WybraneZamowienie = null;
            }
        }

        private void OtworzArchiwum(object parametr)
        {
            CzyArchiwum = true;
            using (ZamowienieDAO bd = new ZamowienieDAO()) Zamowienia = bd.Pobierz("CzyZakonczone = 1");
        }

        private void OtworzAktualne(object parametr)
        {
            CzyArchiwum = false;
            using (ZamowienieDAO bd = new ZamowienieDAO()) Zamowienia = bd.Pobierz("CzyZakonczone = 0");
        }

        private void UsunZamowienie(object parametr)
        {
            if (_wybraneZamowienie == null) MessageBox.Show("Wybierz zamówienie do usunięcia");
            else
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć wybrany element?", "USUWANIE", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using (PozycjaDAO bd = new PozycjaDAO()) foreach (Pozycja pozycja in _wybraneZamowienie.Pozycje) bd.Usun(pozycja);
                    using (ZamowienieDAO bd = new ZamowienieDAO())
                    {
                        if (bd.Usun(_wybraneZamowienie)) Zamowienia.Remove(_wybraneZamowienie);
                        else MessageBox.Show("Element, który starasz się usunąć, jest powiązany z innymi elementami." +
                                             "\nNajpierw usuń wszystkie powiązane elementy.", "BŁĄD!");
                    }
                }
            }
        }
    }
}