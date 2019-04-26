using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lakiernia.Model;

namespace Lakiernia.Data_Access
{
    public class PozycjaDAO : DAO<Pozycja>, IDAO<Pozycja>, IDisposable
    {
        override public long Dodaj(Pozycja element)
        {
            string sql = "insert into Pozycje (IdM, IdZam, IdF, Cena, Rabat, VAT, Liczba) values (" + element.Material.ID + "," + element.IDZamowienia + "," + element.Farba.ID + "," + Regex.Replace(Math.Abs(element.Cena).ToString(), ",", ".") + "," + 
                         element.Rabat + "," + element.VAT + "," + element.Liczba + ")";
            return DodajElement(element, sql);
        }

        override public bool Edytuj(Pozycja element)
        {
            string sql = "update Pozycje set IdM = " + element.Material.ID + ", IdZam = " + element.IDZamowienia +
                         ", IdF = " + element.Farba.ID + ", Cena = " + Regex.Replace(Math.Abs(element.Cena).ToString(), ",", ".") + ", Rabat = " + element.Rabat +
                         ", VAT = " + element.VAT + ", Liczba = " + element.Liczba +
                         " where IdPoz = " + element.ID + ";";
            return EdytujElement(element, sql);
        }

        override public bool Usun(Pozycja element)
        {
            string sql = "delete from Pozycje where (IdPoz = " + element.ID + ")";
            return EdytujElement(element, sql);
        }

        override public ObservableCollection<Pozycja> Pobierz(string warunek = "")
        {
            if (warunek != "") warunek = " where " + warunek;
            string sql = "select * from Pozycje inner join Farby on Pozycje.IdF = Farby.IdF inner join Materialy on " +
                         "Pozycje.IdM = Materialy.IdM" + warunek + " order by IdPoz";
            return PobierzElementy(sql);
        }

        public ObservableCollection<Pozycja> PobierzOkres(DateTime start, DateTime koniec)
        {
            string sql = "select * from Pozycje inner join Farby on Pozycje.IdF = Farby.IdF inner join Materialy on " +
                         "Pozycje.IdM = Materialy.IdM inner join Zamowienia on Pozycje.IdZam = Zamowienia.IdZam where DataOdbioru >= " + start.ToUniversalTime().Ticks + " and DataOdbioru <= " + koniec.ToUniversalTime().Ticks + " order by DataOdbioru";
            return PobierzElementy(sql);
        }

        override protected void DodawanieDoListy(SQLiteDataReader reader, ObservableCollection<Pozycja> elementy)
        {
            elementy.Add(new Pozycja(long.Parse(reader["IdPoz"].ToString()), long.Parse(reader["IdZam"].ToString()),
                          new Material(Int32.Parse(reader["IdM"].ToString()), reader["NazwaM"].ToString(),
                          uint.Parse(reader["Dlugosc"].ToString()), uint.Parse(reader["Szerokosc"].ToString())),
                          new Farba(Int32.Parse(reader["IdF"].ToString()), reader["Kolor"].ToString(), reader["Producent"].ToString(),
                          Double.Parse(reader["Ilosc"].ToString())),
                          Decimal.Parse(reader["Cena"].ToString()), uint.Parse(reader["Rabat"].ToString()), uint.Parse(reader["VAT"].ToString()),
                          uint.Parse(reader["Liczba"].ToString())));
        }
    }
}