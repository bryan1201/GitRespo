using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDB
{
    class SqlServerFactory: IFactory
    {
        public IDept CreateDept()
        {
            return new SqlServerDept();
        }

        public IUser CreateUser()
        {
            return new SqlServerUser();
        }
    }

    class OracleServerFactory : IFactory
    {
        public IDept CreateDept()
        {
            return new OracleServerDept();
        }

        public IUser CreateUser()
        {
            return new OracleServerUser();
        }
    }
}
