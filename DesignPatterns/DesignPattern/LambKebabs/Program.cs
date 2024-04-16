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
            Barbecuer boy = new Barbecuer(); //Receiver
            Command bakeCmd1 = new BakeMuttonCommand(boy); //ConcreteCommand1
            Command bakeCmd2 = new BakeMuttonCommand(boy); //ConcreteCommand1
            Command bakeCmd3 = new BakeChickenWingCommand(boy); //ConcreteCommand2
            Waiter girl = new Waiter(); //Invoker
            girl.SetOrder(bakeCmd1);
            girl.SetOrder(bakeCmd2);
            girl.SetOrder(bakeCmd3);
            girl.CancelOrder(bakeCmd3);
            bakeCmd3 = new BakeMuttonCommand(boy);
            girl.SetOrder(bakeCmd3);
            girl.Notify();
            Console.Read();
        }
    }
}
