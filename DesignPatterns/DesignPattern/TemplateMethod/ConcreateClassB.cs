using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMethod
{
    class ConcreateClassB:AbstractClass
    {
        public override void PrimitiveOperation1()
        {
            Console.WriteLine("具體類別B方法1實現");
        }

        public override void PrimitiveOperation2()
        {
            Console.WriteLine("具體類別B方法2實現");
        }
    }
}
