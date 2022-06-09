using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik_2._0
{
    public class Predmet
    {
        //public int index;
        public string ime;
        public List<Tuple<int, string>> ocena_opis = new List<Tuple<int, string>>();
        public double prosek;
        public Predmet(string ime)//int index,string ime)
        {
            //this.index = index;
            this.ime = ime;
        }
        public Predmet(List<Tuple<int, string>> oo)
        {
            this.ocena_opis = oo;
        }
        public void racunaj()
        {
            foreach (var item in ocena_opis)
            {
                prosek += item.Item1;
            }
            prosek /= ocena_opis.Count();
            //return prosek.ToString("0.00");
        }
    }
}
