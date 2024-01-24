using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BService.Models
{

    interface IFactory
    {
        IRawData CreateRawData(string server);
        IMDN CreateMDN(string server);
        IAuditLog CreateAuditLog(string server);
        IMTDBCollection CreateMTDBCollection(string server);
        ILOOKUPDBCollection CreateLOOKUPDBCollection(string server);
        IPROCESSDBCollection CreatePROCESSDBCollection(string server);
        IMTREFDBCollection CreateMTREFDBCollection(string server);
        IMTRef CreateMTREFDB(string server);
        IStatistic CreateStatistic(string server);
    }
}