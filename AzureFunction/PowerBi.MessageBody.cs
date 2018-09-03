using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PowerBi
{
    public class MessageBody : List<Value>
    {
    }

    public class Value
    {
        [JsonProperty("TimeCreated")]
        public DateTime TimeCreated { get; set; }
        [JsonProperty("LocalTime")]
        public DateTime LocalTime { get; set; }

        [JsonProperty("Temperature", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double Temperature { get; set; }
        [JsonProperty("Humidity", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double Humidity { get; set; }
        [JsonProperty("VPD", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double VPD { get; set; }

        [JsonProperty("Pressure", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ushort Pressure { get; set; }
        [JsonProperty("CO2", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ushort CO2 { get; set; }
    }
}