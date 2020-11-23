using Newtonsoft.Json;
using Scribble.Functions.Models;
using System.Collections.Generic;

namespace Scribble.Functions.Requests
{
    public class DrawRequest
    {
        [JsonProperty(PropertyName = "token")]
        public string PlayerID { get; set; }

        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode { get; set; }

        [JsonProperty(PropertyName = "drawObjects")]
        public List<DrawObject> DrawObjects { get; set; }
    }
}
