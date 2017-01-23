using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayContext
{
    class Note : Expression
    {
        public override void Excute(string key, double value)
        {
            try
            {
                string note = string.Empty;
                switch(key)
                {
                    case "C":
                        note = "1";
                        break;
                    case "D":
                        note = "2";
                        break;
                    case "E":
                        note = "3";
                        break;
                    case "F":
                        note = "4";
                        break;
                    case "G":
                        note = "5";
                        break;
                    case "A":
                        note = "6";
                        break;
                    case "B":
                        note = "7";
                        break;
                }

                Console.Write("{0} ", note);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }

        }
    }
}
