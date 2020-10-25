using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScriblioClone.App.Responses
{
    public class GameLobbyState
    {
        public LobbyUser User { get; set; }
        public string GameCode { get; set; }
        public string LobbyOwnerUserName { get; set; }
        public IEnumerable<string> Users { get; set; }
        public int Rounds { get; set; }
        public int SecondsToDraw { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
