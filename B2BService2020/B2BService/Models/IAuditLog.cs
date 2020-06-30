using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BService.Models
{
    public interface IAuditLog
    {
        AuditLog Get();
        AuditLog Get(string messageid, string contenttype);
        AuditLog2 Get2();
        AuditLog2 Get2(string messageid, string contenttype);
    }
}
