using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayContext
{
    class Scale:Expression
    {
        public override void Excute(string key, double value)
        {
            try
            {
                string scale = string.Empty;
                switch(Convert.ToInt32(value))
                {
                    case 1:
                        scale = "低音";
                        break;
                    case 2:
                        scale = "中音";
                        break;
                    case 3:
                        scale = "高音";
                        break;
                }

                Console.Write("{0} ", scale);
            }
            catch(Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
