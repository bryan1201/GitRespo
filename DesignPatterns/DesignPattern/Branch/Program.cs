using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Branch
{
    class Program
    {
        static void Main(string[] args)
        {
            RunCompony();
            RunComposite();
        }

        static void RunCompony()
        {
            ConcreteCompany root = new ConcreteCompany("北京總公司");
            root.Add(new HRDepartment("總公司人力資源部"));
            root.Add(new FinanceDepartment("總公司財務部"));

            ConcreteCompany comp = new ConcreteCompany("上海華東分公司");
            comp.Add(new HRDepartment("上海華東分公司人力資源部"));
            comp.Add(new FinanceDepartment("上海華東分公司財務部"));

            root.Add(comp);

            ConcreteCompany comp1 = new ConcreteCompany("南京辦事處");
            comp1.Add(new HRDepartment("南京辦事處人力資源部"));
            comp1.Add(new FinanceDepartment("南京辦事處財務部"));

            comp.Add(comp1);

            ConcreteCompany comp2 = new ConcreteCompany("杭州辦事處");
            comp2.Add(new HRDepartment("杭州辦事處人力資源部"));
            comp2.Add(new FinanceDepartment("杭州辦事處財務部"));

            comp.Add(comp2);

            Console.WriteLine("\n結構圖：");
            root.Display(1);

            Console.WriteLine("\n職責：");
            root.LineOfDuty();

            //Console.Read();
        }

        static void RunComposite()
        {
            Composite root = new Composite("# root");
            root.Add(new Leaf("Leaf A"));
            root.Add(new Leaf("Leaf B"));

            Composite comp = new Composite("Composite X");
            comp.Add(new Leaf("Leaf XA"));
            comp.Add(new Leaf("Leaf XB"));

            root.Add(comp);

            Composite comp2 = new Composite("Composite XY");
            comp2.Add(new Leaf("Leaf XYA"));
            comp2.Add(new Leaf("Leaf XYB"));

            comp.Add(comp2);

            root.Add(new Leaf("Leaf C"));

            Leaf leaf = new Leaf("Leaf D");
            root.Add(leaf);
            root.Display(1);

            root.Remove(leaf);
            root.Display(2);
            Console.Read();
        }
    }
}
