using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outfit
{
    // 服飾類別
    class Finery: Person
    {
        protected Person _component;

        public void Decorate(Person component)
        {
            this._component = component;
        }

        public override void Show()
        {
            if (_component != null)
            {
                Console.Write(" {");
                _component.Show();
                Console.Write("} ");
            }
            else
            {
                base.Show();
            }
        }
    }
}
