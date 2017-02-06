using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossIsComing
{
    class Program
    {
        static void Main(string[] args)
        {
            Boss huhansan = new Boss();
            // 秘書登記通知行為對象
            StockObserver SecretaryNotify1 = new StockObserver("魏關奼", huhansan);
            NBAObserver SecretaryNotify2 = new NBAObserver("易管查", huhansan);
            
            // 事件關聯登記
            huhansan.SecretaryUpdate += new EventHandler(SecretaryNotify1.CloseStockMarket);
            huhansan.SecretaryUpdate += new EventHandler(SecretaryNotify2.CloseNBADirectSeeding);

            huhansan.SubjectState = "我胡漢三老闆回來了！";

            // 老闆告知，秘書第一時間知道。同時觸發上述登記關聯的事件！
            huhansan.Notify();
            Console.Read();
        }
    }
}
