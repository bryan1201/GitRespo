using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyTicketFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n#{0}：", "RunIterator");
            RunIterator();

            Console.WriteLine("\n#{0}：", "RunList");
            RunList();

            Console.WriteLine("\n#{0}：", "RunEnumerator()");
            RunEnumerator();
        }

        static void RunIterator()
        {
            ConcreteAggregate a = new ConcreteAggregate();
            a[0] = "乘客大鳥";
            a[1] = "乘客小菜";
            a[2] = "乘客";
            a[3] = "乘客的行李";
            a[4] = "乘客老外(說英文：Please buy ticket!)";
            a[5] = "客運女員工";
            a[6] = "偷上車 & 偷手機的小偷";

            a[2] = "乘客1";
            a[3] = "乘客1的行李";
            Iterator i = new ConcreteIterator(a);
            object item = i.First();
            while(!i.IsDone())
            {
                Console.WriteLine("{0}，請買車票！", i.CurrentItem());
                i.Next();
            }

            Console.Read();
        }

        static void RunList()
        {
            IList<string> a = new List<string>();
            a.Add("乘客大鳥");
            a.Add("乘客小菜");
            a.Add("乘客");
            a.Add("乘客的行李");
            a.Add("乘客老外(說英文：Please buy ticket!)");
            a.Add("客運女員工");
            a.Add("偷上車 & 偷手機的小偷");

            foreach(string item in a)
            {
                Console.WriteLine("{0}，請買車票！", item);
            }

            IEnumerator<string> i = a.GetEnumerator();
            while(i.MoveNext())
            {

            }

            Console.Read();
        }

        static void RunEnumerator()
        {
            IList<string> a = new List<string>();
            a.Add("乘客大鳥");
            a.Add("乘客小菜");
            a.Add("乘客");
            a.Add("乘客的行李");
            a.Add("乘客老外(說英文：Please buy ticket!)");
            a.Add("客運女員工");
            a.Add("偷上車 & 偷手機的小偷");

            IEnumerator<string> i = a.GetEnumerator();
            while (i.MoveNext())
            {
                Console.WriteLine("{0}，請買車票！", i.Current);
            }

            Console.Read();
        }
    }
}
