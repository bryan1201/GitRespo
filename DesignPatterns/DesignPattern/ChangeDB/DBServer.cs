using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDB
{
    // SqlServer
    class SqlServerUser : IUser
    {
        public void InsertUser(User user)
        {
            Console.WriteLine("Insert User to SqlServer DB Table.");
        }

        public User GetUser(int id)
        {
            string result = string.Format("Get User Where ID = {0} from SqlServer DB User Table.", id.ToString());
            Console.WriteLine(result);
            return null;
        }
    }

    class SqlServerDept : IDept
    {
        public void InsertDept(Dept dept)
        {
            Console.WriteLine("Insert Dept to SqlServer DB Table.");
        }

        public Dept GetDept(int id)
        {
            string result = string.Format("Get Dept Where ID = {0} from SqlServer DB Dept Table.", id.ToString());
            Console.WriteLine(result);
            return null;
        }
    }

    // Oracle Server
    class OracleServerUser : IUser
    {
        public void InsertUser(User user)
        {
            Console.WriteLine("Insert User to OracleServer DB Table.");
        }

        public User GetUser(int id)
        {
            string result = string.Format("Get User Where ID = {0} from OracleServer DB User Table.", id.ToString());
            Console.WriteLine(result);
            return null;
        }
    }

    class OracleServerDept : IDept
    {
        public void InsertDept(Dept dept)
        {
            Console.WriteLine("Insert Dept to OracleServer DB Table.");
        }

        public Dept GetDept(int id)
        {
            string result = string.Format("Get Dept Where ID = {0} from OracleServer DB Dept Table.", id.ToString());
            Console.WriteLine(result);
            return null;
        }
    }
}
