using ScriblioClone.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ScriblioClone.App.Responses
{
    public class GameStateResponse
    {
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
