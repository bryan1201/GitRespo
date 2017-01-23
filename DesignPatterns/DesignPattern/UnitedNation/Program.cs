using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitedNation
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitedNationsSecurityCouncil UNSC = new UnitedNationsSecurityCouncil();
            USA c1 = new USA(UNSC);
            IRAQ c2 = new IRAQ(UNSC);

            UNSC.Colleague1 = c1;
            UNSC.Colleague2 = c2;

            c1.Declare("不準研製核武，否則要發動戰爭！");
            c2.Declare("我們沒有核武，也不怕戰爭侵略！");

            Console.Read();
        }
    }
}
