using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightingale
{
    class SimpleFactory
    {
        public static Nightingale Create(string type)
        {
            Nightingale result = null;
            switch(type)
            {
                case "Student":
                    result = new StudentGraduated();
                    break;
                case "Volunteer":
                    result = new Volunteer();
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
