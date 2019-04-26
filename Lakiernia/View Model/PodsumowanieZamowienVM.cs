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
using System.Windows.Controls;

namespace Lakiernia.View_Model
{
    class PodsumowanieZamowienVM : ObiektEdytowalny
    {
        private DateTime _start;
        private DateTime _koniec;
        private DateTime _nowyStart;
        private DateTime _nowyKoniec;
        private ICommand _zastosujKmd;
        private SeriesCollection _seriesCollection;
        private string[] _labels;

        public ComboBoxItem Czestotliwosc { get; set; }
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
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged("SeriesCollection");
            }
        }
        public string[] Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged("Labels");
            }
        }

        public PodsumowanieZamowienVM()
        {
            Koniec = DateTime.Now.Date;
            Start = Koniec.AddDays(-30);
            NowyKoniec = Koniec;
            NowyStart = Start;
            GenerujDiagram();
        }

        private void Zastosuj(object obj)
        {
            Start = NowyStart;
            Koniec = NowyKoniec;
            GenerujDiagram();
        }

        private bool CzyPoprawne(object obj)
        {
            return NowyKoniec != null && NowyStart != null;
        }

        private void GenerujDiagram()
        {
            Labels = new string[0];
            SeriesCollection = new SeriesCollection();

            var dni = new List<DzienPrzychodow>();
            if (Koniec >= Start)
            {
                DateTime obecna = Start.Date;
                ObservableCollection<Zamowienie> zamowienia;
                while (obecna.Date <= Koniec.Date)
                {
                    using (ZamowienieDAO bd = new ZamowienieDAO())
                        zamowienia = bd.Pobierz("DataOdbioru >= " +
                                                obecna.ToUniversalTime().Ticks +
                                                " and DataOdbioru <= " +
                                                obecna.ToUniversalTime().AddMinutes(1439).Ticks);
                    dni.Add(new DzienPrzychodow { dzien = obecna, przychod = zamowienia.Sum(z => z.Pozycje.Sum(p => ObliczPrzychod(p))) });
                    obecna = obecna.AddDays(1);
                }
            }

            Func<DzienPrzychodow, string> grupowanie = d => d.dzien.ToShortDateString();

            switch (Czestotliwosc?.Content)
            {
                case "Miesiąc":
                    grupowanie = d => $"{d.dzien:MM yyyy}";
                    break;
                case "Rok":
                    grupowanie = d => d.dzien.Year.ToString();
                    break;
                default:
                    break;
            }

            var lista = dni.GroupBy(grupowanie, d => d.przychod, (okres, przychod) => new { Label = okres, Value = przychod.Sum() }); 

            var values = new ChartValues<decimal>();
            var labels = new List<string>();

            foreach (var element in lista)
            {
                values.Add(element.Value);
                labels.Add(element.Label);
            }

            Labels = labels.ToArray();
            SeriesCollection.Add(new LineSeries
            {
                Title = "Przychody",
                Values = values,
                LineSmoothness = 0
            });
        }

        private decimal ObliczPrzychod(Pozycja p)
        {
            decimal koszt = p.Cena * p.Material.Dlugosc * p.Material.Szerokosc * p.Liczba / 1000000M;
            decimal przychod = koszt - (decimal)p.Rabat / 100m * koszt;
            return Math.Round(przychod, 2);
        }
    }
    struct DzienPrzychodow
    {
        public DateTime dzien { get; set; }
        public decimal przychod { get; set; }
    }
}