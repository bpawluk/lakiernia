using Lakiernia.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace Lakiernia.Data_Access
{
    public class PracodawcaDAO : DAO<Pracodawca>, IDAO<Pracodawca>, IDisposable
    {
        override public long Dodaj(Pracodawca element)
        {
            string sql = "insert into Pracodawcy (ImiePr, NazwiskoPr, NazwaFirmy, AdresF, NIPF, TelefonPr, EmailPr, Bank, NumerKonta) values ('" +
                         element.Imie + "','" + element.Nazwisko + "','" + element.Firma + "','" + 
                         element.Ulica + ";" + element.Numer + ";" + element.Kod + ";" + element.Miasto + "','" + 
                         element.Nip + "','" + element.Telefon + "','" + element.Email + "','" + element.Bank + "','" + element.Konto + "')";
            return DodajElement(element, sql);
        }

        override public bool Edytuj(Pracodawca element)
        {
            string sql = "update Pracodawcy set ImiePr = '" + element.Imie + "', NazwiskoPr = '" + element.Nazwisko + 
                         "', NazwaFirmy = '" + element.Firma + "', AdresF = '" + element.Ulica + ";" + element.Numer + ";" + element.Miasto + ";" +
                         element.Kod + "', NIPF = '" + element.Nip + "', TelefonPr = '" + element.Telefon + "', EmailPr = '" + element.Email +
                         "', Bank = '" + element.Bank + "', NumerKonta = '" + element.Konto + "' where IdPr = " + element.ID + ";";
            return EdytujElement(element, sql);
        }

        override public bool Usun(Pracodawca element)
        {
            string sql = "delete from Pracodawcy where (IdPr = " + element.ID + ")";
            return EdytujElement(element, sql);
        }

        override public ObservableCollection<Pracodawca> Pobierz(string warunek = "")
        {
            if (warunek != "") warunek = " where " + warunek;
            string sql = "select * from Pracodawcy" + warunek + " order by IdPr";
            return PobierzElementy(sql);
        }

        override protected void DodawanieDoListy(SQLiteDataReader reader, ObservableCollection<Pracodawca> elementy)
        {
            string[] adres = reader["AdresF"].ToString().Split(';');
            elementy.Add(new Pracodawca(long.Parse(reader["IdPr"].ToString()), reader["ImiePr"].ToString(), reader["NazwiskoPr"].ToString(),
                                               reader["NazwaFirmy"].ToString(), adres[0], adres[1], adres[2], adres[3], reader["NIPF"].ToString(),
                                               reader["TelefonPr"].ToString(), reader["EmailPr"].ToString(), reader["Bank"].ToString(), 
                                               reader["NumerKonta"].ToString()));
        }
    }
}