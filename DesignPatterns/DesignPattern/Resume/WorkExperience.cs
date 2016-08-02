using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    class WorkExperience : ICloneable
    {
        private string _workDate;
        public string WorkDate
        {
            get { return _workDate; }
            set { _workDate = value; }
        }
        private string _company;
        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public Object Clone()
        {
            return (Object)this.MemberwiseClone();
        }
    }
}
