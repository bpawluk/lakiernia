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
    class PodsumowanieFarbVM : ObiektEdytowalny
    {
        private SeriesCollection _kawalki;
        private IList<ZuzycieFarb> _lista;
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
        public IList<ZuzycieFarb> Lista
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

        public PodsumowanieFarbVM()
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
            ObservableCollection<Pozycja> pozycje;
            using (PozycjaDAO bd = new PozycjaDAO()) pozycje = bd.PobierzOkres(Start, Koniec.AddMinutes(1439));
            Lista = new List<ZuzycieFarb>(pozycje.GroupBy(p => p.Farba.Kolor + " " + p.Farba.Producent, p => Math.Round(p.Material.Dlugosc * p.Material.Szerokosc * p.Liczba / 1000000D,1), (farba, powierzchnie) => new ZuzycieFarb { Farba = farba, Powierzchnia = powierzchnie.Sum() })).OrderByDescending(z => z.Powierzchnia).ToList();
        }

        private void GenerujDiagram()
        {
            double suma = Lista.Sum(zf => zf.Powierzchnia);
            double pozostale = 0;
            Kawalki = new SeriesCollection();
            foreach (ZuzycieFarb zf in Lista)
            {
                if (zf.Powierzchnia / suma > 0.025)
                {
                    Kawalki.Add(new PieSeries
                    {
                        Title = zf.Farba,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(zf.Powierzchnia) },
                        DataLabels = true
                    });
                }
                else pozostale += zf.Powierzchnia;
            }
            if (pozostale > 0)
            {
                Kawalki.Add(new PieSeries
                {
                    Title = "Pozostałe",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(pozostale) },
                    DataLabels = true
                });
            }
        }
    }

    public struct ZuzycieFarb
    {
        public string Farba { get; set; }
        public double Powierzchnia { get; set; }
    }
}
