using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossIsComing
{
    class NBAObserver
    {
        private string _name;
        private ISubject _sub;
        public NBAObserver(string name, ISubject sub)
        {
            this._name = name;
            this._sub = sub;
        }

        // 關閉股票行情
        public void CloseNBADirectSeeding()
        {
            Console.WriteLine("{0} {1} 關閉NBA直播，繼續工作！", this._sub.SubjectState, this._name);
        }
    }
}
