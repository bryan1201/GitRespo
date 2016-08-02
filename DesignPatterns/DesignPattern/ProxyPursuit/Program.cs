using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyPursuit
{
    class Program
    {
        static void Main(string[] args)
        {
            SchoolGirl agirl = new SchoolGirl();
            agirl.Name = "李嬌嬌";
            Proxy pp = new Proxy("DaiLi" , agirl);
            Give(pp);

            agirl.Name = "可可";
            pp = new Proxy("Chung", agirl);
            Give(pp);
            Console.Read();
        }

        static void Give(Proxy p)
        {
            Console.Write("# {0}\n", p.Name);
            p.GiveDolls();
            p.GiveFlowers();
            p.GiveChoolate();
        }
    }
}
