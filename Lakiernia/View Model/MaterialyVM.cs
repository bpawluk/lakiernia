using Lakiernia.Data_Access;
using Lakiernia.Model;
using Lakiernia.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Lakiernia.View_Model
{
    public class MaterialyVM : ObiektEdytowalny
    {
        private ObservableCollection<Material> _materialy;
        private Material _wybranyMaterial;
        private Material _edytowanyMaterial;
        private readonly string _tekstZachecajacy = "Wyszukaj materiał po nazwie...";
        private string _szukanaNazwa;
        private ICommand _resetujKmd;
        private ICommand _zapiszKmd;
        private ICommand _usunKmd;
        private ICommand _nowyMaterialKmd;

        public ObservableCollection<Material> Materialy
        {
            get
            {
                return _materialy;
            }
            private set
            {
                _materialy = value;
                OnPropertyChanged("Materialy", "WybranyMaterial", "WybranaNazwa", "WybranaDlugosc", "WybranaSzerokosc");
            }
        }

        public Material WybranyMaterial
        {
            get
            {
                return _wybranyMaterial;
            }
            set
            {
                _wybranyMaterial = value;
                OnPropertyChanged("WybranyMaterial");
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

        public string WybranaNazwa
        {
            get
            {
                return _edytowanyMaterial.Nazwa;
            }
            set
            {
                _edytowanyMaterial.Nazwa = value;
                OnPropertyChanged("WybranaNazwa");
            }
        }

        public uint WybranaDlugosc
        {
            get
            {
                return _edytowanyMaterial.Dlugosc;
            }
            set
            {
                _edytowanyMaterial.Dlugosc = value;
                OnPropertyChanged("WybranaDlugosc");
            }
        }

        public uint WybranaSzerokosc
        {
            get
            {
                return _edytowanyMaterial.Szerokosc;
            }
            set
            {
                _edytowanyMaterial.Szerokosc = value;
                OnPropertyChanged("WybranaSzerokosc");
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

        public ICommand NowyMaterialKmd
        {
            get
            {
                if (_nowyMaterialKmd == null) _nowyMaterialKmd = new Komenda(NowyMaterial);
                return _nowyMaterialKmd;
            }
        }

        public MaterialyVM()
        {
            using (MaterialDAO bd = new MaterialDAO()) Materialy = bd.Pobierz();
            _wybranyMaterial = null;
            _edytowanyMaterial = new Material();
            PropertyChanged += WybranoMaterial;
            SzukanaNazwa = _tekstZachecajacy;
            PropertyChanged += Odswiez;
        }

        private bool CzyWybrano(object parametr)
        {
            return WybranyMaterial != null && WybranyMaterial.ID > 0;
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

        private void Zapisz(object parametr)
        {
            if (_edytowanyMaterial.ID == -1)
            {
                Materialy.Add(_edytowanyMaterial);
                using (MaterialDAO bd = new MaterialDAO()) _edytowanyMaterial.ID = bd.Dodaj(_edytowanyMaterial);
                WybranyMaterial = null;
                OnPropertyChanged("WybranyMaterial");
            }
            else
            {
                WybranyMaterial.kopiuj(_edytowanyMaterial);
                using (MaterialDAO bd = new MaterialDAO()) bd.Edytuj(_edytowanyMaterial);
            }
        }

        private void Usun(object parametr)
        {
            if (WybranyMaterial == null)
            {
                MessageBox.Show("Wybierz materiał do usunięcia");
                return;
            }
            else
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć wybrany element?", "USUWANIE", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using (MaterialDAO bd = new MaterialDAO())
                    {
                        if (bd.Usun(WybranyMaterial)) Materialy.Remove(WybranyMaterial);
                        else MessageBox.Show("Element, który starasz się usunąć, jest powiązany z innymi elementami." +
                                             "\nNajpierw usuń wszystkie powiązane elementy.", "BŁĄD!");
                    }
                }
            }
            WybranyMaterial = null;
            OnPropertyChanged("WybranyMaterial");
        }

        private void NowyMaterial(object parametr)
        {
            WybranyMaterial = null;
            OnPropertyChanged("WybranyMaterial");
        }

        private void WybranoMaterial(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("WybranyMaterial"))
            {
                if (WybranyMaterial == null) _edytowanyMaterial = new Material();
                else _edytowanyMaterial = new Material(WybranyMaterial);
                OnPropertyChanged("WybranaNazwa", "WybranaDlugosc", "WybranaSzerokosc");
            }
        }

        private void Odswiez(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SzukanaNazwa"))
            {
                ObservableCollection<Material> sfiltrowane;
                using (MaterialDAO bd = new MaterialDAO())
                {
                    if (SzukanaNazwa.Equals("")) sfiltrowane = bd.Pobierz();
                    else if (SzukanaNazwa.Equals(_tekstZachecajacy)) sfiltrowane = null;
                    else sfiltrowane = bd.Pobierz("NazwaM like '%" + SzukanaNazwa + "%'");

                    if (sfiltrowane != null)
                    {
                        long wybranyID = WybranyMaterial?.ID ?? -1;
                        Materialy.Clear();
                        foreach (Material material in sfiltrowane) Materialy.Add(material);
                        WybranyMaterial = Materialy.Where(m => m.ID == wybranyID).FirstOrDefault();
                    }
                }
            }
        }
    }
}
