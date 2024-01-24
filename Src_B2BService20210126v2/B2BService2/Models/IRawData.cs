using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BService.Models
{
    public interface IRawData
    {
        RawData Get();
        RawData Get(string messageid, string contenttype);
    }
}
