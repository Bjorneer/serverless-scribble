using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;

namespace DrawioFunctions.Models
{
    public class Game
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "ownwerid")]
        public string OwnerID { get; set; }

        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "ingame")]
        public bool InGame { get; set; }

        [JsonProperty(PropertyName = "players")]
        public List<Player> Players { get; set; } = new List<Player>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class Player
    {
        [JsonProperty(PropertyName = "playerid")]
        public string PlayerID { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
