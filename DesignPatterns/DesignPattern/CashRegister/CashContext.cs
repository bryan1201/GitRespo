using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister
{
    //策略模式
    class CashContext
    {
        absCashSuper cs = null;
        public CashContext(string type)
        {
            switch(type)
            {
                case "正常收費":
                    CashNormal cs0 = new CashNormal();
                    cs = cs0;
                    break;
                case "滿300送100":
                    CashReturn cr1 = new CashReturn("300","100");
                    cs = cr1;
                    break;
                case "打八折":
                    CashRebate cr2 = new CashRebate("0.8");
                    cs = cr2;
                    break;
                default:
                    CashNormal csd = new CashNormal();
                    cs = csd;
                    break;
            }
        }

        public double GetResult(double money)
        {
            return cs.acceptCash(money);
        }
    }
}
