using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyPursuit
{
    //[代理模式]為人作嫁衣
    // 代理模式Proxy，為其他物件提供一種代理行為，以控制對原有物件的存取。[DP]
    // 使用的場合： 遠端代理、虛擬代理、安全代理
    // # 遠端代理，就是為一個物件，在不同的位址空間提供局部代表。如此可以隱藏原有物件存在於不同位址空間的事實。[DP]
    // # 虛擬代理，就是根據需要建立卻耗費資源的物件。透過它來存放耗費資源的真實物件。[DP]。例如：網頁內只存圖片連結，連結真實卻佔記憶體的遠端另一伺服器的圖片。
    // # 安全代理，就是用來控制在存取真實物件時的許可權。[DP]
    // # 智慧參考，是指當調用真實物件時，代理處理另外一些附加的事件。[DP]
    class Proxy : IGiveGift
    {
        Pursuit _p;
        public SchoolGirl Girl { get; set; }
        public string Name { get; set; }
        public Proxy(string proxyname, SchoolGirl mm)
        {
            Name = proxyname;
            Girl = mm;
            _p = new Pursuit(this);
        }
        public void GiveDolls()
        {
            _p.GiveDolls();
        }
        public void GiveFlowers()
        {
            _p.GiveFlowers();
        }
        public void GiveChoolate()
        {
            _p.GiveChoolate();
        }
    }
}
