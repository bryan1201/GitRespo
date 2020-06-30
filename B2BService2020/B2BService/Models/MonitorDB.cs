using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Reflection;
using System.Web.Script.Serialization;

namespace B2BService.Models
{
    public class MonitorDB
    {
        public IEnumerable<DBSession> CurrentSession { get; set; }
        public IEnumerable<DBProcess> CurrentProcess { get; set; }
        public IEnumerable<DBParameter> CurrentParameter { get; set; }
        public DBSession TotalSession { get; set; }
        public DBProcess TotalProcess { get; set; }
        public decimal ParameterSessionValue { get; set; }
        public decimal ParameterProcessValue { get; set; }
        public IEnumerable<MONITOR_DB> CurrentMONITOR_DB { get; set; }
        public decimal MaxSessionValue { get; set; }
        public decimal MaxProcessValue { get; set; }
        public decimal MaxSessionPercent { get; set; }
        public decimal MaxProcessPercent { get; set; }

        public MonitorDB(string server)
        {
            IStatistic st = DataAccess.CreateStatistic(server);
            
            CurrentParameter = st.GetParameter();
            ParameterSessionValue = st.GetParameterSession();
            ParameterProcessValue = st.GetParameterProcess();
            CurrentSession = st.StatisticSession();
            CurrentProcess = st.StatisticProcess();
            TotalSession = st.TotalStatisticSession();
            TotalProcess = st.TotalStatisticProcess();

            CurrentMONITOR_DB = st.QueryMONITOR_DB();
            MaxSessionValue = st.GetMaxSessionValue();
            MaxProcessValue = st.GetMaxProcessValue();

            MaxSessionPercent = Math.Round(100 * MaxSessionValue / ParameterSessionValue, 2);
            MaxProcessPercent = Math.Round(100 * MaxProcessValue / ParameterProcessValue, 2);
        }
    }

    public class DBParameter
    {
        [Key]
        public string Name { get; set; }
        public string value { get; set; }
    }

    public abstract class absDBMonitor
    {
        [Key]
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal NameValue { get; set; }
        public decimal Percent { get; set; }
        public decimal MaxValue { get; set; }
        public decimal MaxPercent { get; set; }
        public DateTime Cdt { get; set; }
    }

    public class DBSession : absDBMonitor
    {
        private string _Type = "SESSION";
        public DBSession()
        {
            this.Type = this._Type;
        }
    }

    public class DBProcess : absDBMonitor
    {
        private string _Type = "PROCESS";
        public DBProcess()
        {
            this.Type = this._Type;
        }
    }

    public class MONITOR_DB: absDBMonitor
    {
        private string config = string.Empty;
        private decimal _maxSessionValue { get; set; }
        private decimal _maxProcessValue { get; set; }
        public MONITOR_DB(string config)
        {
            this.config = config;
        }

