using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundStock
{
    class Program
    {
        static void Main(string[] args)
        {
            Found fd = new Found();
            fd.BuyFound();
            fd.SellFound();
            Console.Read();
        }
    }
}
