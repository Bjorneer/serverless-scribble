using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scribble.Functions.Models
{
    public class MessageItem
    {
        [JsonProperty(PropertyName ="user")]
        public string User { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
