using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xecurity_App.Model
{
    public class KeyCardHistoryDto
    {
        private string Image;

        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("dateUploaded")]
        public DateTime dateUploaded { get; set; }
        [JsonPropertyName("status")]
        public string status { get; set; }
        [JsonPropertyName("imageData")]
        public string image { get; set; }
        [JsonPropertyName("user")]
        public string user { get; set; }
        [JsonPropertyName("keyCardId")]
        public int keycardId { get; set; }
        [JsonPropertyName("serverRoomName")]
        public string serverRoomName { get; set;}
        [JsonPropertyName("locationName")]
        public string locationName { get; set;}
        [JsonPropertyName("addressName")]
        public string addressName { get; set;}
    }
}
