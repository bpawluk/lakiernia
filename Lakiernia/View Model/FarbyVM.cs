using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using Lakiernia.Data_Access;
using Lakiernia.Model;
using Lakiernia.Utils;

namespace Lakiernia.View_Model
{
    public class FarbyVM : ObiektEdytowalny
    {
        private ObservableCollection<Farba> _farby;
        private Farba _wybranaFarba;
        private Farba _edytowanaFarba;
        private readonly string _tekstZachecajacy = "Wyszukaj farbę po kolorze...";
        private string _szukanyKolor;
        private ICommand _zapiszKmd;
        private ICommand _usunKmd;
        private ICommand _nowaFarbaKmd;
        private ICommand _resetujKmd;

        public ObservableCollection<Farba> Farby
        {
            get
            {
                return _farby;
            }
            private set
            {
                _farby = value;
                OnPropertyChanged("Farby", "WybranaFarba");
            }
        }

        public Farba WybranaFarba
        {
            get
            {
                return _wybranaFarba;
            }
            set
            {
                _wybranaFarba = value;
                OnPropertyChanged("WybranaFarba");
            }
        }

        public Farba EdytowanaFarba
        {
            get
            {
                return _edytowanaFarba;
            }
            set
            {
                _edytowanaFarba = value;
                OnPropertyChanged("EdytowanaFarba");
            }
        }

        public string TekstZachecajacy { get => _tekstZachecajacy; }

        public string SzukanyKolor
        {
            get
            {
                return _szukanyKolor;
            }
            set
            {
                _szukanyKolor = value;
                OnPropertyChanged("SzukanyKolor");
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

        public ICommand UsunKmd
        {
            get
            {
                if (_usunKmd == null) _usunKmd = new Komenda(Usun, CzyWybrano);
                return _usunKmd;
            }
        }

        public ICommand NowaFarbaKmd
        {
            get
            {
                if (_nowaFarbaKmd == null) _nowaFarbaKmd = new Komenda(NowaFarba);
                return _nowaFarbaKmd;
            }
        }

        public ICommand ResetujKmd
        {
            get
            {
                if (_resetujKmd == null) _resetujKmd = new Komenda(Resetuj, CzySzukanie);
                return _resetujKmd;
            }
        }

        public FarbyVM()
        {
            using (FarbaDAO bd = new FarbaDAO()) Farby = bd.Pobierz();
            _wybranaFarba = null;
            _edytowanaFarba = new Farba();
            SzukanyKolor = _tekstZachecajacy;
            PropertyChanged += WybranoFarbe;
            PropertyChanged += Odswiez;
        }

        private bool CzyWybrano(object parametr)
        {
            return WybranaFarba != null && WybranaFarba.ID > 0;
        }

        private bool CzySzukanie(object parametr)
        {
            return SzukanyKolor != "" && SzukanyKolor != TekstZachecajacy;
        }

        private void Zapisz(object parametr)
        {
            if (_edytowanaFarba.ID == -1)
            {
                Farby.Add(_edytowanaFarba);
                using (FarbaDAO bd = new FarbaDAO()) _edytowanaFarba.ID = bd.Dodaj(_edytowanaFarba);
                WybranaFarba = null;
            }
            else
            {
                WybranaFarba.Kopiuj(_edytowanaFarba);
                using (FarbaDAO bd = new FarbaDAO()) bd.Edytuj(_edytowanaFarba);
            }
        }

        private void Usun(object parametr)
        {
            if (WybranaFarba == null)
            {
                MessageBox.Show("Wybierz farbę do usunięcia");
                return;
            }
            else
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć wybrany element?", "USUWANIE", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using (FarbaDAO bd = new FarbaDAO())
                    {
                        if (bd.Usun(WybranaFarba)) Farby.Remove(WybranaFarba);
                        else MessageBox.Show("Element, który starasz się usunąć, jest powiązany z innymi elementami." +
                                             "\nNajpierw usuń wszystkie powiązane elementy.", "BŁĄD!");
                    }
                }
            }
            WybranaFarba = null;
        }

        private void Resetuj(object parametr)
        {
            SzukanyKolor = "";
            SzukanyKolor = _tekstZachecajacy;
        }

        private void NowaFarba(object parametr)
        {
            WybranaFarba = null;
        }

        private void Odswiez(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SzukanyKolor"))
            {
                ObservableCollection<Farba> sfiltrowane;
                using (FarbaDAO bd = new FarbaDAO())
                {
                    if (SzukanyKolor.Equals("")) sfiltrowane = bd.Pobierz();
                    else if (SzukanyKolor.Equals(_tekstZachecajacy)) sfiltrowane = null;
                    else sfiltrowane = bd.Pobierz("Kolor like '%" + SzukanyKolor + "%'");

                    if (sfiltrowane != null)
                    {
                        long wybranaID = WybranaFarba?.ID ?? -1;
                        Farby.Clear();
                        foreach (Farba farba in sfiltrowane) Farby.Add(farba);
                        WybranaFarba = Farby.Where( f=> f.ID == wybranaID).FirstOrDefault();
                    }
                }
            }
        }

        private void WybranoFarbe(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("WybranaFarba"))
            {
                if (WybranaFarba == null) EdytowanaFarba = new Farba();
                else EdytowanaFarba = new Farba(WybranaFarba);
            }
        }
    }
}