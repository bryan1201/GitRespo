using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDB
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User();
            Dept dept = new Dept();
            IUser iuser = DataAccess.CreateUser();
            iuser.InsertUser(user);
            iuser.GetUser(1);

            IDept idept = DataAccess.CreateDept();
            idept.InsertDept(dept);
            idept.GetDept(0102);
            
            Console.Read();
        }
    }
}
