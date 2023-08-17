using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xecurity_App.Model
{
    public class DangerTemps
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("temperature")]
        public float Temperature { get; set; }
        [JsonPropertyName("humidity")]
        public float Humidity { get; set; }
        [JsonPropertyName("dateUploaded")]
        public DateTime DateUploaded { get; set; }
        [JsonPropertyName("serverRoomName")]
        public string ServerRoomName { get; set; }
        [JsonPropertyName("locationName")]
        public string LocationName { get; set; }
        [JsonPropertyName("locationAddress")]
        public string LocationAddress { get; set; }

    }
}
