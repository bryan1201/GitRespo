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

    public class DEVServerSYSEVENTDBCollection : ISYSEVENTDBCollection
    {
        private string config = Constant.DEVDBContext;
        private SYSEVENTDBCollection syseventdbcollection;
        public IEnumerable<TURNKEY_SYSEVENT_LOG> Get(TURNKEY_SYSEVENT_LOG syseventdb)
        {
            syseventdbcollection = new SYSEVENTDBCollection(config);
            syseventdbcollection.Get(syseventdb);
            return syseventdbcollection.SYSEVENTDBList;
        }

        public string GetSqlString()
        {
            return syseventdbcollection.GetSqlString();
        }
    }

    public class QASServerSYSEVENTDBCollection : ISYSEVENTDBCollection
    {
        private string config = Constant.QASDBContext;
        private SYSEVENTDBCollection syseventdbcollection;
        public IEnumerable<TURNKEY_SYSEVENT_LOG> Get(TURNKEY_SYSEVENT_LOG syseventdb)
        {
            syseventdbcollection = new SYSEVENTDBCollection(config);
            syseventdbcollection.Get(syseventdb);
            return syseventdbcollection.SYSEVENTDBList;
        }

        public string GetSqlString()
        {
            return syseventdbcollection.GetSqlString();
        }
    }

    public class PRDServerSYSEVENTDBCollection : ISYSEVENTDBCollection
    {
        private string config = Constant.PRDDBContext;
        private SYSEVENTDBCollection syseventdbcollection;
        public IEnumerable<TURNKEY_SYSEVENT_LOG> Get(TURNKEY_SYSEVENT_LOG syseventdb)
        {
            syseventdbcollection = new SYSEVENTDBCollection(config);
            syseventdbcollection.Get(syseventdb);
            return syseventdbcollection.SYSEVENTDBList;
        }

        public string GetSqlString()
        {
            return syseventdbcollection.GetSqlString();
        }
    }
}