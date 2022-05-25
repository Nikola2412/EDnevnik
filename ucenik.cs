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
        //tuple ocena i opis nje
        public List<Tuple<int,int,string>> id_Ocena_opis = new List<Tuple<int,int, string>>();
        public int id;
        public bool pol;
        public ucenik(string UCENIK, List<Tuple<int,int,string>> ocene, double Srednja, int id, bool pol)
        {
            this.id = id;
            this.UCENIK = UCENIK;
            this.id_Ocena_opis = ocene;
            //srednja ocena
            this.srednja = Math.Round(Srednja / this.id_Ocena_opis.Count(), 2);
            this.pol = pol;

        }
    }
}
