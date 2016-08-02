using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRaise
{
    class Program
    {
        static void Main(string[] args)
        {
            //Run1();
            Run2();
        }

        static void Run1()
        {
            Handler h1 = new ConcreteHandler1();
            Handler h2 = new ConcreteHandler2();
            Handler h3 = new ConcreteHandler3();
            h1.SetSuccessor(h2);
            h2.SetSuccessor(h3);
            h3.SetSuccessor(h1);

            int[] reqs = { 2, 5, 14, 22, 18, 3, 27, 20 };

            foreach(int req in reqs)
            {
                h1.HandleRequest(req);
            }

            Console.Read();
        }

        static void Run2()
        {
            CommonManager manager = new CommonManager("經理");
            Majordomo major = new Majordomo("總監");
            GeneralManager general = new GeneralManager("總經理");

            manager.SetSuperior(major);
            major.SetSuperior(general);

            Request req = new Request("請假", "員工小菜請假", 1);
            manager.RequestApplications(req);

            Request req2 = new Request("請假", "員工小菜請假", 4);
            manager.RequestApplications(req2);

            Request req3 = new Request("加薪", "員工小菜請求加薪", 500);
            manager.RequestApplications(req3);

            Request req4 = new Request("加薪", "員工小菜請求加薪", 1000);
            manager.RequestApplications(req4);

            manager.SetSuperior(general);
            Request req5 = new Request("加薪&積優報表", "員工小菜請求加薪", 1000);
            manager.RequestApplications(req5);

            Console.Read();
        }
    }
}
