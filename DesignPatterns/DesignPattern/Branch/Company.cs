using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Branch
{
    public abstract class Company
    {
        protected string name;
        public Company(string name)
        { this.name = name; }

        public abstract void Add(Company c);
        public abstract void Remove(Company c);
        public abstract void Display(int depth);
        public abstract void LineOfDuty();
    }

    public class ConcreteCompany : Company
    {
        private List<Company> children = new List<Company>();
        public ConcreteCompany(string name) : base(name)
        { }

        public override void Add(Company c)
        {
            children.Add(c);
        }

        public override void Remove(Company c)
        {
            children.Remove(c);
        }

        public override void Display(int depth)
        {
            string leaf = string.Format("{0}{1}", new string('-', depth), name);
            Console.WriteLine(leaf);

            foreach (Company component in children)
            {
                component.Display(depth + 2);
            }
        }

        public override void LineOfDuty()
        {
            foreach (Company component in children)
            {
                component.LineOfDuty();
            }
        }
    }

    public class HRDepartment : Company
    {
        public HRDepartment(string name) : base(name)
        { }

        public override void Add(Company c)
        {
        }

        public override void Remove(Company c)
        {
        }

        public override void Display(int depth)
        {
            string leaf = string.Format("{0}{1}", new string('-', depth), name);
            Console.WriteLine(leaf);
        }

        public override void LineOfDuty()
        {
            Console.WriteLine("{0} 員工招聘培訓管理", name);
        }
    }

    public class FinanceDepartment : Company
    {
        public FinanceDepartment(string name) : base(name)
        { }

        public override void Add(Company c)
        {
        }

        public override void Remove(Company c)
        {
        }

        public override void Display(int depth)
        {
            string leaf = string.Format("{0}{1}", new string('-', depth), name);
            Console.WriteLine(leaf);
        }

        public override void LineOfDuty()
        {
            Console.WriteLine("{0} 公司財務收支管理", name);
        }
    }
}
