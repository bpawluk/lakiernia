using Lakiernia.Utils;
using Lakiernia.Model;
using Lakiernia.Data_Access;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Linq;

namespace Lakiernia.View_Model
{
    public class KlienciVM : ObiektEdytowalny
    {
        public event EventHandler<ObiektEdytowalnyEventArgs> OtworzDaneKlienta;

        private ObservableCollection<Klient> _klienci;
        private Klient _wybranyKlient;
        private readonly string _tekstZachecajacy = "Wyszukaj klienta po nazwie...";
        private string _szukanaNazwa;
        private ICommand _resetujKmd;
        private ICommand _nowyKlientKmd;
        private ICommand _edytujKlientaKmd;
        private ICommand _usunKlientaKmd;

        public ObservableCollection<Klient> Klienci
        {
            get
            {
                return _klienci;
            }
            private set
            {
                _klienci = value;
                OnPropertyChanged("Klienci", "WybranyKlient");
            }
        }

        public Klient WybranyKlient
        {
            get
            {
                return _wybranyKlient;
            }
            set
            {
                _wybranyKlient = value;
                OnPropertyChanged("WybranyKlient");
            }
        }

        public string TekstZachecajacy { get => _tekstZachecajacy; }

        public string SzukanaNazwa
        {
            get
            {
                return _szukanaNazwa;
            }
            set
            {
                _szukanaNazwa = value;
                OnPropertyChanged("SzukanaNazwa");
            }
        }

        public ICommand ResetujKmd
        {
            get
            {
                if (_resetujKmd == null) _resetujKmd = new Komenda(Resetuj,CzySzukanie);
                return _resetujKmd;
            }
        }

        public ICommand NowyKlientKmd
        {
            get
            {
                if (_nowyKlientKmd == null) _nowyKlientKmd = new Komenda(NowyKlient);
                return _nowyKlientKmd;
            }
        }

        public ICommand EdytujKlientaKmd
        {
            get
            {
                if (_edytujKlientaKmd == null) _edytujKlientaKmd = new Komenda(EdytujKlienta,CzyWybrano);
                return _edytujKlientaKmd;
            }
        }

        public ICommand UsunKlientaKmd
        {
            get
            {
                if (_usunKlientaKmd == null) _usunKlientaKmd = new Komenda(UsunKlienta, CzyWybrano);
                return _usunKlientaKmd;
            }
        }

        public KlienciVM()
        {
            using (KlientDAO bd = new KlientDAO()) Klienci = bd.Pobierz();
            _wybranyKlient = null;
            SzukanaNazwa = _tekstZachecajacy;
            PropertyChanged += Odswiez;
        }

        private bool CzyWybrano(object parametr)
        {
            return WybranyKlient != null && WybranyKlient.ID > 0;
        }

        private bool CzySzukanie(object parametr)
        {
            return SzukanaNazwa != "" && SzukanaNazwa != TekstZachecajacy;
        }

        private void Resetuj(object parametr)
        {
            SzukanaNazwa = "";
            SzukanaNazwa = _tekstZachecajacy;
        }

        private void NowyKlient(object parametr)
        {
            OtworzDaneKlienta?.Invoke(this, new ObiektEdytowalnyEventArgs(new Klient()));
        }

        private void EdytujKlienta(object parametr)
        {
            if (_wybranyKlient == null) MessageBox.Show("Wybierz klienta do usunięcia");
            else OtworzDaneKlienta?.Invoke(this, new ObiektEdytowalnyEventArgs(_wybranyKlient ?? new Klient()));
        }

        private void UsunKlienta(object parametr)
        {
            if (_wybranyKlient == null) MessageBox.Show("Wybierz klienta do usunięcia");
            else
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć wybrany element?", "USUWANIE", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using (KlientDAO bd = new KlientDAO())
                    {
                        if (bd.Usun(_wybranyKlient)) Klienci.Remove(_wybranyKlient);
                        else MessageBox.Show("Element, który starasz się usunąć, jest powiązany z innymi elementami." +
                                             "\nNajpierw usuń wszystkie powiązane elementy.", "BŁĄD!");
                    }
                }
            }
        }

        private void Odswiez(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SzukanaNazwa"))
            {
                ObservableCollection<Klient> sfiltrowani;
                using (KlientDAO bd = new KlientDAO())
                {
                    if (SzukanaNazwa.Equals("")) sfiltrowani = bd.Pobierz();
                    else if (SzukanaNazwa.Equals(_tekstZachecajacy)) sfiltrowani = null;
                    else sfiltrowani = bd.Pobierz("NazwaK like '%" + SzukanaNazwa + "%'");

                    if (sfiltrowani != null)
                    {
                        Klient wybrany = WybranyKlient;
                        Klienci.Clear();
                        foreach (Klient klient in sfiltrowani) Klienci.Add(klient);
                        if (Klienci.Contains(wybrany)) WybranyKlient = Klienci.ElementAt(Klienci.IndexOf(wybrany));
                    }
                }
            }
        }
    }
}