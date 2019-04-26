using Lakiernia.Model;
using Lakiernia.Utils;
using Lakiernia.Data_Access;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Lakiernia.View_Model
{
    class PodsumowanieKlientowVM : ObiektEdytowalny
    {
        private SeriesCollection _kawalki;
        private IList<PodsumowanieKlienta> _lista;
        private DateTime _start;
        private DateTime _koniec;
        private DateTime _nowyStart;
        private DateTime _nowyKoniec;
        private ICommand _zastosujKmd;

        public SeriesCollection Kawalki
        {
            get => _kawalki;
            set
            {
                _kawalki = value;
                OnPropertyChanged("Kawalki");
            }
        }
        public IList<PodsumowanieKlienta> Lista
        {
            get => _lista;
            set
            {
                _lista = value;
                OnPropertyChanged("Lista");
            }
        }
        public DateTime Start
        {
            get => _start;
            set
            {
                _start = value;
                OnPropertyChanged("Start");
            }
        }
        public DateTime Koniec
        {
            get => _koniec;
            set
            {
                _koniec = value;
                OnPropertyChanged("Koniec");
            }
        }
        public DateTime NowyStart
        {
            get => _nowyStart;
            set
            {
                _nowyStart = value;
                OnPropertyChanged("NowyStart");
            }
        }
        public DateTime NowyKoniec
        {
            get => _nowyKoniec;
            set
            {
                _nowyKoniec = value;
                OnPropertyChanged("NowyKoniec");
            }
        }
        public ICommand ZastosujKmd
        {
            get
            {
                if (_zastosujKmd == null) _zastosujKmd = new Komenda(Zastosuj, CzyPoprawne);
                return _zastosujKmd;
            }
        }

        public PodsumowanieKlientowVM()
        {
            Koniec = DateTime.Now.Date;
            Start = Koniec.AddDays(-30);
            NowyKoniec = Koniec;
            NowyStart = Start;
            GenerujListe();
            GenerujDiagram();
        }

        private void Zastosuj(object obj)
        {
            Start = NowyStart;
            Koniec = NowyKoniec;
            GenerujListe();
            GenerujDiagram();
        }

        private bool CzyPoprawne(object obj)
        {
            return NowyKoniec != null && NowyStart != null;
        }

        private void GenerujListe()
        {
            ObservableCollection<Zamowienie> zamowienia;
            using (ZamowienieDAO bd = new ZamowienieDAO()) zamowienia = bd.Pobierz("DataOdbioru >= " + Start.ToUniversalTime().Ticks + " and DataOdbioru <= " + Koniec.AddMinutes(1439).ToUniversalTime().Ticks);
            Lista = new List<PodsumowanieKlienta>(zamowienia.GroupBy(z => z.Klient.Nazwa, z => z.Pozycje.Sum(p => ObliczPrzychod(p)), (klient, przychody) => new PodsumowanieKlienta { Klient = klient, IloscZamowien = przychody.Count(), SumaZamowien = przychody.Sum()})).OrderByDescending(z => z.SumaZamowien).ToList();
        }

        private decimal ObliczPrzychod(Pozycja p)
        {
            decimal koszt = p.Cena * p.Material.Dlugosc * p.Material.Szerokosc * p.Liczba / 1000000M;
            decimal przychod = koszt - (decimal)p.Rabat/100m * koszt;
            return Math.Round(przychod,2);
        }

        private void GenerujDiagram()
        {
            decimal suma = Lista.Sum(pk => pk.SumaZamowien);
            decimal pozostale = 0;
            Kawalki = new SeriesCollection();
            foreach (PodsumowanieKlienta pk in Lista)
            {
                if (pk.SumaZamowien / suma > 0.025M)
                {
                    Kawalki.Add(new PieSeries
                    {
                        Title = pk.Klient,
                        Values = new ChartValues<ObservableValue> { new ObservableValue((double)pk.SumaZamowien) },
                        DataLabels = true
                    });
                }
                else pozostale += pk.SumaZamowien;
            }
            if (pozostale > 0)
            {
                Kawalki.Add(new PieSeries
                {
                    Title = "Pozostałe",
                    Values = new ChartValues<ObservableValue> { new ObservableValue((double)pozostale) },
                    DataLabels = true
                });
            }
        }
    }

    public struct PodsumowanieKlienta
    {
        public string Klient { get; set; }
        public int IloscZamowien { get; set; }
        public decimal SumaZamowien { get; set; }
    }
}