        public IEnumerable<MONITOR_DB> QUERY()
        {
            DateTime dtEnd = DateTime.Now;
            DateTime dtFrom = dtEnd.AddHours(-23.0);
            
            //SELECT * FROM MT_DB WHERE ROWNUM <= 100 AND CREATEDATE >= TO_DATE('2016/06/30 13:00:00', 'yyyy/MM/dd HH24:MI:SS') AND CREATEDATE <= TO_DATE('2016/07/01 13:00:00', 'yyyy/MM/dd HH24:MI:SS')  ORDER BY CREATEDATE DESC
            string sqlString = "SELECT d.* FROM MONITOR_DB d WHERE d.NAME = '@Total' AND TO_CHAR(d.DATETIME, 'yyyy/MM/dd HH24:MI:SS') >= '" + dtFrom.ToString("yyyy/MM/dd HH:mm:ss") + "'";
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, sqlString).Tables[0];
            IEnumerable<MONITOR_DB> dresult = ConvertToMONITOR_DB_Readings(dt);
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    _maxSessionValue = dresult.Where(x => x.Type == "SESSION").Max(x => x.NameValue);
                    _maxProcessValue = dresult.Where(x => x.Type == "PROCESS").Max(x => x.NameValue);
                }
                else
                {
                    _maxSessionValue = 0;
                    _maxProcessValue = 0;
                }
            return dresult;
        }

        public decimal GetMaxSessionValue()
        {
            return this._maxSessionValue;
        }

        public decimal GetMaxProcessValue()
        {
            return this._maxProcessValue;
        }

        private IEnumerable<MONITOR_DB> ConvertToMONITOR_DB_Readings(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new MONITOR_DB(this.config)
                {
                    Id = Guid.Parse(row[0].ToString()),
                    Type = Convert.ToString(row[1]),
                    Name = Convert.ToString(row[2]),
                    NameValue = Convert.ToDecimal(row[3].ToString()),
                    Percent = Convert.ToDecimal(row[4].ToString()),
                    Cdt = Convert.ToDateTime(row[5].ToString())
                };
            }
        }

        public string SqlMonitorDB(absDBMonitor item)
        {
            string sqlString = string.Empty;
            StringBuilder sb = new StringBuilder();
            string value = string.Empty;
            string sqlvalue = string.Empty;
            sqlString = "INSERT INTO MONITOR_DB(EVENTID, TYPE, NAME, NAMEVALUE, PERCENT, DATETIME) ";
            sb.Append(sqlString);
            sb.Append("VALUES(");
            value = item.Id.ToString();
            sqlvalue = "'@',"; sqlvalue = sqlvalue.Replace("@", value);
            sb.Append(sqlvalue);

            value = item.Type;
            sqlvalue = "'@',"; sqlvalue = sqlvalue.Replace("@", value);
            sb.Append(sqlvalue);

            value = item.Name;
            sqlvalue = "'@',"; sqlvalue = sqlvalue.Replace("@", value);
            sb.Append(sqlvalue);

            value = item.NameValue.ToString();
            sqlvalue = "'@',"; sqlvalue = sqlvalue.Replace("@", value);
            sb.Append(sqlvalue);

            value = item.Percent.ToString();
            sqlvalue = "'@',"; sqlvalue = sqlvalue.Replace("@", value);
            sb.Append(sqlvalue);

            value = item.Cdt.ToString("yyyy/MM/dd HH:mm:ss");
            sqlvalue = "TO_DATE('@', 'yyyy/MM/dd HH24:MI:SS') "; sqlvalue = sqlvalue.Replace("@", value);
            sb.Append(sqlvalue);

            sb.Append(")");

            sqlString = sb.ToString();
            return sqlString;
        }

        public void LogProcessDB(IEnumerable<DBProcess> CurrentProcess)
        {
            foreach (var item in CurrentProcess)
            {
                string sqlString = SqlMonitorDB(item);
                DAO.oracleCmdSP(this.config, sqlString);
            }
        }

        public void LogSessionDB(IEnumerable<DBSession> CurrentSession)
        {
            foreach (var item in CurrentSession)
            {
                string sqlString = SqlMonitorDB(item);
                DAO.oracleCmdSP(this.config, sqlString);
            }
        }

        public void LogTotalProcessDB(DBProcess TotalProcess)
        {
            string sqlString = SqlMonitorDB(TotalProcess);
            DAO.oracleCmdSP(this.config, sqlString);
        }

        public void LogTotalSessionDB(DBSession TotalSession)
        {
            string sqlString = SqlMonitorDB(TotalSession);
            DAO.oracleCmdSP(this.config, sqlString);
        }
    }

    public class DatasetMonitor
    {
        public string label { get; set; }
        public string fillColor { get; set; }
        public List<decimal> data { get; set; }
    }

    public class RootBarChart
    {
        public List<string> labels { get; set; }
        public List<DatasetMonitor> datasets { get; set; }
        private MONITOR_DB _mon;

        public RootBarChart(MONITOR_DB mon)
        {
            this._mon = mon;
            this.labels = new List<string>();
            this.datasets = new List<DatasetMonitor>();
        }

        public string JsonChartData()
        {
            var query = _mon.QUERY().ToList();
            var labels = query.OrderBy(x=>x.Cdt).Select(x => string.Format("{0:00}d", x.Cdt.Day) + string.Format(" {0:00}h",x.Cdt.Hour)).Distinct().ToList();
            var types = query.Select(x => x.Type).Distinct().ToList();
            //var groups = query.GroupBy(q => new { q.Type, q.Cdt.Hour });

            foreach (var label in labels)
            {
                this.labels.Add(label);
            }

            foreach (var type in types)
            {
                DatasetMonitor dm = new DatasetMonitor();
                dm.label = type;
                dm.fillColor = (dm.label == "SESSION") ? "#FC9775" : "#5A69A6";
                dm.data = new List<decimal>();
                var groups = query.Where(x => x.Type == type).OrderBy(x=>x.Cdt).ToList();
                var groupmondb = groups.ToList().GroupBy(q => new { q.Cdt.Hour });
                   
                foreach (var item in groupmondb)
                {
                    var peritem = Math.Round(item.Average(x => x.Percent),2);
                    dm.data.Add(peritem);
                }

                this.datasets.Add(dm);
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(this);
            return json;
        }

        private IList<string> InitTimeAxie()
        {
            IList<string> listTime = new List<string>();
            for(int i=0;i<=23;i++)
            {
                listTime.Add(i.ToString("00") + ":00");
            }

            return listTime;
        }
    }

    public class Statistic
    {
        private string config = string.Empty;
        public IEnumerable<DBSession> CurrentSession;
        public IEnumerable<DBProcess> CurrentProcess;
        public IEnumerable<DBParameter> CurrentParameter;
        public IEnumerable<MONITOR_DB> CurrentMONITOR_DB;
        public DBSession TotalSession;
        public DBProcess TotalProcess;
        public decimal ParameterSessionValue;
        public decimal ParameterProcessValue;
        public decimal MaxSessionValue;
        public decimal MaxProcessValue;
        private MONITOR_DB _monitorDB;

        public Statistic(string config) {
            this.config = config;
            Init();
        }

        public void Init()
        {
            CurrentParameter = GetParameter();
            CurrentSession = StatisticSession();
            CurrentProcess = StatisticProcess();
            TotalSession = TotalStatisticSession();
            TotalProcess = TotalStatisticProcess();
            _monitorDB = new MONITOR_DB(this.config);
            //CurrentMONITOR_DB = QueryMONITOR_DB();
        }

        public string ChartData()
        {
            RootBarChart rbchart = new RootBarChart(_monitorDB);
            return rbchart.JsonChartData();
        }

        public void LogTotalSessionDB()
        {
            TotalSession = TotalStatisticSession();
            _monitorDB.LogTotalSessionDB(TotalSession);
        }

        public void LogTotalProcessDB()
        {
            TotalProcess = TotalStatisticProcess();
            _monitorDB.LogTotalProcessDB(TotalProcess);
        }

        public void LogSessionDB()
        {
            CurrentSession = StatisticSession();
            _monitorDB.LogSessionDB(CurrentSession);
        }

        public void LogProcessDB()
        {
            CurrentProcess = StatisticProcess();
            _monitorDB.LogProcessDB(CurrentProcess);
        }

        public DBSession TotalStatisticSession()
        {
            TotalSession = CurrentSession.GroupBy(x => new { x.Type }).Select(g =>
                    new DBSession()
                    {
                        Id = g.FirstOrDefault().Id,
                        Type = g.Key.Type,
                        Name = "@Total",
                        NameValue = g.Sum(y => y.NameValue),
                        Percent = (100 * g.Sum(y => y.NameValue) / ParameterSessionValue),
                        Cdt = DateTime.Now
                    }
                ).FirstOrDefault();

            return TotalSession;
        }

        public DBProcess TotalStatisticProcess()
        {
            TotalProcess = CurrentProcess.GroupBy(x => new { x.Type }).Select(g =>
                    new DBProcess()
                    {
                        Id = g.FirstOrDefault().Id,
                        Type = g.Key.Type,
                        Name = "@Total",
                        NameValue = g.Sum(y => y.NameValue),
                        Percent = (100 * g.Sum(y => y.NameValue) / ParameterProcessValue),
                        Cdt = DateTime.Now
                    }
                ).FirstOrDefault();

            return TotalProcess;
        }

        public IEnumerable<DBSession> StatisticSession()
        {
            string sqlString = "SELECT 'SESSION' AS \"TYPE\", USERNAME AS \"NAME\", count(1) AS Cnt FROM v$session GROUP BY 'SESSION', USERNAME";
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, sqlString).Tables[0];
            IEnumerable<DBSession> dresult = ConvertToSessionReadings(dt);
            return dresult;
        }

        public IEnumerable<DBProcess> StatisticProcess()
        {
            string sqlString = "SELECT 'PROCESS' AS \"TYPE\", USERNAME AS \"NAME\", count(1) AS Cnt FROM v$process GROUP BY 'PROCESS', USERNAME";
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, sqlString).Tables[0];
            IEnumerable<DBProcess> dresult = ConvertToProcessReadings(dt);
            return dresult;
        }

        public IEnumerable<DBParameter> GetParameter()
        {
            string sqlString = "SELECT name, value FROM v$parameter WHERE name IN ('sessions','processes')";
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, sqlString).Tables[0];
            IEnumerable<DBParameter> dp = ConvertToParameterReadings(dt);
            ParameterSessionValue = Convert.ToDecimal(dp.Where(x => x.Name == "sessions").Select(x => x.value).FirstOrDefault().ToString());
            ParameterProcessValue = Convert.ToDecimal(dp.Where(x => x.Name == "processes").Select(x => x.value).FirstOrDefault().ToString());
            return dp;
        }
        
        public decimal GetParameterSession()
        {
            return ParameterSessionValue;
        }

        public decimal GetParameterProcess()
        {
            return ParameterProcessValue;
        }

        public IEnumerable<MONITOR_DB> QueryMONITOR_DB()
        {
            _monitorDB = new MONITOR_DB(this.config);
            CurrentMONITOR_DB = _monitorDB.QUERY();
            MaxSessionValue = _monitorDB.GetMaxSessionValue();
            MaxProcessValue = _monitorDB.GetMaxProcessValue();
            return CurrentMONITOR_DB;
        }

        public decimal GetMaxSessionValue()
        {
            return MaxSessionValue;
        }

        public decimal GetMaxProcessValue()
        {
            return MaxProcessValue;
        }

        private IEnumerable<DBParameter> ConvertToParameterReadings(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new DBParameter
                {
                    Name = Convert.ToString(row["Name"]),
                    value = Convert.ToString(row["value"])
                };
            }
        }

        private IEnumerable<DBSession> ConvertToSessionReadings(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new DBSession
                {
                    Id = System.Guid.NewGuid(),
                    Type = Convert.ToString(row[0]),
                    Name = Convert.ToString(row[1]),
                    NameValue = Convert.ToDecimal(row[2].ToString()),
                    Percent = 100* Convert.ToDecimal(row[2].ToString())/ParameterSessionValue,
                    Cdt = DateTime.Now
                };
            }
        }

        private IEnumerable<DBProcess> ConvertToProcessReadings(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new DBProcess
                {
                    Id = System.Guid.NewGuid(),
                    Type = Convert.ToString(row[0]),
                    Name = Convert.ToString(row[1]),
                    NameValue = Convert.ToDecimal(row[2].ToString()),
                    Percent = 100 * Convert.ToDecimal(row[2].ToString()) / ParameterProcessValue,
                    Cdt = DateTime.Now
                };
            }
        }
    }

 
}