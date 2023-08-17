using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xecurity_App.Model
{
    public class KeyCard
    {
        [JsonPropertyName("id")]
        public float id { get; set; }

        [JsonPropertyName("password")]
        public string password { get; set; }

        [JsonPropertyName("expDate")]
        public DateTime expDate { get; set; }

        [JsonPropertyName("active")]
        public bool active { get; set; }
    }
}
