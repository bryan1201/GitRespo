using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Branch
{
    public abstract class Component
    {
        protected string name;
        public Component(string name)
        { this.name = name; }

        public abstract void Add(Component c);
        public abstract void Remove(Component c);
        public abstract void Display(int depth);
    }

    public class Leaf: Component
    {
        public Leaf(string name):base(name)
        { }

        public override void Add(Component c)
        {
            Console.WriteLine("Cannot add to a leaf!");
        }

        public override void Remove(Component c)
        {
            Console.WriteLine("Cannot remove from a leaf!");
        }

        public override void Display(int depth)
        {
            string leaf = string.Format("{0}{1}", new string('-', depth), name);
            Console.WriteLine(leaf);
        }
    }

    public class Composite:Component
    {
        private List<Component> children = new List<Component>();
        public Composite(string name):base(name)
        { }

        public override void Add(Component c)
        {
            children.Add(c);
        }

        public override void Remove(Component c)
        {
            children.Remove(c);
        }

        public override void Display(int depth)
        {
            string leaf = string.Format("{0}{1}", new string('-', depth), name);
            Console.WriteLine(leaf);

            foreach(Component component in children)
            {
                component.Display(depth + 2);
            }
        }
    }
}
