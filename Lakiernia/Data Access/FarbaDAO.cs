using Lakiernia.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Lakiernia.Data_Access
{
    public class FarbaDAO : DAO<Farba>, IDAO<Farba>, IDisposable
    {
        override public long Dodaj(Farba element)
        {
            string sql = "insert into Farby (Kolor, Producent, Ilosc) values ('" + element.Kolor + "','" + element.Producent +
                         "'," + Regex.Replace(element.Ilosc.ToString(), ",", ".") + ")";
            return DodajElement(element, sql);
        }

        override public bool Edytuj(Farba element)
        {
            string sql = "update Farby set kolor = '" + element.Kolor + "', producent = '" + element.Producent +
                                                    "', ilosc = " + Regex.Replace(element.Ilosc.ToString(), ",", ".") + 
                                                    " where IdF = " + element.ID + ";";
            return EdytujElement(element, sql);
        }

        override public bool Usun(Farba element)
        {
            string sql = "delete from Farby where (IdF = " + element.ID + ")";
            return EdytujElement(element, sql);
        }

        override public ObservableCollection<Farba> Pobierz(string warunek = "")
        {
            if (warunek != "") warunek = " where " + warunek;
            string sql = "select * from Farby" + warunek + " order by IdF";
            return PobierzElementy(sql);
        }

        override protected void DodawanieDoListy(SQLiteDataReader reader, ObservableCollection<Farba> elementy)
        {
            elementy.Add(new Farba(Int32.Parse(reader["IdF"].ToString()), reader["Kolor"].ToString(), 
                                   reader["Producent"].ToString(), Double.Parse(reader["Ilosc"].ToString())));
        }
    }
}
