using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    class Program
    {
        // 原型模式，深層複製
        static void Main(string[] args)
        {
            Console.WriteLine("ProtoType Resume");
            Resume a = new Resume("a");
            a.SetPersonalInfo("Male", "29");
            a.SetWorkExperience("1998-2000", "XX Company");

            Resume b = (Resume)a.Clone();
            b.SetWorkExperience("1998-2006", "YY Company");

            Resume c = (Resume)a.Clone();
            c.SetPersonalInfo("Male", "24");
            c.SetWorkExperience("1998-2003", "ZZ Company");

            a.Display();
            b.Display();
            c.Display();

            Console.Read();
        }
    }
}
