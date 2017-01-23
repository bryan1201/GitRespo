using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons
{
    interface IAction
    {
        void GetManConclusion(Man concreteElementA);
        void GetWomanConclusion(Woman concreteElementB);
    }
}
