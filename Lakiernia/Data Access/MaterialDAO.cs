using Lakiernia.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace Lakiernia.Data_Access
{
    public class MaterialDAO : DAO<Material>, IDAO<Material>, IDisposable
    {
        override public long Dodaj(Material element)
        {
            string sql = "insert into Materialy (NazwaM, Dlugosc, Szerokosc) values " +
                         "('" + element.Nazwa + "'," + element.Dlugosc + "," + element.Szerokosc + ")";
            return DodajElement(element, sql);
        }

        override public bool Edytuj(Material element)
        {
            string sql = "update Materialy set NazwaM = '" + element.Nazwa + "', Dlugosc = " + element.Dlugosc +
                         ", Szerokosc = " + element.Szerokosc + " where IdM = " + element.ID + ";";
            return EdytujElement(element, sql);
        }

        override public bool Usun(Material element)
        {
            string sql = "delete from Materialy where (IdM = " + element.ID + ")";
            return EdytujElement(element, sql);
        }

        override public ObservableCollection<Material> Pobierz(string warunek = "")
        {
            if (warunek != "") warunek = " where " + warunek;
            string sql = "select * from Materialy" + warunek + " order by IdM";
            return PobierzElementy(sql);
        }

        override protected void DodawanieDoListy(SQLiteDataReader reader, ObservableCollection<Material> elementy)
        {
            elementy.Add(new Material(Int32.Parse(reader["IdM"].ToString()), reader["NazwaM"].ToString(),
                                      uint.Parse(reader["Dlugosc"].ToString()), uint.Parse(reader["Szerokosc"].ToString())));
        }
    }
}