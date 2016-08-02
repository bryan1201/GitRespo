using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            // Template 樣版
            AbstractClass c;
            c = new ConcreateClassA();
            c.TemplateMethod();

            c = new ConcreateClassB();
            c.TemplateMethod();

            Console.Read();
        }
    }
}
