using Newtonsoft.Json;

namespace Scribble.Functions.Requests
{
    public class GuessRequest
    {
        [JsonProperty(PropertyName = "token")]
        public string PlayerID { get; set; }

        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode { get; set; }

        [JsonProperty(PropertyName = "guess")]
        public string Guess { get; set; }
    }
}
