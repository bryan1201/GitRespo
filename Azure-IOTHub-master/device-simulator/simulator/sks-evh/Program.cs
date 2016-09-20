using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
namespace sks_evh
{
    class Program
    {
        static void Main(string[] args)
        {
            string iotHubConnectionString = "HostName=sks-demo-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=OH/eB28iElMTVY8I2MLucAReOQd+kDXgr12XY3srMqs=";
            string iotHubD2cEndpoint = "messages/events";
            StoreEventProcessor.StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=michistorageea;AccountKey=xu0WWCzn+tL/lDM70rUV6pCX2ILovPa8imlj8HLKqr9iNgJcfBrCJabH1RdbKKeM9u5ht30KOGNoIYuNWc1hVg==";
            StoreEventProcessor.ServiceBusConnectionString = "Endpoint=sb://michiazurecontw.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=MFHzNRwJAYkqtus+6u/MGsM74nE44Z2VWmIm9S0EPbg=";

            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, iotHubD2cEndpoint, EventHubConsumerGroup.DefaultGroupName, iotHubConnectionString, StoreEventProcessor.StorageConnectionString, "messages-events");
            Console.WriteLine("Registering EventProcessor...");
            eventProcessorHost.RegisterEventProcessorAsync<StoreEventProcessor>().Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
