using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lakiernia.Data_Access
{
    public abstract class DAO<T> : IDAO<T>
    {
        protected SQLiteConnection conn;

        protected DAO()
        {
            conn = App.Open();
        }

        public abstract long Dodaj(T element);

        public abstract bool Edytuj(T element);

        public abstract ObservableCollection<T> Pobierz(string warunek = "");

        public abstract bool Usun(T element);

        protected abstract void DodawanieDoListy(SQLiteDataReader reader, ObservableCollection<T> elementy);

        protected long DodajElement(T element, string sql)
        {
            long noweID = -1;

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                SQLiteTransaction transaction = conn.BeginTransaction();
                command.ExecuteNonQuery();
                noweID = conn.LastInsertRowId;
                transaction.Commit();
            }
            catch { }

            return noweID;
        }

        protected bool EdytujElement(T element, string sql)
        {
            bool czyEdytowano = false;

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                if (command.ExecuteNonQuery() > 0) czyEdytowano = true;
            }
            catch { }

            return czyEdytowano;
        }

        protected ObservableCollection<T> PobierzElementy(string sql)
        {
            ObservableCollection<T> elementy = new ObservableCollection<T>();
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read()) DodawanieDoListy(reader, elementy);
            }
            catch { }

            return elementy;
        }

        public void Dispose()
        {
            conn.Close();
        }
    }
}
