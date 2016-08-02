using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightingale
{
    class UndergraduateFactory:IFactory
    {
        public Nightingale Create()
        {
            return new Undergraduate();
        }
    }
}
