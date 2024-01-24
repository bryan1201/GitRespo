using einvoice.Models.eInvoiceMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace einvoice.Models
{
    public interface IRawDataCollection
    {
        string GetFtpUrl();
        string GetContent(string filepathname, string contenttype);
        void SaveRawData(A0101 Invoice, string filename);
    }
}
