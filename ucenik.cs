using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik_2._0
{
    public class ucenik
    {
        public string UCENIK;
        public double srednja;
        public List<Tuple<int,string>> Ocena_opis = new List<Tuple<int, string>>();
        public int id;
        public bool pol;
        public ucenik(string UCENIK, List<Tuple<int,string>> ocene, double Srednja, int id, bool pol)
        {
            this.id = id;
            this.UCENIK = UCENIK;
            this.Ocena_opis = ocene;

            this.srednja = Math.Round(Srednja / this.Ocena_opis.Count(), 2);
            this.pol = pol;

        }
    }
}
