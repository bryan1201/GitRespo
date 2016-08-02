using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister
{
    class CashRebate : absCashSuper
    {
        private double _moneyRebate = 1.0d;
        public CashRebate(string moneyRebate)
        {
            this._moneyRebate = double.Parse(moneyRebate);
        }

        public override double acceptCash(double money)
        {
            double result = money;
            try {
                result = money * _moneyRebate;
            }
            catch
            {
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
