using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xecurity_App.Model
{
    public class Temperature
    {
        [JsonPropertyName("id")]
        public float id { get; set; }

        [JsonPropertyName("temperature")]
        public float temperature { get; set; }

        [JsonPropertyName("humidity")]
        public float humidity { get; set; }

        [JsonPropertyName("dateUploaded")]
        public DateTime dateUploaded { get; set; }
        
        [JsonPropertyName("sensorId")]
        public int sensorId { get; set; }
    }
}
