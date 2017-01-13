using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons
{
    abstract class Person
    {
        //public string Action { get; set; }
        public abstract void Accept(Action visitor);
    }
}
