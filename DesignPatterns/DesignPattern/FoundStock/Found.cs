using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundStock
{
    class Found
    {
        Stock1 gu1;
        Stock2 gu2;
        Stock3 gu3;
        NationalDebt1 nd1;
        NationalDebt2 nd2;
        Realty1 rt1;
        Realty2 rt2;

        public Found()
        {
            gu1 = new Stock1();
            gu2 = new Stock2();
            gu3 = new Stock3();
            nd1 = new NationalDebt1();
            nd2 = new NationalDebt2();
            rt1 = new Realty1();
            rt2 = new Realty2();
        }

        public void BuyFound()
        {
            gu1.Buy();
            gu2.Buy();
            gu3.Buy();
            nd1.Buy();
            nd2.Buy();
            rt1.Buy();
            rt2.Buy();
        }

        public void SellFound()
        {
            gu1.Sell();
            gu2.Sell();
            gu3.Sell();
            nd1.Sell();
            nd2.Sell();
            rt1.Sell();
            rt2.Sell();
        }
    }
}
