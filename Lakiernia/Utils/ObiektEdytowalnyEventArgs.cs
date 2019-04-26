using Lakiernia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lakiernia.Utils
{
    public class ObiektEdytowalnyEventArgs : EventArgs
    {
        private readonly ObiektEdytowalny _obiekt;

        public ObiektEdytowalnyEventArgs(ObiektEdytowalny obiekt)
        {
            _obiekt = obiekt;
        }

        public ObiektEdytowalny Obiekt
        {
            get
            {
                return _obiekt;
            }
        }
    }
}
