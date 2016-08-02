using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRaise
{
    public abstract class Handler
    {
        protected Handler successor { get; set; }
        public void SetSuccessor(Handler succ)
        {
            this.successor = succ;
        }
        public abstract void HandleRequest(int Request);
    }

    public class ConcreteHandler1: Handler
    {
        public override void HandleRequest(int Request)
        {
            if(Request >=0 && Request <10)
            {
                Console.WriteLine("{0} 處理請求 {1}", this.GetType().Name, Request);
            }
            else if(successor != null)
            {
                successor.HandleRequest(Request);
            }
        }
    }

    public class ConcreteHandler2 : Handler
    {
        public override void HandleRequest(int Request)
        {
            if (Request >= 10 && Request < 20)
            {
                Console.WriteLine("{0} 處理請求 {1}", this.GetType().Name, Request);
            }
            else if (successor != null)
            {
                successor.HandleRequest(Request);
            }
        }
    }

    public class ConcreteHandler3 : Handler
    {
        public override void HandleRequest(int Request)
        {
            if (Request >= 20 && Request < 30)
            {
                Console.WriteLine("{0} 處理請求 {1}", this.GetType().Name, Request);
            }
            else if (successor != null)
            {
                successor.HandleRequest(Request);
            }
        }
    }
}
