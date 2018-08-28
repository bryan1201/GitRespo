using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using B2BService.Models;
using System.Web.Script.Serialization;
using System.Text;

namespace B2BService.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MonitorDB(string piServer, DateTime? cdtFrom, DateTime? cdtEnd)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            ViewBag.piServer = piServer;

            if (Constant.IsWebPerfLogEnabled == true)
                LogDBStatus(piServer);

            MonitorDB db = new Models.MonitorDB(piServer);
            return View(db);
        }

        public ActionResult MonitorDBApi(string piServer, DateTime? cdtFrom, DateTime? cdtEnd)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            ViewBag.piServer = piServer;
            MonitorDB db = new Models.MonitorDB(piServer);
            
            return Json(db, JsonRequestBehavior.AllowGet);
        }

        public string MonitorDBApiString(string piServer, DateTime? cdtFrom, DateTime? cdtEnd)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            ViewBag.piServer = piServer;
            MonitorDB db = new Models.MonitorDB(piServer);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(db);

            return json;
        }

        public string GetChartData(string piServer)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            IStatistic st = DataAccess.CreateStatistic(piServer);
            string json = st.ChartData();
            return json;
        }

        public void LogDBStatus(string piServer)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            IStatistic st = DataAccess.CreateStatistic(piServer);
            st.LogSessionDB();
            st.LogTotalSessionDB();
            st.LogProcessDB();
            st.LogTotalProcessDB();
            AlertMail(st);
        }

        public void AlertMail(IStatistic st)
        {
            bool IsMailTest = Constant.IsMailTest;
            bool IsMailEnabled = Constant.IsMailEnabled;
            if (IsMailEnabled == false)
                return;

            decimal avgSession = Math.Round(st.TotalStatisticSession().Percent, 0);
            decimal avgProcess = Math.Round(st.TotalStatisticProcess().Percent, 0);
            decimal ParameterSession = Math.Round(st.GetParameterSession(),0);
            decimal ParameterProcess = Math.Round(st.GetParameterProcess(),0);
            DateTime eventDT = st.TotalStatisticSession().Cdt;

            if (avgProcess >= 80 || avgSession >= 80 || IsMailTest)
            {
                StringBuilder subject = new StringBuilder();
                subject.Append("Alert PI ORACLE DB Performance (over 80%)- ");
                subject.Append("Session: "); subject.Append(avgSession.ToString()); subject.Append("% , ");
                subject.Append("Process: "); subject.Append(avgProcess.ToString()); subject.Append("%");

                StringBuilder content = new StringBuilder();

                content.Append(@"<h3>PIP ORACLE Database performance averaged over the last five minutes more than 80%: </h3>");
                content.Append(@"<p>event: " + eventDT.ToString("yyyy/MM/dd HH:mm:ss") + "</p>");
                content.Append(@"<p>Average usage Percentage of Database:");
                content.Append(@"<ul>");
                content.Append(@"<li>Session: ");
                content.Append(avgSession.ToString());
                content.Append(@" % </li>");
                content.Append(@"<li>Process: ");
                content.Append(avgProcess.ToString());
                content.Append(@" % </li>");
                content.Append(@"</ul>");
                content.Append(@"</p>");
                content.Append(@"<p>");
                content.Append(@"Please refer to the following url for the detail: <br />");
                content.Append(@"<a href='" + Constant.B2BDBPerfURL + "'>B2B Service/Database Performance Monitoring</a>");
                content.Append(@"</p>");

                try
                {
                    Method.SendMail(subject.ToString(), content.ToString());
                }
                catch
                {
                    // do nothing;
                }
            }
            
        }
    }
}