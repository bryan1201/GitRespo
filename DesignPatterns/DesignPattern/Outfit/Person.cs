using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outfit
{
    // Decorator裝飾模式
    //ConcreteComponent
    class Person
    {
        public Person()
        { }

        private string _name;
        public Person(string name)
        {
            this._name = name;
        }

        public virtual void Show()
        {
            Console.Write("Nice outfit Person, {0}!", _name);
        }
    }
}
