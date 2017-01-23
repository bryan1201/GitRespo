using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitedNation
{
    class IRAQ: Country
    {
        public IRAQ(UnitedNations mediator): base(mediator)
        { }

        public void Declare(string message)
        {
            mediator.Declare(message, this);
        }

        public void GetMessage(string message)
        {
            string m = string.Format("伊拉克獲得對方的資訊：{0}", message);
            Console.WriteLine(m);
        }
    }
}
