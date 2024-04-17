using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightingale
{
    //Graduated from nursing school
    class StudentGraduated : Nightingale
    {
        public StudentGraduated()
        {
            Console.WriteLine(string.Format("{0}, {1}",this.ToString(), "from nursing school, 護校畢業生作護士"));
        }
    }
}
