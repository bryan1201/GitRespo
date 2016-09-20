using IOTGateway.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gw_simulator
{
    public static class Extensions
    {
        public static string ToJsonString(this TelemetryData data)
        {
            return JsonConvert.SerializeObject(data);
        }
        public static void Random(this TelemetryData data, string deviceID, string seqNo, string msg, int min, int max)
        {
            var random = new Random((int)DateTime.UtcNow.Ticks);
            int type = random.Next(0, 2);
            var rnd = random.Next(0, 101);
            if (rnd <= max && rnd >= min)
            {
                type = 1;
            }
            data.Temperature = rnd;
            data.SeqNo = seqNo;
            data.Timestamp = DateTime.UtcNow;
            data.Type = type;
            data.DeviceId = deviceID;
            data.UID = "UID-" + Guid.NewGuid().ToString();
            data.DCorAC = random.Next(0, 1);
            data.ADSLor3G = random.Next(100) >= 50 ? "ADSL" : "3G";
            data.Message = msg;
        }
    }
}
