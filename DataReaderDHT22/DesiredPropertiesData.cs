// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace DataReaderDHT22
{
    public class DesiredPropertiesData
    {
        private bool _sendData = true;
        private int _sendInterval = 30;
        private int _GPIO_Pin = 0;
        private string _Sensor_Type = "dht22";

        public DesiredPropertiesData(TwinCollection twinCollection)
        {
            Console.WriteLine($"Updating desired properties {twinCollection.ToJson(Formatting.Indented)}");
            try
            {
                if (twinCollection.Contains("SendData") && twinCollection["SendData"] != null)
                {
                    _sendData = twinCollection["SendData"];
                }

                if (twinCollection.Contains("SendInterval") && twinCollection["SendInterval"] != null)
                {
                    _sendInterval = twinCollection["SendInterval"];
                    if (_sendInterval < 30)
                    {
                        _sendInterval = 30;
                    }
                }

                if (twinCollection.Contains("GPIOPin") && twinCollection["GPIOPin"] != null)
                {
                    _GPIO_Pin = twinCollection["GPIOPin"];
                }

                if (twinCollection.Contains("SensorType") && twinCollection["SensorType"] != null)
                {
                    _Sensor_Type = twinCollection["SensorType"];
                }
            }
            catch (AggregateException aexc)
            {
                foreach (var exception in aexc.InnerExceptions)
                {
                    Console.WriteLine($"[ERROR] Could not retrieve desired properties {aexc.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Reading desired properties failed with {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"Value for SendData = {_sendData}");
                Console.WriteLine($"Value for SendInterval = {_sendInterval}");
                Console.WriteLine($"Value for GPIOPin = {_GPIO_Pin}");
                Console.WriteLine($"Value for SensorType = {_Sensor_Type}");
            }
        }

        public bool SendData => _sendData;
        public int SendInterval => _sendInterval;
        public int GPIOPin => _GPIO_Pin;
        public string SensorType => _Sensor_Type;
    }
}
