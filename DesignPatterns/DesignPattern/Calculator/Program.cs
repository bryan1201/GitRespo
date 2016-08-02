using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> listopersign = new List<string>();
            listopersign.Add("+");
            listopersign.Add("-");
            listopersign.Add("*");
            listopersign.Add("/");
            OperationFactory fac = new OperationFactory();

            foreach (var item in listopersign)
            {
                Operation oper = fac.createOperate(item);
                oper.NumberA = 88.8f;
                oper.NumberB = 11.1f;
                double result = oper.GetResult();
                Console.WriteLine("{0:#.###} {1} {2:#.###} = {3:#.###}", oper.NumberA, item, oper.NumberB, result);
            }
            Console.Read();
        }
    }
}
