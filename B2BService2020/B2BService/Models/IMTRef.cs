using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BService.Models
{
    public interface IMTRef
    {
        object GetPARTNER(ServiceType Type);
        object GetDIVISION(ServiceType Type, string partner);
        object GetREGION(ServiceType Type, string partner, string division);
        object GetISASENDERID(ServiceType Type, string partner, string division, string region);
        object GetISARECEIVERID(ServiceType Type, string partner, string division, string region);
        object GetGSSENDERID(ServiceType Type, string partner, string division, string region);
        object GetEDIMSGTYPE(ServiceType Type, string partner, string division, string region);
    }
}
