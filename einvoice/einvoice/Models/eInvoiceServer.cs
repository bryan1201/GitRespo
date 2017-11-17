using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace einvoice.Models
{
    public class eInvoiceServer
    {
    }

    public class DEVServerMTDBCollection : IMTDBCollection
    {
        private string config = Constant.DEVDBContext;
        private MTDBCollection mtdbcollection;
        public IEnumerable<TURNKEY_MESSAGE_LOG> Get(TURNKEY_MESSAGE_LOG mtdb)
        {
            mtdbcollection = new MTDBCollection(config);
            mtdbcollection.Get(mtdb);
            return mtdbcollection.MTDBList;
        }

        public string GetSqlString()
        {
            return mtdbcollection.GetSqlString();
        }
    }

    public class QASServerMTDBCollection : IMTDBCollection
    {
        private string config = Constant.QASDBContext;
        private MTDBCollection mtdbcollection;
        public IEnumerable<TURNKEY_MESSAGE_LOG> Get(TURNKEY_MESSAGE_LOG mtdb)
        {
            mtdbcollection = new MTDBCollection(config);
            mtdbcollection.Get(mtdb);
            return mtdbcollection.MTDBList;
        }

        public string GetSqlString()
        {
            return mtdbcollection.GetSqlString();
        }
    }

    public class PRDServerMTDBCollection : IMTDBCollection
    {
        private string config = Constant.PRDDBContext;
        private MTDBCollection mtdbcollection;
        public IEnumerable<TURNKEY_MESSAGE_LOG> Get(TURNKEY_MESSAGE_LOG mtdb)
        {
            mtdbcollection = new MTDBCollection(config);
            mtdbcollection.Get(mtdb);
            return mtdbcollection.MTDBList;
        }

        public string GetSqlString()
        {
            return mtdbcollection.GetSqlString();
        }
    }
}