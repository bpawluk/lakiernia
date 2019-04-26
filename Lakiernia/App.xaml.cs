using Lakiernia.View;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Lakiernia
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static String _nazwaPliku = "Lakiernia.sqlite";

        public static SQLiteConnection Open()
        {
            SQLiteConnection databaseConnection = new SQLiteConnection("Data Source=" + _nazwaPliku + ";Version=3;");
            databaseConnection.Open();
            String sql = "pragma foreign_keys = on";
            SQLiteCommand cmd = new SQLiteCommand(sql, databaseConnection);
            cmd.ExecuteNonQuery();
            return databaseConnection;
        }
    }
}
