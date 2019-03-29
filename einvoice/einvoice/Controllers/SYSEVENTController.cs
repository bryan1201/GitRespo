using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using einvoice.Models;

namespace einvoice.Controllers
{
    public class SYSEVENTController : Controller
    {
        // GET: SYSEVENT
        public ActionResult Index(string eInvServer, string PARTY_ID, string SEQNO, string INFORMATION1, string MESSAGE6,
       DateTime? CdtFrom, DateTime? CdtEnd)
        {
            try
            {
                eInvServer = (eInvServer == null) ? Constant.DEVServer : eInvServer;
                TURNKEY_SYSEVENT_LOG tml = new Models.TURNKEY_SYSEVENT_LOG();
                tml.SEQNO = SEQNO;
                tml.PARTY_ID = PARTY_ID;
                tml.INFORMATION1 = INFORMATION1;
                tml.MESSAGE6 = MESSAGE6;
                tml.SetCreateDateFrom(CdtFrom);
                tml.SetCreateDateEnd(CdtEnd);
                ISYSEVENTDBCollection isyseventdbcollection = DataAccess.CreateSYSEVENTDBCollection(eInvServer);
                IEnumerable<TURNKEY_SYSEVENT_LOG> result = isyseventdbcollection.Get(tml);
                ViewBag.eInvServer = eInvServer;
                ViewBag.SqlString = isyseventdbcollection.GetSqlString();
                return View(result);
            }
            catch (Exception ex)
            {
                string rslt = ex.Message;
                return View();
            }
        }
    }
}