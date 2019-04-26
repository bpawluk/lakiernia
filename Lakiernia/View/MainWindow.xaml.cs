using Lakiernia.Model;
using Lakiernia.Utils;
using Lakiernia.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lakiernia.View
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StronaPowitalna spo = new StronaPowitalna();
            StronaPowitalnaVM spvm = new StronaPowitalnaVM();
            spvm.OtworzDaneZamowienia += OtworzDaneZamowienia;
            spo.DataContext = spvm;
            Zawartosc.Navigate(spo);
        }

        public void OtworzGlowna(object sender, RoutedEventArgs e)
        {
            StronaPowitalna spo = new StronaPowitalna();
            StronaPowitalnaVM spvm = new StronaPowitalnaVM();
            spvm.OtworzDaneZamowienia += OtworzDaneZamowienia;
            spo.DataContext = spvm;
            Zawartosc.Navigate(spo);
        }

        public void OtworzZamowienia(object sender, RoutedEventArgs e)
        {
            Zamowienia zam = new Zamowienia();
            ZamowieniaVM zvm = new ZamowieniaVM();
            zam.DataContext = zvm;
            zvm.OtworzDaneZamowienia += OtworzDaneZamowienia;
            Zawartosc.Navigate(zam);
        }

        public void OtworzFarby(object sender, RoutedEventArgs e)
        {
            Farby far = new Farby();
            FarbyVM fvm = new FarbyVM();
            far.DataContext = fvm;
            Zawartosc.Navigate(far);
        }

        public void OtworzKlienci(object sender, RoutedEventArgs e)
        {
            Klienci kli = new Klienci();
            KlienciVM kvm = new KlienciVM();
            kli.DataContext = kvm;
            kvm.OtworzDaneKlienta += OtworzDaneKlienta;
            Zawartosc.Navigate(kli);
        }

        private void OtworzMaterialy(object sender, RoutedEventArgs e)
        {
            Materialy mat = new Materialy();
            MaterialyVM mvm = new MaterialyVM();
            mat.DataContext = mvm;
            Zawartosc.Navigate(mat);
        }

        public void OtworzPodsumowanie(object sender, RoutedEventArgs e)
        {
            Podsumowanie pod = new Podsumowanie();
            PodsumowanieVM pvm = new PodsumowanieVM();
            pod.DataContext = pvm;
            Zawartosc.Navigate(pod);
        }

        public void OtworzPracodawcy(object sender, RoutedEventArgs e)
        {
            DanePracodawcy pra = new DanePracodawcy();
            DanePracodawcyVM pvm = new DanePracodawcyVM();
            pra.DataContext = pvm;
            Zawartosc.Navigate(pra);
        }

        public void OtworzDaneKlienta(object sender, ObiektEdytowalnyEventArgs e)
        {
            DaneKlienta dkl = new DaneKlienta();
            DaneKlientaVM dkvm = new DaneKlientaVM(e.Obiekt as Klient);
            dkl.DataContext = dkvm;
            dkvm.Powrot += OtworzKlienci;
            Zawartosc.Navigate(dkl);
        }

        public void OtworzDaneZamowienia(object sender, ObiektEdytowalnyEventArgs e)
        {
            DaneZamowienia dz = new DaneZamowienia();
            DaneZamowieniaVM dzvm = new DaneZamowieniaVM(e.Obiekt as Zamowienie);
            dz.DataContext = dzvm;
            dzvm.Powrot += OtworzZamowienia;
            Zawartosc.Navigate(dz);
        }
    }
}
