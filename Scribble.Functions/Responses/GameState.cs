using Newtonsoft.Json;
using System.Collections.Generic;

namespace Scribble.Functions.Responses
{
    public class GameState
    {
        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode { get; set; }

        [JsonProperty(PropertyName = "players")]
        public List<SlimPlayer> Players { get; set; }

        [JsonProperty(PropertyName = "playerId")]
        public string PlayerID { get; set; }

        [JsonProperty(PropertyName = "word")]
        public string Word { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        [JsonProperty(PropertyName = "secondsLeft")]
        public int SecondsLeft { get; set; }
    }

    public class SlimPlayer
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }
    }
}
