using Lakiernia.Data_Access;
using Lakiernia.Model;
using Lakiernia.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Lakiernia.View_Model
{
    public class StronaPowitalnaVM : ObiektEdytowalny
    {
        public event EventHandler<ObiektEdytowalnyEventArgs> OtworzDaneZamowienia;

        private ObservableCollection<Zamowienie> _zamowienia;
        private ObservableCollection<Farba> _farby;
        private DateTime _dataWyjsciowa = DateTime.Now;
        private ICommand _otworzKmd;
        private ICommand _doTyluKmd;
        private ICommand _doPrzoduKmd;
        private ICommand _zapiszFarbyKmd;

        public DateTime Pierwsza { get => _dataWyjsciowa; }
        public DateTime Druga { get => _dataWyjsciowa.AddDays(1); }
        public DateTime Trzecia { get => _dataWyjsciowa.AddDays(2); }
        public DateTime Czwarta { get => _dataWyjsciowa.AddDays(3); }
        public DateTime Piąta { get => _dataWyjsciowa.AddDays(4); }

        public ObservableCollection<Zamowienie> Pierwsze
        {
            get => new ObservableCollection<Zamowienie>(_zamowienia.Where(zam => zam.DataOdbioru.Date == Pierwsza.Date));
        }
        public ObservableCollection<Zamowienie> Drugie
        {
            get => new ObservableCollection<Zamowienie>(_zamowienia.Where(zam => zam.DataOdbioru.Date == Druga.Date));
        }
        public ObservableCollection<Zamowienie> Trzecie
        {
            get => new ObservableCollection<Zamowienie>(_zamowienia.Where(zam => zam.DataOdbioru.Date == Trzecia.Date));
        }
        public ObservableCollection<Zamowienie> Czwarte
        {
            get => new ObservableCollection<Zamowienie>(_zamowienia.Where(zam => zam.DataOdbioru.Date == Czwarta.Date));
        }
        public ObservableCollection<Zamowienie> Piąte
        {
            get => new ObservableCollection<Zamowienie>(_zamowienia.Where(zam => zam.DataOdbioru.Date == Piąta.Date));
        }

        public ObservableCollection<Farba> Farby { get => _farby; set {_farby = value; OnPropertyChanged("Farby");} }

        public ICommand OtworzKmd
        {
            get
            {
                if (_otworzKmd == null) _otworzKmd = new Komenda(Otworz);
                return _otworzKmd;
            }
        }

        public ICommand DoTyluKmd
        {
            get
            {
                if (_doTyluKmd == null) _doTyluKmd = new Komenda(DoTylu);
                return _doTyluKmd;
            }
        }

        public ICommand DoPrzoduKmd
        {
            get
            {
                if (_doPrzoduKmd == null) _doPrzoduKmd = new Komenda(DoPrzodu);
                return _doPrzoduKmd;
            }
        }

        public ICommand ZapiszFarbyKmd
        {
            get
            {
                if (_zapiszFarbyKmd == null) _zapiszFarbyKmd = new Komenda(ZapiszFarby);
                return _zapiszFarbyKmd;
            }
        }

        public StronaPowitalnaVM()
        {
            using (ZamowienieDAO bd = new ZamowienieDAO()) _zamowienia = bd.Pobierz("CzyZakonczone = 0");
            using (FarbaDAO bd = new FarbaDAO()) _farby = new ObservableCollection<Farba>(bd.Pobierz().OrderBy(f => f.Ilosc));
            foreach(Farba f in _farby) f.PropertyChanged += ZmienionoIlosc;
        }

        private void Otworz(object parametr)
        {
            OtworzDaneZamowienia?.Invoke(this, new ObiektEdytowalnyEventArgs(parametr as Zamowienie ?? new Zamowienie()));
        }

        private void DoTylu(object parametr)
        {
            _dataWyjsciowa = _dataWyjsciowa.AddDays(-1);
            OnPropertyChanged("Pierwsza", "Druga", "Trzecia", "Czwarta", "Piąta", "Pierwsze", "Drugie", "Trzecie", "Czwarte", "Piąte");
        }

        private void DoPrzodu(object parametr)
        {
            _dataWyjsciowa = _dataWyjsciowa.AddDays(1);
            OnPropertyChanged("Pierwsza", "Druga", "Trzecia", "Czwarta", "Piąta", "Pierwsze", "Drugie", "Trzecie", "Czwarte", "Piąte");
        }

        private void ZapiszFarby(object parametr)
        {
            foreach (Farba f in _farby) using (FarbaDAO bd = new FarbaDAO()) bd.Edytuj(f);
        }

        private void ZmienionoIlosc(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Ilosc")) Farby = new ObservableCollection<Farba>(_farby.OrderBy(f => f.Ilosc));
        }
    }
}
