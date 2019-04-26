using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lakiernia.Model;

namespace Lakiernia.Data_Access
{
    public class ZamowienieDAO : DAO<Zamowienie>, IDAO<Zamowienie>, IDisposable
    {
        override public long Dodaj(Zamowienie element)
        {
            string sql = "insert into Zamowienia (IdK, IdPr, DataZam, DataOdbioru, CzyZakonczone) values (" + element.Klient.ID +
                         "," + 1 + ",'" + element.DataZlozenia.ToUniversalTime().Ticks +
                         "','" + element.DataOdbioru.ToUniversalTime().Ticks +
                         "'," + 0 + ")";
            return DodajElement(element, sql);
        }

        override public bool Edytuj(Zamowienie element)
        {
            string sql = "update Zamowienia set IdK = " + element.Klient.ID +
                         ", DataZam = '" + element.DataZlozenia.ToUniversalTime().Ticks +
                         "', DataOdbioru = '" + element.DataOdbioru.ToUniversalTime().Ticks +
                         "', CzyZakonczone = " + element.CzyZakonczone +
                         " where IdZam = " + element.ID + ";";
            return EdytujElement(element, sql);
        }

        override public bool Usun(Zamowienie element)
        {
            string sql = "delete from Zamowienia where (IdZam = " + element.ID + ")";
            return EdytujElement(element, sql);
        }

        override public ObservableCollection<Zamowienie> Pobierz(string warunek = "")
        {
            if (warunek != "") warunek = " where " + warunek;
            string sql = "select * from Zamowienia inner join Klienci on Zamowienia.IdK = Klienci.IdK " +
                         "inner join Pracodawcy on Zamowienia.IdPr = Pracodawcy.IdPr" + warunek + " order by IdZam;";
            return PobierzElementy(sql);
        }

        protected override void DodawanieDoListy(SQLiteDataReader reader, ObservableCollection<Zamowienie> elementy)
        {
            string[] adres = reader["AdresK"].ToString().Split(';');
            elementy.Add(new Zamowienie(Int32.Parse(reader["IdZam"].ToString()),
                                        new Klient(long.Parse(reader["IdK"].ToString()), reader["NazwaK"].ToString(), 
                                        reader["TelefonK"].ToString(), reader["EmailK"].ToString(), 
                                        adres[0], adres[1], adres[2], adres[3], reader["NIPK"].ToString(), 
                                        reader["czyOsoba"].ToString().Equals("0") ? TypKlienta.Osoba : TypKlienta.Firma),
                                        Int64.Parse(reader["DataZam"].ToString()), Int64.Parse(reader["DataOdbioru"].ToString()),
                                        Int32.Parse(reader["CzyZakonczone"].ToString())));
        }
    }
}
