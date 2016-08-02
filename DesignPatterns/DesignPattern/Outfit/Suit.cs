using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outfit
{
    class Suit:Finery
    {
        public override void Show()
        {
            Console.Write("Suit西裝");
            base.Show();
        }
    }
}
