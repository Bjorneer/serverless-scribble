using Newtonsoft.Json;

namespace Scribble.Functions.Requests
{
    public class StartRequest
    {
        [JsonProperty(PropertyName = "token")]
        public string PlayerID { get; set; }

        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode { get; set; }
    }
}
