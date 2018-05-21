using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataReaderMHZ19
{
/* 
    public class DataReader
    {
        public static async Task<MessageBody> ReadDataAsync(int GPIO_Pin, string SensorType)
        {
            DataReaderOutput processOutput = null;
            try
            {
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.FileName = "go-datareader-dht22";
                    process.StartInfo.Arguments = $" -pin {GPIO_Pin}";

                    Console.WriteLine($"{process.StartInfo.FileName} {process.StartInfo.Arguments}");

                    process.Start();
                    process.WaitForExit();

                    var stdOut = process.StandardOutput.ReadToEnd();
                    processOutput = JsonConvert.DeserializeObject<DataReaderOutput>(stdOut);
                    if (processOutput == null)
                    {
                        Console.WriteLine("StandardError:");
                        Console.WriteLine(process.StandardError.ReadToEnd());

                        Console.WriteLine("StandardOutput:");
                        Console.WriteLine(stdOut);
                    }
                }
                Console.WriteLine("END");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            if (processOutput != null)
            {
                var messageBody = new MessageBody
                {
                    Ambient = new Ambient
                    {
                        Temperature = processOutput.Temperatur,
                        Humidity = processOutput.Humidity,
                        VPD = processOutput.VPD
                    },
                    Retry = processOutput.Retry,
                    TimeCreated = string.Format("{0:O}", DateTime.UtcNow)
                };
                return messageBody;
            }
            return null;
        }
    }*/
}