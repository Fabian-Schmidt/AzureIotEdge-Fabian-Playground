namespace DataReaderMHZ19
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    using Microsoft.Azure.Devices.Shared;
    using Newtonsoft.Json;

    class Program
    {
        private static volatile DesiredPropertiesData desiredPropertiesData;

        static void Main(string[] args)
        {
            Init().Wait();
        }

        /// <summary>
        /// Initializes the ModuleClient
        /// </summary>
        static async Task Init()
        {
            AmqpTransportSettings amqpSetting = new AmqpTransportSettings(TransportType.Amqp_Tcp_Only);
            ITransportSettings[] settings = { amqpSetting };

            // Open a connection to the Edge runtime
            ModuleClient ioTHubModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
            await ioTHubModuleClient.OpenAsync();
            Console.WriteLine("IoT Hub module client initialized.");

            var moduleTwin = await ioTHubModuleClient.GetTwinAsync();
            var moduleTwinCollection = moduleTwin.Properties.Desired;
            desiredPropertiesData = new DesiredPropertiesData(moduleTwinCollection);

            // callback for updating desired properties through the portal or rest api
            await ioTHubModuleClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertiesUpdate, null);

            // as this runs in a loop we don't await
            await SendData(ioTHubModuleClient);
        }

        private static Task OnDesiredPropertiesUpdate(TwinCollection twinCollection, object userContext)
        {
            desiredPropertiesData = new DesiredPropertiesData(twinCollection);
            return Task.CompletedTask;
        }

        private static async Task SendData(ModuleClient deviceClient)
        {
            var dataReader = new DataReaderMHZ19.serial.DataReader();
            try
            {
                dataReader.Open(desiredPropertiesData.SerialPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            while (true)
            {
                try
                {
                    if (desiredPropertiesData.SendData)
                    {
                        var messageBody = await dataReader.ReadGoodValueAsync();
                        if (messageBody == null)
                        {
                            Console.WriteLine($"\t{DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToLongTimeString()}> No data read.");
                        }
                        else
                        {
                            var messageString = JsonConvert.SerializeObject(messageBody);
                            var messageBytes = Encoding.UTF8.GetBytes(messageString);
                            var message = new Message(messageBytes);

                            await deviceClient.SendEventAsync("co2Output", message);
                            Console.WriteLine($"\t{DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToLongTimeString()}> Sending message body: {messageString}");
                        }
                    }
                    await Task.Delay(TimeSpan.FromSeconds(desiredPropertiesData.SendInterval));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Unexpected Exception {ex.Message}");
                    Console.WriteLine($"\t{ex.ToString()}");
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
            }
            //dataReader.Close();
        }
    }
}
