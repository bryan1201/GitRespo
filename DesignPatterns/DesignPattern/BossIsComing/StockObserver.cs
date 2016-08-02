using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossIsComing
{
    class StockObserver
    {
        private string _name;
        private ISubject _sub;
        public StockObserver(string name, ISubject sub)
        {
            this._name = name;
            this._sub = sub;
        }

        // 關閉股票行情
        public void CloseStockMarket()
        {
            Console.WriteLine("{0} {1} 關閉股票行情，繼續工作！", this._sub.SubjectState, this._name);
        }
    }
}
