using Newtonsoft.Json;

namespace Scribble.Functions.Requests
{
    public class JoinRequest
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode { get; set; }
    }
}
