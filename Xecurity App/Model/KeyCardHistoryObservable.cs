using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xecurity_App.Model
{
    public class KeyCardHistoryObservable
    {
        public int id { get; set; }
        public DateTime dateUploaded { get; set; }
        public string status { get; set; }
        public Uri image { get; set; }
        public string user { get; set; }
        public int keyCardId { get; set; }
        public string serverRoomName { get; set; }
        public string locationName { get; set; }
        public string addressName { get; set; }
    }
}
