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
        public List<int> ocene = new List<int>();
        public double srednja;
        public int id;
        public bool pol;
        public ucenik(string UCENIK, List<int> ocene, double Srednja, int id, bool pol)
        {
            this.id = id;
            this.UCENIK = UCENIK;
            this.ocene = ocene;
            this.srednja = Math.Round(Srednja / this.ocene.Count(), 2);
            this.pol = pol;

        }
    }
}
