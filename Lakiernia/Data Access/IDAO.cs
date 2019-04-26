using System.Collections.ObjectModel;

namespace Lakiernia.Data_Access
{
    public interface IDAO<T>
    {
        long Dodaj(T element);
        bool Edytuj(T element);
        bool Usun(T element);
        ObservableCollection<T> Pobierz(string warunek = "");
    }
}
