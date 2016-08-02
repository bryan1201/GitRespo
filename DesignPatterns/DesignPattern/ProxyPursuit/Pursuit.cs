using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyPursuit
{
    class Pursuit : IGiveGift
    {
        Proxy _proxy;
        public Pursuit(Proxy p)
        {
            _proxy = p;
        }

        public void GiveDolls()
        {
            Console.Write("{0} Say: {1}, 送你洋娃娃\n", _proxy.Name, _proxy.Girl.Name );
        }
    
        public void GiveFlowers()
        {
            Console.Write("{0} Say: {1}, 送你花\n", _proxy.Name, _proxy.Girl.Name);
        }

        public void GiveChoolate()
        {
            Console.Write("{0} Say: {1}, 送你巧克力\n", _proxy.Name, _proxy.Girl.Name);
        }
    }
}
