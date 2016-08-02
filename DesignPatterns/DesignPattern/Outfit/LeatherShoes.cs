using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outfit
{
    class LeatherShoes:Finery
    {
        public override void Show()
        {
            Console.Write("LeatherShoes皮鞋");
            base.Show();
        }
    }
}
