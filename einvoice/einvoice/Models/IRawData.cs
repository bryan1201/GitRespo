using einvoice.Models.eInvoiceMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace einvoice.Models
{
    public interface IRawData
    {
        void SaveRawData(A0101 Invoice, string filename);
    }
}
