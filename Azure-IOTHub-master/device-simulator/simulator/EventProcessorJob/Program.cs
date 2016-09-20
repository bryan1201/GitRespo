using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;

namespace EventProcessorJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var host = new JobHost();
            string iotHubConnectionString = "HostName=sks-demo-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=OH/eB28iElMTVY8I2MLucAReOQd+kDXgr12XY3srMqs=";
            
            //https://azure.microsoft.com/en-us/documentation/articles/iot-hub-devguide/#endpoints
            string iotHubD2cEndpoint = "messages/events";
            SKSEventProcessor.StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=michistorageea;AccountKey=xu0WWCzn+tL/lDM70rUV6pCX2ILovPa8imlj8HLKqr9iNgJcfBrCJabH1RdbKKeM9u5ht30KOGNoIYuNWc1hVg==";
            SKSEventProcessor.ServiceBusConnectionString = "Endpoint=sb://michiazurecontw.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=MFHzNRwJAYkqtus+6u/MGsM74nE44Z2VWmIm9S0EPbg=";

            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, iotHubD2cEndpoint, EventHubConsumerGroup.DefaultGroupName, iotHubConnectionString, SKSEventProcessor.StorageConnectionString,"messages-events");

            Console.WriteLine("Registering EventProcessor...");
            eventProcessorHost.RegisterEventProcessorAsync<SKSEventProcessor>().Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            host.RunAndBlock();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
