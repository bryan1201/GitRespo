using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDB
{
    interface IUser
    {
        void InsertUser(User user);
        User GetUser(int id);
    }
}
