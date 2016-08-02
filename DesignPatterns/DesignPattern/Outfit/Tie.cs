using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outfit
{
    class Tie:Finery
    {
        public override void Show()
        {
            Console.Write("Tie領帶");
            base.Show();
        }
    }
}
