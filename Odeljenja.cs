using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik_2._0
{
    public class Odeljenja
    {
        public int id_odeljenja;
        public List<ucenik> u = new List<ucenik>();
        
        public Odeljenja(int id)
        {
            this.id_odeljenja = id;
        }
    }
}
