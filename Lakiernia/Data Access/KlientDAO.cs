using Lakiernia.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace Lakiernia.Data_Access
{
    public class KlientDAO : DAO<Klient>, IDAO<Klient>, IDisposable
    {
        override public long Dodaj(Klient element)
        {
            string sql = "insert into Klienci (NazwaK, TelefonK, EmailK, AdresK, NIPK, CzyOsoba) values ('" + element.Nazwa + "','" +
                         element.Telefon + "','" + element.Email + "','" + element.Ulica + ";" + element.Numer + ";" + 
                         element.Kod + ";" + element.Miasto + "','" + element.Nip + "','" + (element.Typ == TypKlienta.Osoba ? 0 : 1) + "')";
            return DodajElement(element, sql);
        }

        override public bool Edytuj(Klient element)
        {
            string sql = "update Klienci set NazwaK = '" + element.Nazwa + "', TelefonK = '" + element.Telefon + "', EmailK = '" + 
                         element.Email + "', AdresK = '" + element.Ulica + ";" + element.Numer + ";" + element.Miasto + ";" + 
                         element.Kod + "', NIPK = '" + element.Nip + "', czyOsoba = " + (element.Typ == TypKlienta.Osoba ? 0 : 1) +
                         " where IdK = " + element.ID + ";";
            return EdytujElement(element, sql);
        }

        override public bool Usun(Klient element)
        {
            string sql = "delete from Klienci where (IdK = " + element.ID + ")";
            return EdytujElement(element, sql);
        }

        override public ObservableCollection<Klient> Pobierz(string warunek = "")
        {
            if (warunek != "") warunek = " where " + warunek;
            string sql = "select * from Klienci" + warunek + " order by NazwaK";
            return PobierzElementy(sql);
        }

        override protected void DodawanieDoListy(SQLiteDataReader reader, ObservableCollection<Klient> elementy)
        {
            string[] adres = reader["AdresK"].ToString().Split(';');
            elementy.Add(new Klient(long.Parse(reader["IdK"].ToString()), reader["NazwaK"].ToString(), reader["TelefonK"].ToString(), 
                                               reader["EmailK"].ToString(), adres[0], adres[1], adres[2], adres[3], reader["NIPK"].ToString(), 
                                               reader["czyOsoba"].ToString().Equals("0") ? TypKlienta.Osoba : TypKlienta.Firma));
        }
    }
}