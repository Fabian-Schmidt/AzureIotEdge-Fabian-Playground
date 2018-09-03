using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunction
{
    public static class QueueMsgToPowerBi
    {
        private const string Setting_PowerBIPushUrl = "PowerBIPushURL";
        private const string localTimeZoneId = "AUS Eastern Standard Time";
        private static readonly TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneId);
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName("QueueMsgToPowerBi")]
        public static async Task Run(
            [ServiceBusTrigger(queueName: "iothubfabian", Connection = "IotHubFabian")]Message myQueueItem,
            ILogger log,
            CancellationToken token)
        {
            var messageProperties = myQueueItem.UserProperties;
            var messageBody = Encoding.UTF8.GetString(myQueueItem.Body);
            var moduleId = messageProperties["iothub-connection-module-id"].ToString();
            if (moduleId == "dht22")
            {
                var msg = JsonConvert.DeserializeObject<DataReaderDHT22.MessageBody>(messageBody);
                var obj = new PowerBi.MessageBody
                {
                    new PowerBi.Value()
                    {
                        TimeCreated = msg.TimeCreated,
                        LocalTime = TimeZoneInfo.ConvertTimeFromUtc(msg.TimeCreated,localTimeZone),
                        Temperature = msg.Ambient.Temperature,
                        Humidity = msg.Ambient.Humidity,
                        VPD = msg.Ambient.VPD
                    }
                };

                var url = Environment.GetEnvironmentVariable(Setting_PowerBIPushUrl);
                await HttpClient.PostAsJsonAsync(url, obj, token);
            }
            else if (moduleId == "mhz19")
            {
                var msg = JsonConvert.DeserializeObject<DataReaderMHZ19.MessageBody>(messageBody);
                var obj = new PowerBi.MessageBody
                {
                    new PowerBi.Value()
                    {
                        TimeCreated = msg.TimeCreated,
                        LocalTime = TimeZoneInfo.ConvertTimeFromUtc(msg.TimeCreated,localTimeZone),
                        CO2 = msg.CO2.Value,
                        Pressure = msg.CO2.Pressure
                    }
                };

                var url = Environment.GetEnvironmentVariable(Setting_PowerBIPushUrl);
                await HttpClient.PostAsJsonAsync(url, obj, token);
            }
            else
            {
                log.LogInformation($"recieved message for unknown module {moduleId}. Message: {messageBody}");
            }
        }
    }
}
