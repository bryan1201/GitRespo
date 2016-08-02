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
            StockObserver tongshi1 = new StockObserver("魏關奼", huhansan);
            NBAObserver tongshi2 = new NBAObserver("易管查", huhansan);
            huhansan.Update += new EventHandler(tongshi1.CloseStockMarket);
            huhansan.Update += new EventHandler(tongshi2.CloseNBADirectSeeding);

            huhansan.SubjectState = "我胡漢三老闆回來了！";
            huhansan.Notify();
            Console.Read();
        }
    }
}
