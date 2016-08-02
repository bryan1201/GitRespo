using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDB
{
    interface IDept
    {
        void InsertDept(Dept dept);
        Dept GetDept(int id);
    }
}
