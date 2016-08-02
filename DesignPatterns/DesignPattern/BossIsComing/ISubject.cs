using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossIsComing
{
    interface ISubject
    {
        void Notify();
        string SubjectState { get; set; }
    }
}
