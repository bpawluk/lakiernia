using Lakiernia.Data_Access;
using Lakiernia.Model;
using Lakiernia.Utils;
using System.Linq;
using System.Windows.Input;

namespace Lakiernia.View_Model
{
    class DanePracodawcyVM : ObiektEdytowalny
    {
        private Pracodawca _pracodawca;
        private ICommand _zapiszKmd;

        public Pracodawca Pracodawca
        {
            get => _pracodawca;
            set
            {
                _pracodawca = value;
                OnPropertyChanged("Pracodawca");
            }
        }

        public ICommand ZapiszKmd
        {
            get
            {
                if (_zapiszKmd == null) _zapiszKmd = new Komenda(Zapisz);
                return _zapiszKmd;
            }
        }

        public DanePracodawcyVM()
        {
            using(PracodawcaDAO bd = new PracodawcaDAO()) _pracodawca = bd.Pobierz().FirstOrDefault();
        }
        
        private void Zapisz(object parametr)
        {
            using (PracodawcaDAO bd = new PracodawcaDAO()) bd.Edytuj(_pracodawca);
        }
    }
}
