using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister
{
    class CashNormal : absCashSuper
    {
        public override double acceptCash(double money)
        {
            return money;
        }
    }
}
