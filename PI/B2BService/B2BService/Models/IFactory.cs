using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BService.Models
{

    interface IFactory
    {
        IRawData CreateRawData();
        IMDN CreateMDN();
        IAuditLog CreateAuditLog();
    }
}