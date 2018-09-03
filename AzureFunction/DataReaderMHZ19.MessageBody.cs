using Newtonsoft.Json;
using System;

namespace DataReaderMHZ19
{
    public class MessageBody
    {
        [JsonProperty("co2")]
        public CO2 CO2 { get; set; }
        [JsonProperty("timeCreated")]
        public DateTime TimeCreated { get; set; }
    }

    [JsonObject("co2")]
    public class CO2
    {
        [JsonProperty("temperature")]
        public short Temperature { get; set; }
        [JsonProperty("accuracy")]
        public byte Accuracy { get; set; }
        [JsonProperty("pressure")]
        public ushort Pressure { get; set; }
        [JsonProperty("value")]
        public ushort Value { get; set; }
    }
}