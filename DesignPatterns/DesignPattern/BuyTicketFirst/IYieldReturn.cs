using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyTicketFirst
{
    interface IYieldReturn
    {
        string type { get; set; }
        double sum { get; set; }
        long memory { get; set; }
    }
}
