using ScriblioClone.App.Responses;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScriblioClone.App.Models
{
    public class GameState
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string GameOwnerId { get; set; }
        public string GameCode { get; set; }
        public IEnumerable<GameUser> Users { get; set; }
        public int RoundsLeft { get; set; }
        public int SecondsToDraw { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
