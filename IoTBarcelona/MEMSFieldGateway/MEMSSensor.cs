using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEMSFieldGateway
{
    public class HelthinSensor
    {
        //public string organization { get { return "inventec IoT Lab"; } }
        //public string displayname { get { return "Helthin-PC"; } }
        //public string location { get { return "Taipei"; } }
        public string UserId { get; set; }
        public string HealthinId { get; set; }
        private static string _barcelonaId = "8E4XGXRPJKVFUM9SGU5J";
        public string BarcelonaId { get { return _barcelonaId; } }

        public double Bloodpressure1
        {
            get
            {
                _measurename = "Bloodpressure1";
                return value;
            }
        }

        public double Bloodpressure2
        {
            get
            {
                _measurename = "Bloodpressure2";
                return value;
            }
        }

        public double Bloodglucose
        {
            get
            {
                _measurename = "Bloodglucose";
                return value;
            }
        }

        public double Heartbeat
        {
            get
            {
                _measurename = "Heartbeat";
                return value;
            }
        }

        private string _measurename;

        public string cdt
        {
            get
            {
                return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            }
        }
        private double value
        {
            get
            {
                if (_measurename.Equals("Bloodpressure1", StringComparison.OrdinalIgnoreCase))
                {
                    Random random = new Random();
                    double rnddouble = random.NextDouble() * (26.2 - 25) + 25;
                    return Math.Round(rnddouble, 3);
                }
                else if (_measurename.Equals("Bloodpressure2", StringComparison.OrdinalIgnoreCase))
                {
                    Random random = new Random();
                    double rnddouble = random.NextDouble() * (152-90) + 52;
                    return Math.Round(rnddouble, 3);
                }
                else if (_measurename.Equals("Bloodglucose", StringComparison.OrdinalIgnoreCase))
                {
                    Random random = new Random();
                    double rnddouble = random.NextDouble() * (252-150) + 52;
                    return Math.Round(rnddouble, 3);
                }
                else if (_measurename.Equals("Heartbeat", StringComparison.OrdinalIgnoreCase))
                {
                    Random random = new Random();
                    double rnddouble = random.NextDouble() * (150 - 0) + 20;
                    return Math.Round(rnddouble, 3);
                }
                else return 0;
            }
        }
    }

    public class MEMSSensor
    {
        private static string _guid = null;
        public string guid { get { return _guid; } }
        public string organization { get { return "Microsoft IoT Open Lab"; } }
        public string displayname { get { return "MEMSSensor-PC"; } }
        public string location { get { return "Taipei"; } }
        private string _measurename;
        public string measurename
        {
            get { return _measurename; }
            set { _measurename = value; }
        }
        private string _unitOfMeasure;
        public string unitofmeasure
        {
            get { return _unitOfMeasure; }
            set { _unitOfMeasure = value; }
        }
        public string timecreated
        {
            get
            {
                return DateTime.UtcNow.ToString("o");
            }
        }
        public double value
        {
            get
            {
                if (_measurename.Equals("temperature",
               StringComparison.OrdinalIgnoreCase))
                {
                    Random random = new Random();
                    double rnddouble = random.NextDouble() * (26.2 - 25) + 25;
                    return Math.Round(rnddouble, 3);
                }
                else if (_measurename.Equals("humidity",
               StringComparison.OrdinalIgnoreCase))
                {
                    Random random = new Random();
                    double rnddouble = random.NextDouble() * (50 - 52) + 52;
                    return Math.Round(rnddouble, 3);
                }
                else return 0;
            }
        }
        public MEMSSensor()
        {
            if (String.IsNullOrEmpty(_guid))
                _guid = System.Guid.NewGuid().ToString("D").ToUpper();
        }
    }

}
