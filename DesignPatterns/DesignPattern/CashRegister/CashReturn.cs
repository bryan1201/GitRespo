using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister
{
    class CashReturn:absCashSuper
    {
        private double _moneyCondition = 0.0d;
        private double _moneyReturn = 0.0d;
        public CashReturn(string moneyCondition, string moneyReturn)
        {
            double.TryParse(moneyCondition, out this._moneyCondition);
            double.TryParse(moneyReturn, out this._moneyReturn);
        }
        public override double acceptCash(double money)
        {
            double result = money;
            try
            {
                if (money >= _moneyCondition)
                    result = money - Math.Floor(money / _moneyCondition) * _moneyReturn;
            }
            catch
            {
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
