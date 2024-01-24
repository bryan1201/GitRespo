using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace B2BService.Models
{
    public class B2BServer
    {
    }

    public class PIDServerRawData: IRawData
    {
        public RawData Get()
        {
            RawData r = new RawData();
            return r;
        }

        public RawData Get(string messageid, string contenttype)
        {
            string url = Constant.PIDUrl;
            RawData r = new RawData(messageid, url, contenttype);
            return r;
        }
    }

    public class PIQServerRawData : IRawData
    {
        public RawData Get()
        {
            RawData r = new RawData();
            return r;
        }

        public RawData Get(string messageid, string contenttype)
        {
            string url = Constant.PIQUrl;
            RawData r = new RawData(messageid, url, contenttype);
            return r;
        }
    }

    public class PIPServerRawData : IRawData
    {
        private readonly string _url = Constant.PIPUrl;
        public RawData Get()
        {
            RawData r = new RawData();
            return r;
        }

        public RawData Get(string messageid, string contenttype)
        {
            string url = _url;
            RawData r = new RawData(messageid, url, contenttype);
            return r;
        }
    }
    public class PODServerRawData : IRawData
    {
        private readonly string _url = Constant.PODUrl;
        public RawData Get()
        {
            RawData r = new RawData();
            return r;
        }

        public RawData Get(string messageid, string contenttype)
        {
            string url = _url;
            RawData r = new RawData(messageid, url, contenttype);
            return r;
        }
    }
    public class POQServerRawData : IRawData
    {
        private readonly string _url = Constant.POQUrl;
        public RawData Get()
        {
            RawData r = new RawData();
            return r;
        }

        public RawData Get(string messageid, string contenttype)
        {
            string url = _url;
            RawData r = new RawData(messageid, url, contenttype);
            return r;
        }
    }
    public class POPServerRawData : IRawData
    {
        private readonly string _url = Constant.POPUrl;
        public RawData Get()
        {
            RawData r = new RawData();
            return r;
        }

        public RawData Get(string messageid, string contenttype)
        {
            string url = _url;
            RawData r = new RawData(messageid, url, contenttype);
            return r;
        }
    }
    public class PIDServerMDN : IMDN
    {
        private readonly string _url = Constant.PIDUrl;
        public MDN Get()
        {
            MDN r = new MDN();
            return r;
        }

        public MDN Get(string messageid, string contenttype)
        {
            string url = _url;
            MDN r = new MDN(messageid, url, contenttype);
            return r;
        }
    }

    public class PIQServerMDN : IMDN
    {
        private readonly string _url = Constant.PIQUrl;
        public MDN Get()
        {
            MDN r = new MDN();
            return r;
        }

        public MDN Get(string messageid, string contenttype)
        {
            string url = _url;
            MDN r = new MDN(messageid, url, contenttype);
            return r;
        }
    }


    public class PIPServerMDN : IMDN
    {
        private readonly string _url = Constant.PIPUrl;
        public MDN Get()
        {
            MDN r = new MDN();
            return r;
        }

        public MDN Get(string messageid, string contenttype)
        {
            string url = _url;
            MDN r = new MDN(messageid, url, contenttype);
            return r;
        }
    }

    public class PODServerMDN : IMDN
    {
        private readonly string _url = Constant.PODUrl;
        public MDN Get()
        {
            MDN r = new MDN();
            return r;
        }

        public MDN Get(string messageid, string contenttype)
        {
            string url = _url;
            MDN r = new MDN(messageid, url, contenttype);
            return r;
        }
    }
    public class POQServerMDN : IMDN
    {
        private readonly string _url = Constant.POQUrl;
        public MDN Get()
        {
            MDN r = new MDN();
            return r;
        }

        public MDN Get(string messageid, string contenttype)
        {
            string url = _url;
            MDN r = new MDN(messageid, url, contenttype);
            return r;
        }
    }
    public class POPServerMDN : IMDN
    {
        private readonly string _url = Constant.POPUrl;
        public MDN Get()
        {
            MDN r = new MDN();
            return r;
        }

        public MDN Get(string messageid, string contenttype)
        {
            string url = _url;
            MDN r = new MDN(messageid, url, contenttype);
            return r;
        }
    }

    public class PIDServerAuditLog : IAuditLog
    {
        private readonly string _url = Constant.PIDUrl;
        public AuditLog Get()
        {
            AuditLog r = new AuditLog();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog Get(string messageid, string contenttype)
        {
            string url = _url;
            AuditLog r = new AuditLog(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2()
        {
            AuditLog2 r = new AuditLog2();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2(string messageid, string contenttype)
        {
            string url = Constant.PIDUrl;
            AuditLog2 r = new AuditLog2(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }
    }

    public class PIQServerAuditLog : IAuditLog
    {
        private readonly string _url = Constant.PIQUrl;
        public AuditLog Get()
        {
            AuditLog r = new AuditLog();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog Get(string messageid, string contenttype)
        {
            string url = _url;
            AuditLog r = new AuditLog(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2()
        {
            AuditLog2 r = new AuditLog2();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2(string messageid, string contenttype)
        {
            string url = Constant.PIQUrl;
            AuditLog2 r = new AuditLog2(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }
    }

    public class PIPServerAuditLog : IAuditLog
    {
        private readonly string _url = Constant.PIPUrl;
        public AuditLog Get()
        {
            AuditLog r = new AuditLog();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog Get(string messageid, string contenttype)
        {
            string url = _url;
            AuditLog r = new AuditLog(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2()
        {
            AuditLog2 r = new AuditLog2();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2(string messageid, string contenttype)
        {
            string url = Constant.PIPUrl;
            AuditLog2 r = new AuditLog2(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }
    }
    public class PODServerAuditLog : IAuditLog
    {
        private readonly string _url = Constant.PODUrl;
        public AuditLog Get()
        {
            AuditLog r = new AuditLog();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog Get(string messageid, string contenttype)
        {
            string url = _url;
            AuditLog r = new AuditLog(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2()
        {
            AuditLog2 r = new AuditLog2();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2(string messageid, string contenttype)
        {
            string url = _url;
            AuditLog2 r = new AuditLog2(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }
    }
    public class POQServerAuditLog : IAuditLog
    {
        private readonly string _url = Constant.POQUrl;
        public AuditLog Get()
        {
            AuditLog r = new AuditLog();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog Get(string messageid, string contenttype)
        {
            string url = _url;
            AuditLog r = new AuditLog(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2()
        {
            AuditLog2 r = new AuditLog2();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2(string messageid, string contenttype)
        {
            string url = _url;
            AuditLog2 r = new AuditLog2(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }
    }
    public class POPServerAuditLog : IAuditLog
    {
        private readonly string _url = Constant.POPUrl;
        public AuditLog Get()
        {
            AuditLog r = new AuditLog();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog Get(string messageid, string contenttype)
        {
            string url = _url;
            AuditLog r = new AuditLog(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog)JsonConvert.DeserializeObject<AuditLog>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2()
        {
            AuditLog2 r = new AuditLog2();
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }

        public AuditLog2 Get2(string messageid, string contenttype)
        {
            string url = _url;
            AuditLog2 r = new AuditLog2(messageid, url, contenttype);
            string response = r.Get();
            var jdo = JsonConvert.DeserializeObject(response);
            r = (AuditLog2)JsonConvert.DeserializeObject<AuditLog2>(jdo.ToString());
            return r;
        }
    }
    public class PIDServerMTDBCollection : IMTDBCollection
    {
        private readonly string config = Constant.PIDConnStr;
        private MTDBCollection mtdbcollection;
        public IEnumerable<VMTDB> Get(MT_DB mtdb)
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

    public class PIQServerMTDBCollection : IMTDBCollection
    {
        private readonly string config = Constant.PIQConnStr;
        private MTDBCollection mtdbcollection;
        public IEnumerable<VMTDB> Get(MT_DB mtdb)
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

    public class PIPServerMTDBCollection : IMTDBCollection
    {
        private readonly string config = Constant.PIPConnStr;
        private MTDBCollection mtdbcollection;
        public IEnumerable<VMTDB> Get(MT_DB mtdb)
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

    public class POPServerMTDBCollection : IMTDBCollection
    {
        private readonly string config = Constant.POPConnStr;
        private MTDBCollection mtdbcollection;
        public IEnumerable<VMTDB> Get(MT_DB mtdb)
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

    public class POQServerMTDBCollection : IMTDBCollection
    {
        private readonly string config = Constant.POQConnStr;
        private MTDBCollection mtdbcollection;
        public IEnumerable<VMTDB> Get(MT_DB mtdb)
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

    public class PODServerMTDBCollection : IMTDBCollection
    {
        private readonly string config = Constant.PODConnStr;
        private MTDBCollection mtdbcollection;
        public IEnumerable<VMTDB> Get(MT_DB mtdb)
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

    public class PIDServerMTREFDBCollection : IMTREFDBCollection
    {
        private readonly string config = Constant.PIDConnStr;
        private MTREFDBCollection mtrefdbcollection;
        public IEnumerable<VMTREFDB> Get(VMTREFDB vmtrefdb)
        {
            mtrefdbcollection = new MTREFDBCollection(config);
            mtrefdbcollection.Get(vmtrefdb);
            return mtrefdbcollection.MTREFDBList;
        }

        public string GetSqlString()
        {
            return mtrefdbcollection.GetSqlString();
        }
    }

    public class PIQServerMTREFDBCollection : IMTREFDBCollection
    {
        private readonly string config = Constant.PIQConnStr;
        private MTREFDBCollection mtrefdbcollection;
        public IEnumerable<VMTREFDB> Get(VMTREFDB vmtrefdb)
        {
            mtrefdbcollection = new MTREFDBCollection(config);
            mtrefdbcollection.Get(vmtrefdb);
            return mtrefdbcollection.MTREFDBList;
        }

        public string GetSqlString()
        {
            return mtrefdbcollection.GetSqlString();
        }
    }

    public class PIPServerMTREFDBCollection: IMTREFDBCollection
    {
        private readonly string config = Constant.PIPConnStr;
        private MTREFDBCollection mtrefdbcollection;
        public IEnumerable<VMTREFDB> Get(VMTREFDB vmtrefdb)
        {
            mtrefdbcollection = new MTREFDBCollection(config);
            mtrefdbcollection.Get(vmtrefdb);
            return mtrefdbcollection.MTREFDBList;
        }

        public string GetSqlString()
        {
            return mtrefdbcollection.GetSqlString();
        }
    }

    public class POPServerMTREFDBCollection : IMTREFDBCollection
    {
        private readonly string config = Constant.POPConnStr;
        private MTREFDBCollection mtrefdbcollection;
        public IEnumerable<VMTREFDB> Get(VMTREFDB vmtrefdb)
        {
            mtrefdbcollection = new MTREFDBCollection(config);
            mtrefdbcollection.Get(vmtrefdb);
            return mtrefdbcollection.MTREFDBList;
        }

        public string GetSqlString()
        {
            return mtrefdbcollection.GetSqlString();
        }
    }

    public class POQServerMTREFDBCollection : IMTREFDBCollection
    {
        private readonly string config = Constant.POQConnStr;
        private MTREFDBCollection mtrefdbcollection;
        public IEnumerable<VMTREFDB> Get(VMTREFDB vmtrefdb)
        {
            mtrefdbcollection = new MTREFDBCollection(config);
            mtrefdbcollection.Get(vmtrefdb);
            return mtrefdbcollection.MTREFDBList;
        }

        public string GetSqlString()
        {
            return mtrefdbcollection.GetSqlString();
        }
    }

    public class PODServerMTREFDBCollection : IMTREFDBCollection
    {
        private readonly string config = Constant.PODConnStr;
        private MTREFDBCollection mtrefdbcollection;
        public IEnumerable<VMTREFDB> Get(VMTREFDB vmtrefdb)
        {
            mtrefdbcollection = new MTREFDBCollection(config);
            mtrefdbcollection.Get(vmtrefdb);
            return mtrefdbcollection.MTREFDBList;
        }

        public string GetSqlString()
        {
            return mtrefdbcollection.GetSqlString();
        }
    }
    public class PIDServerLOOKUPDBCollection : ILOOKUPDBCollection
    {
        private readonly string config = Constant.PIDConnStr;
        private LOOKUPDBCollection lookupdbcollection;
        public IEnumerable<LOOKUP_DB> Get(LOOKUP_DB db)
        {
            lookupdbcollection = new LOOKUPDBCollection(config);
            lookupdbcollection.Get(db);
            return lookupdbcollection.LOOKUPDBList;
        }
    }

    public class PIQServerLOOKUPDBCollection : ILOOKUPDBCollection
    {
        private readonly string config = Constant.PIQConnStr;
        private LOOKUPDBCollection lookupdbcollection;
        public IEnumerable<LOOKUP_DB> Get(LOOKUP_DB db)
        {
            lookupdbcollection = new LOOKUPDBCollection(config);
            lookupdbcollection.Get(db);
            return lookupdbcollection.LOOKUPDBList;
        }
    }

    public class PIPServerLOOKUPDBCollection : ILOOKUPDBCollection
    {
        private readonly string config = Constant.PIPConnStr;
        private LOOKUPDBCollection lookupdbcollection;
        public IEnumerable<LOOKUP_DB> Get(LOOKUP_DB db)
        {
            lookupdbcollection = new LOOKUPDBCollection(config);
            lookupdbcollection.Get(db);
            return lookupdbcollection.LOOKUPDBList;
        }
    }

    public class PODServerLOOKUPDBCollection : ILOOKUPDBCollection
    {
        private readonly string config = Constant.PODConnStr;
        private LOOKUPDBCollection lookupdbcollection;
        public IEnumerable<LOOKUP_DB> Get(LOOKUP_DB db)
        {
            lookupdbcollection = new LOOKUPDBCollection(config);
            lookupdbcollection.Get(db);
            return lookupdbcollection.LOOKUPDBList;
        }
    }

    public class POQServerLOOKUPDBCollection : ILOOKUPDBCollection
    {
        private readonly string config = Constant.POQConnStr;
        private LOOKUPDBCollection lookupdbcollection;
        public IEnumerable<LOOKUP_DB> Get(LOOKUP_DB db)
        {
            lookupdbcollection = new LOOKUPDBCollection(config);
            lookupdbcollection.Get(db);
            return lookupdbcollection.LOOKUPDBList;
        }
    }

    public class POPServerLOOKUPDBCollection : ILOOKUPDBCollection
    {
        private readonly string config = Constant.POPConnStr;
        private LOOKUPDBCollection lookupdbcollection;
        public IEnumerable<LOOKUP_DB> Get(LOOKUP_DB db)
        {
            lookupdbcollection = new LOOKUPDBCollection(config);
            lookupdbcollection.Get(db);
            return lookupdbcollection.LOOKUPDBList;
        }
    }
    public class PIPServerMTREFDB : IMTRef
    {
        private readonly string config = Constant.PIPConnStr;
        private MT_REF_DB mtrefdb;
        object IMTRef.GetDIVISION(ServiceType type, string partner)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetDIVISION(type, partner);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetEDIMSGTYPE(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetEDIMSGTYPE(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetGSSENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetGSSENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISARECEIVERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                object results = mtrefdb.GetISARECEIVERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISASENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetISASENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetPARTNER(ServiceType type)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);

                var results = mtrefdb.GetPARTNER(type);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetREGION(ServiceType type, string partner, string division)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetREGION(type, partner, division);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
    public class PIQServerMTREFDB : IMTRef
    {
        private readonly string config = Constant.PIQConnStr;
        private MT_REF_DB mtrefdb;
        object IMTRef.GetDIVISION(ServiceType type, string partner)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetDIVISION(type, partner);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetEDIMSGTYPE(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetEDIMSGTYPE(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetGSSENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetGSSENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISARECEIVERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                object results = mtrefdb.GetISARECEIVERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISASENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetISASENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetPARTNER(ServiceType type)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                
                var results = mtrefdb.GetPARTNER(type);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetREGION(ServiceType type, string partner, string division)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetREGION(type, partner, division);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
    public class PIDServerMTREFDB : IMTRef
    {
        private readonly string config = Constant.PIDConnStr;
        private MT_REF_DB mtrefdb;
        object IMTRef.GetDIVISION(ServiceType type, string partner)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetDIVISION(type, partner);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetEDIMSGTYPE(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetEDIMSGTYPE(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetGSSENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetGSSENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISARECEIVERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                object results = mtrefdb.GetISARECEIVERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISASENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetISASENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetPARTNER(ServiceType type)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);

                var results = mtrefdb.GetPARTNER(type);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetREGION(ServiceType type, string partner, string division)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetREGION(type, partner, division);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
    public class POPServerMTREFDB : IMTRef
    {
        private readonly string config = Constant.POPConnStr;
        private MT_REF_DB mtrefdb;
        object IMTRef.GetDIVISION(ServiceType type, string partner)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetDIVISION(type, partner);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetEDIMSGTYPE(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetEDIMSGTYPE(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetGSSENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetGSSENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISARECEIVERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                object results = mtrefdb.GetISARECEIVERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISASENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetISASENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetPARTNER(ServiceType type)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);

                var results = mtrefdb.GetPARTNER(type);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetREGION(ServiceType type, string partner, string division)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetREGION(type, partner, division);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
    public class POQServerMTREFDB : IMTRef
    {
        private readonly string config = Constant.POQConnStr;
        private MT_REF_DB mtrefdb;
        object IMTRef.GetDIVISION(ServiceType type, string partner)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetDIVISION(type, partner);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetEDIMSGTYPE(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetEDIMSGTYPE(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetGSSENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetGSSENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISARECEIVERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                object results = mtrefdb.GetISARECEIVERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISASENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetISASENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetPARTNER(ServiceType type)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);

                var results = mtrefdb.GetPARTNER(type);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetREGION(ServiceType type, string partner, string division)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetREGION(type, partner, division);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
    public class PODServerMTREFDB : IMTRef
    {
        private readonly string config = Constant.PODConnStr;
        private MT_REF_DB mtrefdb;
        object IMTRef.GetDIVISION(ServiceType type, string partner)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetDIVISION(type, partner);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetEDIMSGTYPE(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetEDIMSGTYPE(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetGSSENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetGSSENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISARECEIVERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                object results = mtrefdb.GetISARECEIVERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetISASENDERID(ServiceType type, string partner, string division, string region)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetISASENDERID(type, partner, division, region);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetPARTNER(ServiceType type)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);

                var results = mtrefdb.GetPARTNER(type);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        object IMTRef.GetREGION(ServiceType type, string partner, string division)
        {
            try
            {
                mtrefdb = new MT_REF_DB(config);
                var results = mtrefdb.GetREGION(type, partner, division);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
    public class PIQServerPROCESSDBCollection : IPROCESSDBCollection
    {
        private readonly string config = Constant.PIQConnStr;
        private PROCESSDBCollection dbcollection;
        public IEnumerable<vProcessDB> Get(PROCESS_DB db)
        {
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }

        public IEnumerable<vProcessDB> Get(string msgid)
        {
            PROCESS_DB db = new PROCESS_DB();
            db.MSGID = msgid;
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }
    }

    public class PIDServerPROCESSDBCollection : IPROCESSDBCollection
    {
        private readonly string config = Constant.PIDConnStr;
        private PROCESSDBCollection dbcollection;
        public IEnumerable<vProcessDB> Get(PROCESS_DB db)
        {
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }

        public IEnumerable<vProcessDB> Get(string msgid)
        {
            PROCESS_DB db = new PROCESS_DB();
            db.MSGID = msgid;
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }
    }

    public class PIPServerPROCESSDBCollection : IPROCESSDBCollection
    {
        private readonly string config = Constant.PIPConnStr;
        private PROCESSDBCollection dbcollection;
        public IEnumerable<vProcessDB> Get(PROCESS_DB db)
        {
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }

        public IEnumerable<vProcessDB> Get(string msgid)
        {
            PROCESS_DB db = new PROCESS_DB();
            db.MSGID = msgid;
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }
    }

    public class POPServerPROCESSDBCollection : IPROCESSDBCollection
    {
        private readonly string config = Constant.POPConnStr;
        private PROCESSDBCollection dbcollection;
        public IEnumerable<vProcessDB> Get(PROCESS_DB db)
        {
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }

        public IEnumerable<vProcessDB> Get(string msgid)
        {
            PROCESS_DB db = new PROCESS_DB();
            db.MSGID = msgid;
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }
    }
    public class POQServerPROCESSDBCollection : IPROCESSDBCollection
    {
        private readonly string config = Constant.POQConnStr;
        private PROCESSDBCollection dbcollection;
        public IEnumerable<vProcessDB> Get(PROCESS_DB db)
        {
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }

        public IEnumerable<vProcessDB> Get(string msgid)
        {
            PROCESS_DB db = new PROCESS_DB();
            db.MSGID = msgid;
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }
    }
    public class PODServerPROCESSDBCollection : IPROCESSDBCollection
    {
        private readonly string config = Constant.PODConnStr;
        private PROCESSDBCollection dbcollection;
        public IEnumerable<vProcessDB> Get(PROCESS_DB db)
        {
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }

        public IEnumerable<vProcessDB> Get(string msgid)
        {
            PROCESS_DB db = new PROCESS_DB();
            db.MSGID = msgid;
            dbcollection = new PROCESSDBCollection(config);
            dbcollection.Get(db);
            return dbcollection.PROCESSDBList;
        }
    }

    public class PIDServerStatistic : IStatistic
    {
        private readonly string config = Constant.PIDConnStr;
        private Statistic dbStatistic;

        public PIDServerStatistic()
        {
            dbStatistic = new Statistic(config);
        }

        public void Init()
        {
            dbStatistic.Init();
        }

        public string ChartData()
        {
            return dbStatistic.ChartData();
        }

        public void LogTotalSessionDB()
        {
            dbStatistic.LogTotalSessionDB();
        }

        public void LogTotalProcessDB()
        {
            dbStatistic.LogTotalProcessDB();
        }

        public void LogSessionDB()
        {
            dbStatistic.LogSessionDB();
        }

        public void LogProcessDB()
        {
            dbStatistic.LogProcessDB();
        }

        public DBSession TotalStatisticSession()
        {
            return dbStatistic.TotalSession;
        }

        public DBProcess TotalStatisticProcess()
        {
            return dbStatistic.TotalProcess;
        }

        public IEnumerable<DBSession> StatisticSession()
        {
            return dbStatistic.CurrentSession;
        }

        public IEnumerable<DBProcess> StatisticProcess()
        {
            return dbStatistic.CurrentProcess;
        }

        public IEnumerable<DBParameter> GetParameter()
        {
            return dbStatistic.CurrentParameter;
        }

        public decimal GetParameterSession()
        {
            return dbStatistic.ParameterSessionValue;
        }

        public decimal GetParameterProcess()
        {
            return dbStatistic.ParameterProcessValue;
        }

        public IEnumerable<MONITOR_DB> QueryMONITOR_DB()
        {
            return dbStatistic.CurrentMONITOR_DB;
        }

        public decimal GetMaxSessionValue()
        {
            return dbStatistic.MaxSessionValue;
        }

        public decimal GetMaxProcessValue()
        {
            return dbStatistic.MaxProcessValue;
        }
    }

    public class PIQServerStatistic : IStatistic
    {
        private readonly string config = Constant.PIQConnStr;
        private Statistic dbStatistic;

        public PIQServerStatistic()
        {
            dbStatistic = new Statistic(config);
        }

        public void Init()
        {
            dbStatistic.Init();
        }

        public string ChartData()
        {
            return dbStatistic.ChartData();
        }

        public void LogTotalSessionDB()
        {
            dbStatistic.LogTotalSessionDB();
        }

        public void LogTotalProcessDB()
        {
            dbStatistic.LogTotalProcessDB();
        }

        public void LogSessionDB()
        {
            dbStatistic.LogSessionDB();
        }

        public void LogProcessDB()
        {
            dbStatistic.LogProcessDB();
        }

        public DBSession TotalStatisticSession()
        {
            return dbStatistic.TotalSession;
        }

        public DBProcess TotalStatisticProcess()
        {
            return dbStatistic.TotalProcess;
        }

        public IEnumerable<DBSession> StatisticSession()
        {
            return dbStatistic.CurrentSession;
        }

        public IEnumerable<DBProcess> StatisticProcess()
        {
            return dbStatistic.CurrentProcess;
        }

        public IEnumerable<DBParameter> GetParameter()
        {
            return dbStatistic.CurrentParameter;
        }

        public decimal GetParameterSession()
        {
            return dbStatistic.ParameterSessionValue;
        }

        public decimal GetParameterProcess()
        {
            return dbStatistic.ParameterProcessValue;
        }

        public IEnumerable<MONITOR_DB> QueryMONITOR_DB()
        {
            return dbStatistic.CurrentMONITOR_DB;
        }

        public decimal GetMaxSessionValue()
        {
            return dbStatistic.MaxSessionValue;
        }

        public decimal GetMaxProcessValue()
        {
            return dbStatistic.MaxProcessValue;
        }
    }

    public class PIPServerStatistic : IStatistic
    {
        private readonly string config = Constant.PIPConnStr;
        private Statistic dbStatistic;

        public PIPServerStatistic()
        {
            dbStatistic = new Statistic(config);
        }

        public void Init()
        {
            dbStatistic.Init();
        }

        public string ChartData()
        {
            return dbStatistic.ChartData();
        }

        public void LogTotalSessionDB()
        {
            dbStatistic.LogTotalSessionDB();
        }

        public void LogTotalProcessDB()
        {
            dbStatistic.LogTotalProcessDB();
        }

        public void LogSessionDB()
        {
            dbStatistic.LogSessionDB();
        }

        public void LogProcessDB()
        {
            dbStatistic.LogProcessDB();
        }

        public DBSession TotalStatisticSession()
        {
            return dbStatistic.TotalSession;
        }

        public DBProcess TotalStatisticProcess()
        {
            return dbStatistic.TotalProcess;
        }

        public IEnumerable<DBSession> StatisticSession()
        {
            return dbStatistic.CurrentSession;
        }

        public IEnumerable<DBProcess> StatisticProcess()
        {
            return dbStatistic.CurrentProcess;
        }

        public IEnumerable<DBParameter> GetParameter()
        {
            return dbStatistic.CurrentParameter;
        }

        public decimal GetParameterSession()
        {
            return dbStatistic.ParameterSessionValue;
        }

        public decimal GetParameterProcess()
        {
            return dbStatistic.ParameterProcessValue;
        }

        public IEnumerable<MONITOR_DB> QueryMONITOR_DB()
        {
            return dbStatistic.QueryMONITOR_DB();
        }

        public decimal GetMaxSessionValue()
        {
            return dbStatistic.MaxSessionValue;
        }

        public decimal GetMaxProcessValue()
        {
            return dbStatistic.MaxProcessValue;
        }
    }

    public class POPServerStatistic : IStatistic
    {
        private readonly string config = Constant.POPConnStr;
        private Statistic dbStatistic;

        public POPServerStatistic()
        {
            dbStatistic = new Statistic(config);
        }

        public void Init()
        {
            dbStatistic.Init();
        }

        public string ChartData()
        {
            return dbStatistic.ChartData();
        }

        public void LogTotalSessionDB()
        {
            dbStatistic.LogTotalSessionDB();
        }

        public void LogTotalProcessDB()
        {
            dbStatistic.LogTotalProcessDB();
        }

        public void LogSessionDB()
        {
            dbStatistic.LogSessionDB();
        }

        public void LogProcessDB()
        {
            dbStatistic.LogProcessDB();
        }

        public DBSession TotalStatisticSession()
        {
            return dbStatistic.TotalSession;
        }

        public DBProcess TotalStatisticProcess()
        {
            return dbStatistic.TotalProcess;
        }

        public IEnumerable<DBSession> StatisticSession()
        {
            return dbStatistic.CurrentSession;
        }

        public IEnumerable<DBProcess> StatisticProcess()
        {
            return dbStatistic.CurrentProcess;
        }

        public IEnumerable<DBParameter> GetParameter()
        {
            return dbStatistic.CurrentParameter;
        }

        public decimal GetParameterSession()
        {
            return dbStatistic.ParameterSessionValue;
        }

        public decimal GetParameterProcess()
        {
            return dbStatistic.ParameterProcessValue;
        }

        public IEnumerable<MONITOR_DB> QueryMONITOR_DB()
        {
            return dbStatistic.QueryMONITOR_DB();
        }

        public decimal GetMaxSessionValue()
        {
            return dbStatistic.MaxSessionValue;
        }

        public decimal GetMaxProcessValue()
        {
            return dbStatistic.MaxProcessValue;
        }
    }
    public class POQServerStatistic : IStatistic
    {
        private readonly string config = Constant.POQConnStr;
        private Statistic dbStatistic;

        public POQServerStatistic()
        {
            dbStatistic = new Statistic(config);
        }

        public void Init()
        {
            dbStatistic.Init();
        }

        public string ChartData()
        {
            return dbStatistic.ChartData();
        }

        public void LogTotalSessionDB()
        {
            dbStatistic.LogTotalSessionDB();
        }

        public void LogTotalProcessDB()
        {
            dbStatistic.LogTotalProcessDB();
        }

        public void LogSessionDB()
        {
            dbStatistic.LogSessionDB();
        }

        public void LogProcessDB()
        {
            dbStatistic.LogProcessDB();
        }

        public DBSession TotalStatisticSession()
        {
            return dbStatistic.TotalSession;
        }

        public DBProcess TotalStatisticProcess()
        {
            return dbStatistic.TotalProcess;
        }

        public IEnumerable<DBSession> StatisticSession()
        {
            return dbStatistic.CurrentSession;
        }

        public IEnumerable<DBProcess> StatisticProcess()
        {
            return dbStatistic.CurrentProcess;
        }

        public IEnumerable<DBParameter> GetParameter()
        {
            return dbStatistic.CurrentParameter;
        }

        public decimal GetParameterSession()
        {
            return dbStatistic.ParameterSessionValue;
        }

        public decimal GetParameterProcess()
        {
            return dbStatistic.ParameterProcessValue;
        }

        public IEnumerable<MONITOR_DB> QueryMONITOR_DB()
        {
            return dbStatistic.QueryMONITOR_DB();
        }

        public decimal GetMaxSessionValue()
        {
            return dbStatistic.MaxSessionValue;
        }

        public decimal GetMaxProcessValue()
        {
            return dbStatistic.MaxProcessValue;
        }
    }
    public class PODServerStatistic : IStatistic
    {
        private readonly string config = Constant.PODConnStr;
        private Statistic dbStatistic;

        public PODServerStatistic()
        {
            dbStatistic = new Statistic(config);
        }

        public void Init()
        {
            dbStatistic.Init();
        }

        public string ChartData()
        {
            return dbStatistic.ChartData();
        }

        public void LogTotalSessionDB()
        {
            dbStatistic.LogTotalSessionDB();
        }

        public void LogTotalProcessDB()
        {
            dbStatistic.LogTotalProcessDB();
        }

        public void LogSessionDB()
        {
            dbStatistic.LogSessionDB();
        }

        public void LogProcessDB()
        {
            dbStatistic.LogProcessDB();
        }

        public DBSession TotalStatisticSession()
        {
            return dbStatistic.TotalSession;
        }

        public DBProcess TotalStatisticProcess()
        {
            return dbStatistic.TotalProcess;
        }

        public IEnumerable<DBSession> StatisticSession()
        {
            return dbStatistic.CurrentSession;
        }

        public IEnumerable<DBProcess> StatisticProcess()
        {
            return dbStatistic.CurrentProcess;
        }

        public IEnumerable<DBParameter> GetParameter()
        {
            return dbStatistic.CurrentParameter;
        }

        public decimal GetParameterSession()
        {
            return dbStatistic.ParameterSessionValue;
        }

        public decimal GetParameterProcess()
        {
            return dbStatistic.ParameterProcessValue;
        }

        public IEnumerable<MONITOR_DB> QueryMONITOR_DB()
        {
            return dbStatistic.QueryMONITOR_DB();
        }

        public decimal GetMaxSessionValue()
        {
            return dbStatistic.MaxSessionValue;
        }

        public decimal GetMaxProcessValue()
        {
            return dbStatistic.MaxProcessValue;
        }
    }
}