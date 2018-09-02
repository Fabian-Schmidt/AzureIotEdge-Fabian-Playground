using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataReaderMHZ19.serial
{
    public class DataReader//:IDisposable
    {
        //See https://revspace.nl/MHZ19
        private static readonly byte[] ReadGasConcentration = new byte[] { 0xFF, 0x01, 0x86, 0x00, 0x00, 0x00, 0x00, 0x00, 0x79 };
        private const int READ_INTERVAL = 10 * 1000;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationToken CancellationToken => cts.Token;
        private SerialDevice mySer = null;

        public DataReader()
        { }

        public bool Open(byte serialPort)
        {
            var desiredPort = $"/dev/ttyS{serialPort}";
            var ports = SerialDevice.GetPortNames();
            bool isTTY = false;
            foreach (var prt in ports)
            {
                Console.WriteLine($"Serial name: {prt}");
                if (prt == desiredPort)
                {
                    isTTY = true;
                }
            }

            if (!isTTY)
            {
                Console.WriteLine($"No '{desiredPort}' serial port!");
                return false;
            }

            mySer = new SerialDevice(desiredPort, BaudRate.B9600);
            mySer.DataReceived += MySer_DataReceived;
            mySer.Open();
            return true;
        }

        public void Close()
        {
            cts.Cancel();
            if (mySer != null)
            {
                mySer.Close();
                mySer = null;
            }
        }

        private volatile MessageBody data = null;

        public async Task<MessageBody> ReadGoodValueAsync()
        {
            data = null;
            while (!CancellationToken.IsCancellationRequested && data == null)
            {
                //Console.WriteLine("Sending read command to sensor");
                mySer.Write(ReadGasConcentration);
                await Task.Delay(250, CancellationToken);
                if (data == null)
                {
                    try
                    {
                        await Task.Delay(READ_INTERVAL, CancellationToken);
                    }
                    catch (TaskCanceledException) { }
                }
            }
            return data;
        }

        private byte[] ReadBuffer;

        private void MySer_DataReceived(object arg1, byte[] arg2)
        {
            if (ReadBuffer != null)
            {
                var newData = new List<byte>(ReadBuffer);
                newData.AddRange(arg2);
                ReadBuffer = newData.ToArray();
            }
            else
            {
                ReadBuffer = arg2;
            }

            if (ReadBuffer.Length == 9)
            {
                if (ReadBuffer[0] == 0xFF /* Starting byte */
                && ReadBuffer[1] == 0x86 /* command byte */)
                {
                    byte checksum = 0x00;
                    for (int i = 1; i < 8; i++)
                    {
                        checksum += ReadBuffer[i];
                    }
                    checksum = (byte)(((byte)0xff) - checksum);
                    checksum += (byte)1;

                    if (checksum != ReadBuffer[8])
                    {
                        Console.Write("CRC error.");
                    }

                    var temp = (short)(ReadBuffer[4] - 40);
                    /** accuracy? 0x04, 0x08, 0x10, 0x20, 0x40(best) */
                    var accuracy = ReadBuffer[5];

                    /** pressure? */
                    var pressure = (ushort)(ReadBuffer[6] * 256 + ReadBuffer[7]);

                    var value = (ushort)(ReadBuffer[2] * 256 + ReadBuffer[3]);

                    Console.WriteLine($"{accuracy:X2}, {temp:D3}, {pressure:D5}, {value:D4}");

                    if (accuracy >= 0x40 && pressure < 15000)
                    {
                        data = new MessageBody()
                        {
                            TimeCreated = string.Format("{0:O}", DateTime.UtcNow),
                            CO2 = new CO2()
                            {
                                Temperature = temp,
                                Accuracy = accuracy,
                                Pressure = pressure,
                                Value = value
                            }
                        };
                    }
                }
                ReadBuffer = null;
            }
            else if (ReadBuffer.Length > 9)
            {
                ReadBuffer = null;
            }
        }
    }
}
