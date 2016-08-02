using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outfit
{
    class TShirts:Finery
    {
        public override void Show()
        {
            Console.Write("TShirts大T恤");
            base.Show();
        }
    }
}
