using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outfit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(" // Decorator裝飾模式\n");

            Person p1 = new Person("小菜");
            
            Sneakers s1 = new Sneakers();
            BigTrouser b1 = new BigTrouser();
            TShirts t1 = new TShirts();
            s1.Decorate(p1);
            b1.Decorate(s1);
            t1.Decorate(b1);
            p1.Show();Console.Write("\n");
            s1.Show(); Console.Write("\n");
            b1.Show(); Console.Write("\n");

            Console.Write("\n# 第一種裝扮：");
            t1.Show();

            Console.Write("\n\n");

            LeatherShoes L1 = new LeatherShoes();
            Tie T1 = new Tie();
            Suit S1 = new Suit();
            L1.Decorate(p1);
            T1.Decorate(L1);
            S1.Decorate(T1);

            p1.Show(); Console.Write("\n");
            L1.Show(); Console.Write("\n");
            T1.Show(); Console.Write("\n");

            Console.Write("\n# 第二種裝扮：");
            S1.Show();
            Console.Read();
        }
    }
}
