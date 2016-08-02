using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outfit
{
    class Sneakers:Finery
    {
        public override void Show()
        {
            Console.Write("Sneakers破球鞋");
            base.Show();
        }
    }
}
