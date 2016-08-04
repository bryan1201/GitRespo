using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;


namespace MEMSFieldGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            int messagesCount = 0;
            // Asynchronously sends event data serialized into JSON
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            EventHubClient client = EventHubClient.CreateFromConnectionString(connectionString, "eventhubhealth");
            Console.WriteLine(client.Path);
            Console.WriteLine("Press Enter to start the MEMS Field Gateway simulation");
            Console.WriteLine("Press Ctrl-C to stop the simulation");
            Console.ReadLine();
            while (true)
            {
                // Get Bloodpressure1 sensor reading
                HelthinSensor helthinsensor = new HelthinSensor();
                helthinsensor.UserId = "wu.bryan@inventec.com";
                helthinsensor.HealthinId = "Taipei01Helthin";
                string json = JsonConvert.SerializeObject(helthinsensor);
                client.SendAsync(new EventData(Encoding.UTF8.GetBytes(json)));
                Console.Write("{0}:", messagesCount);
                Console.WriteLine(json);
                messagesCount++;
                
                Thread.Sleep(5000); // sleeps for 800 ms
            } 
        }
    }
}
