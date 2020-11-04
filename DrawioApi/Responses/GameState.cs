using DrawioFunctions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrawioFunctions.Responses
{
    public class GameState
    {
        [JsonProperty(PropertyName = "started")]
        public bool Started { get; set; }

        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode { get; set; }

        [JsonProperty(PropertyName = "players")]
        public List<SlimPlayer> Players { get; set; }

        [JsonProperty(PropertyName = "playerId")]
        public string PlayerID { get; set; }
    }

    public class SlimPlayer
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }
    }
}
