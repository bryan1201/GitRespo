using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightingale
{
    class Program
    {
        // Factory Pattern
        static void Main(string[] args)
        {
            Console.WriteLine("Simple Factory Pattern");
            Nightingale stA = SimpleFactory.Create("Student");
            stA.BuyRice();
            Nightingale stB = SimpleFactory.Create("Student");
            stB.Sweep();
            Nightingale stC = SimpleFactory.Create("Student");
            stC.Wash();
            Console.WriteLine("按任意鍵...執行工廠模式");
            Console.Read();
            Console.WriteLine("Factory Pattern");
            IFactory factory = new UndergraduateFactory();
            Nightingale stD = factory.Create();

            stD.BuyRice();
            stD.Sweep();
            stD.Wash();
            Console.Read();
            Console.Read();
        }
    }
}
