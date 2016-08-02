using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBATranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            Player b = new Forwards("Badier");
            b.Attack();

            Player m = new Guards("Mack Grady");
            m.Attack();

            Player ym = new Translator("姚明");
            ym.Attack();
            ym.Defense();

            Console.Read();
        }
    }
}
