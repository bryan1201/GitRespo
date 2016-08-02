using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister
{
    class Program
    {
        static double total = 0.0d;
        static void Main(string[] args)
        {
            Console.Write("//策略模式\n");

            string output = "";
            double Price = 350.0d;
            double Number = 10.0d;
            double totalPrice = 0.0d;
            //CashContext csuper = new CashContext("正常收費");
            //CashContext csuper = new CashContext("滿300送100");
            CashContext csuper = null;
            

            Console.Write("#1 正常收費\n");

            csuper = new CashContext("正常收費");

            totalPrice = csuper.GetResult(Price * Number);
            total += totalPrice;
            output = string.Format("Unit Price:{0:###.###}\nQuantity:{1:###}\nSum:{2:###.###}\n\n", Price, Number, total);
            Console.Write(output);

            Console.Write("#2 打八折\n");
            csuper = new CashContext("打八折");
            total = 0.0d;
            totalPrice = csuper.GetResult(Price * Number);
            total += totalPrice;
            output = string.Format("Unit Price:{0:###.###}\nQuantity:{1:###}\nSum:{2:###.###}\n\n", Price, Number, total);
            Console.Write(output);

            Console.Write("#3 滿300送100\n");
            csuper = new CashContext("滿300送100");
            total = 0.0d;
            totalPrice = csuper.GetResult(Price * Number);
            total += totalPrice;
            output = string.Format("Unit Price:{0:###.###}\nQuantity:{1:###}\nSum:{2:###.###}\n", Price, Number, total);
            Console.Write(output);
            Console.Read();
        }
    }
}
