using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BService.Models
{
    public interface IMDN
    {
        MDN Get();
        MDN Get(string messageid, string contenttype);
    }
}
