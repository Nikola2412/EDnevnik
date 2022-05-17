using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik_2._0
{
    public class Predmet
    {
        public int index;
        public string ime;
        public List<Tuple<int, string>> ocena_opis = new List<Tuple<int, string>>();
        public Predmet(int index,string ime)
        {
            this.index = index;
            this.ime = ime;
        }
        public Predmet( string ime)
        {
            this.ime = ime;
        }
    }
}
