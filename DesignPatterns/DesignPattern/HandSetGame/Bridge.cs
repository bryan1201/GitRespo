using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandSetGame
{
    abstract class Implementor
    {
        public abstract void Operation();
    }
    class ConcreteImplementorA: Implementor
    {
        public override void Operation()
        {
            Console.WriteLine("具體實現A的方法執行");
        }
    }

    class ConcreteImplementorB : Implementor
    {
        public override void Operation()
        {
            Console.WriteLine("具體實現B的方法執行");
        }
    }

    class Abstraction
    {
        protected Implementor implementor;

        public void SetImplementor(Implementor _imp) {
            this.implementor = _imp;
        }
        public virtual void Operation()
        {
            implementor.Operation();
        }
    }

    class RefinedAbstraction: Abstraction
    {
        public override void Operation()
        {
            implementor.Operation();
        }
    }
}
