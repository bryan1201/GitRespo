using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightingale
{
    class Volunteer : Nightingale
    {
        public Volunteer()
        {
            Console.WriteLine(string.Format("{0}, {1}", this.ToString(), "社區義工作護士"));
        }
    }
}
