using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons
{
    interface IPerson
    {
        //public string Action { get; set; }
        void Accept(IAction visitor);
    }
}
