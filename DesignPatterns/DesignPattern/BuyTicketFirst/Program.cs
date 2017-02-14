using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

            Console.WriteLine("\n#{0}：", "yieldReturn()");
            yieldReturn();
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

        static void yieldReturn()
        {
            var myList = new List<int>();
            for (var i = 0; i < 10000000; i++)
            {
                myList.Add(i);
            }

            var timeList = new List<long>();
            for (var i = 0; i < 10; i++)
            {
                var sum = 0;
                var sw = new Stopwatch();
                sw.Start();
                //var list = getEvenNumByYield(myList);
                //var list = getEvenNumByLinq(myList);
                var list = getEvenNumByTemp(myList);
                foreach (var l in list)
                {
                    sum += l;
                }

                // Reuse collection
                foreach (var l in list)
                {
                    sum -= l;
                }
                sw.Stop();
                timeList.Add(sw.ElapsedMilliseconds);
                Console.WriteLine("Exe time: {0} ms", sw.ElapsedMilliseconds);
            }

            Console.WriteLine("Average: {0} ms", (double)timeList.Sum() / 10);
            Console.WriteLine("Memory Usage: {0}", Process.GetCurrentProcess().PrivateMemorySize64);

            Console.Read();
            Console.Read();
        }

        static IEnumerable<int> getEvenNumByYield(IEnumerable<int> collection)
        {
            foreach (var i in collection)
            {
                if (i % 2 == 0) yield return i;
            }
        }

        static IEnumerable<int> getEvenNumByLinq(IEnumerable<int> collection)
        {
            return collection.ToList().Where(i => i % 2 == 0);
        }

        static IEnumerable<int> getEvenNumByTemp(IEnumerable<int> collection)
        {
            var temp = new List<int>();
            foreach (var i in collection)
            {
                if (i % 2 == 0) temp.Add(i);
            }
            return temp;
        }
    }
}
