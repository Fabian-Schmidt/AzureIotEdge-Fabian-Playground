using Newtonsoft.Json;
using System;

namespace DataReaderDHT22
{
    public class MessageBody
    {
        [JsonProperty("ambient")]
        public Ambient Ambient { get; set; }
        [JsonProperty("timeCreated")]
        public DateTime TimeCreated { get; set; }
        [JsonProperty("retry")]
        public int Retry { get; set; }
    }

    [JsonObject("ambient")]
    public class Ambient
    {
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
        [JsonProperty("humidity")]
        public double Humidity { get; set; }
        [JsonProperty("vpd")]
        public double VPD { get; set; }
        
    }
}