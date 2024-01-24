using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandSetGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Abstraction ab = new RefinedAbstraction();
            ab.SetImplementor(new ConcreteImplementorA());
            ab.Operation();

            ab.SetImplementor(new ConcreteImplementorB());
            ab.Operation();

            Console.Read();
        }
    }
}
