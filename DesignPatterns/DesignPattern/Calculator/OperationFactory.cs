using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    //簡單工廠模式
    public class OperationFactory
    {
        public OperationFactory() { }
        public Operation createOperate(string operate)
        {
            Operation oper = null;
            switch(operate)
            {
                case "+":
                    oper = new Add();
                    break;
                case "-":
                    oper = new Sub();
                    break;
                case "*":
                    oper = new Mul();
                    break;
                case "/":
                    oper = new Div();
                    break;
                default:
                    break;
            }
            return oper;
        }
    }

    public class Add : Operation
    {
        public override double GetResult()
        {
            double result = 0;
            result = NumberA + NumberB;
            return result;
        }
    }

    public class Sub : Operation
    {
        public override double GetResult()
        {
            double result = 0;
            result = NumberA - NumberB;
            return result;
        }
    }

    public class Mul : Operation
    {
        public override double GetResult()
        {
            double result = 0;
            result = NumberA * NumberB;
            return result;
        }
    }

    public class Div : Operation
    {
        public override double GetResult()
        {
            double result = 0;
            if (NumberB == 0)
                throw new Exception("Error, Can not divided to zero!");
            result = NumberA / NumberB;
            return result;
        }
    }
}
