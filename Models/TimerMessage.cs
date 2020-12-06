using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Smorgo.PeriodicTimer.Models
{
    public class TimerMessage : IMqttMessage
    {
        public String Label {get; set;}
        public DateTime Timestamp {get; set;}
        [JsonIgnore]
        public String Payload
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}