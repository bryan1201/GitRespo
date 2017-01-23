using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitedNation
{
    class UnitedNationsSecurityCouncil: UnitedNations
    {
        private USA colleague1;
        private IRAQ colleague2;

        public USA Colleague1 { set { colleague1 = value; } }

        public IRAQ Colleague2 { set { colleague2 = value; } }

        public override void Declare(string message, Country colleague)
        {
            try
            {
                if (colleague == colleague1)
                    colleague2.GetMessage(message);

                if (colleague == colleague2)
                    colleague1.GetMessage(message);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
