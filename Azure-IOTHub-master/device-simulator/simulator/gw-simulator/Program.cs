using IOTGateway;
using IOTGateway.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gw_simulator
{
    class Program
    {
        static string deviceId = null;
        static int max = 100;
        static int min = 0;
        static bool Continue = true;
        #region Helper Function
        static void Wait(string msg = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                Log("Press [ENTER] to continue...");
            }
            else
            {
                Log(msg);
            }
            Console.ReadLine();
        }

        static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        static void Log(string msg)
        {
            Console.ResetColor();
            Console.WriteLine(msg);
        }
        #endregion
        static void Main(string[] args)
        {
            Error("You MUST register deviceId to Field Gateway first, navigate to http://{youwebname}.azurewebsites.net/swagger/ui to register device");
            Wait("Press [ENTER] to continue...");
            if (args.Length < 3)
            {
                Error("Usage: sumulator {deviceid} {max} {min}");
                Wait();
                return;
            }
            deviceId = args[0];
            max = int.Parse(args[2]);
            min = int.Parse(args[1]);

            //SendTelemetry2();
            //ReceiveCommandAsync();
            Thread send = new Thread(SendTelemetry_SKS);// SendTelemetry_SKS);
            //Thread recv = new Thread(ReceiveCommand2);
            send.Start();
            //recv.Start();
            Wait("Press [ENTER] to exit...");
            send.Join(3000);
            //recv.Join(3000);
            Continue = false;
        }
       static void ReceiveCommand2()
        {
            int i = 0;
            while (Continue)
            {
                i++;
                try
                {
                    Log($"Receiving...");
                    var url = ConfigurationManager.AppSettings["gwhost"] + $"/api/FieldGateway/ReceiveCommand?deviceId={deviceId}";
                    HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
                    req.Method = "GET";
                    req.ContentType = "application/json";
                    req.ContentLength = 0;
                    //req.GetRequestStream().Write(buffer, 0, buffer.Length);

                    using (var resp = req.GetResponse())
                    {
                        using (var respStream = resp.GetResponseStream())
                        {
                            using (var sr = new StreamReader(respStream))
                            {
                                var cmd = sr.ReadToEnd();
                                if (!string.IsNullOrEmpty(cmd) && cmd != "\"\"")
                                {
                                    Success(cmd);
                                }
                            }
                        }
                    }
                    Thread.Sleep(3000);
                }
                catch (Exception exp)
                {
                    Error($"RECV::Exception[{exp.Message}]");
                }
            }
        }

        static void SendTelemetry2()
        {
            int i = 0;
            while (Continue)
            {
                i++;
                try
                {
                    var telemetry = GenerateMessage(i, $"message:{i}");
                    var text = JsonConvert.SerializeObject(telemetry);
                    Log($"[{i}]Sending {text}");
                    var buffer = Encoding.UTF8.GetBytes(text);
                    var url = ConfigurationManager.AppSettings["gwhost"] + "/api/FieldGateway/SendTelemetry";
                    HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
                    req.Method = "POST";
                    req.ContentType = "application/json";
                    req.GetRequestStream().Write(buffer, 0, buffer.Length);
                    
                    using (var resp = req.GetResponse())
                    {
                        using (var respStream = resp.GetResponseStream())
                        {
                            using(var sr = new StreamReader(respStream))
                            {
                                var cmd = sr.ReadToEnd();
                            }
                        }
                    }
                    
                    Thread.Sleep(500);
                }
                catch(Exception exp)
                {
                    Error($"SEND::Exception[{exp.Message}]");
                }
            }

        }
        static void SendTelemetry_SKS()
        {
            //int i = 0;
            for(int i = 0; i < 500; i ++)
            {
                //i++;
                try
                {
                    var text = GenerateMessage(deviceId);
                    Log($"[{i}]Sending {text}");
                    var buffer = Encoding.UTF8.GetBytes(text);
                    //var url = "http://sksappgateway.azurewebsites.net/uisgw/api/Receive";
                    var url = "http://sksiotdev.azurewebsites.net/uisgw/api/Receive";
                    HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
                    req.Method = "POST";
                    req.ContentType = "application/json";
                    req.GetRequestStream().Write(buffer, 0, buffer.Length);
                    Log($"Remote Url:{req.RequestUri.ToString()}");
                    using (var resp = req.GetResponse())
                    {
                        using (var respStream = resp.GetResponseStream())
                        {
                            using (var sr = new StreamReader(respStream))
                            {
                                var cmd = sr.ReadToEnd();
                            }
                        }
                    }

                    Thread.Sleep(500);
                }
                catch (Exception exp)
                {
                    Error($"SEND::Exception[{exp.Message}]");
                }
            }

        }
        static string GenerateMessage(string deviceId)
        {
            var resp = "{\"id\": 0,\"sno\": \"string\",\"sigtime\": \"string\",\"crtime\": \"string\",\"catg\": \"string\",\"mno\": \"{$deviceId$}\"" +
                          ",\"uid\": \"string\",\"power\": \"string\",\"lan\": \"string\",\"scode\": \"string\",\"status\": \"string\",\"R6scode\": \"string\"}";
            resp = resp.Replace("{$deviceId$}", deviceId);

            return resp;
        }
        static TelemetryData GenerateMessage(int seq, string message)
        {
            TelemetryData msg = new TelemetryData();
            msg.Random(deviceId, string.Format("{0}{1}", DateTime.UtcNow.ToString("yyyymmdd"), seq.ToString("0000000")), message, min, max);
            return msg;
        }
    }
}
