using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons
{
    class ObjectStructure
    {
        private IList<IPerson> elements = new List<IPerson>();

        public void Attach(IPerson element)
        {
            elements.Add(element);
        }

        public void Detach(IPerson element)
        {
            elements.Remove(element);
        }

        public void Display(IAction visitor)
        {
            foreach(IPerson e in elements)
            {
                e.Accept(visitor);
            }
        }
    }
}
