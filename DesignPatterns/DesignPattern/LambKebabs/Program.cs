using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambKebabs
{
    class Program
    {
        static void Main(string[] args)
        {
            Barbecuer boy = new Barbecuer();
            Command bakeCmd1 = new BakeMuttonCommand(boy);
            Command bakeCmd2 = new BakeMuttonCommand(boy);
            Command bakeCmd3 = new BakeChickenWingCommand(boy);
            Waiter girl = new Waiter();
            girl.SetOrder(bakeCmd1);
            girl.SetOrder(bakeCmd2);
            girl.SetOrder(bakeCmd3);
            girl.Notify();
            Console.Read();
        }
    }
}
