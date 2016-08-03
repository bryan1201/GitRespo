using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BService.Models
{
    public interface IStatistic
    {
        void Init();
        IEnumerable<DBSession> StatisticSession();
        IEnumerable<DBProcess> StatisticProcess();
        IEnumerable<DBParameter> GetParameter();
        IEnumerable<MONITOR_DB> QueryMONITOR_DB();

        DBSession TotalStatisticSession();
        DBProcess TotalStatisticProcess();
        decimal GetParameterSession();
        decimal GetParameterProcess();
        decimal GetMaxSessionValue();
        decimal GetMaxProcessValue();

        void LogSessionDB();
        void LogProcessDB();
        void LogTotalSessionDB();
        void LogTotalProcessDB();
        string ChartData();
    }
}
