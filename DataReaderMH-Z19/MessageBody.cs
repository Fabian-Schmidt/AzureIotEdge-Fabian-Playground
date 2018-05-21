// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;

namespace DataReaderMHZ19
{
    public class MessageBody
    {
        [JsonProperty("ambient")]
        public Ambient Ambient { get; set; }
        [JsonProperty("timeCreated")]
        public string TimeCreated { get; set; }
    }

    [JsonObject("ambient")]
    public class Ambient
    {
        [JsonProperty("temperature")]
        public short Temperature { get; set; }
        [JsonProperty("accuracy")]
        public byte Accuracy { get; set; }
        [JsonProperty("pressure")]
        public ushort Pressure { get; set; }
        [JsonProperty("co2")]
        public ushort CO2 { get; set; }
    }
}
