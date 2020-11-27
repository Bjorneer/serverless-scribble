using System.Collections.Generic;

namespace Scribble.Functions.Models
{
    public class Game
    {
        public string OwnerID { get; set; }
        public string GameCode { get; set; }
        public int RoundsLeft { get; set; }
        public int SecondsPerRound { get; set; }
        public List<Player> Players { get; set; }
        public string LastPainterId { get; set; }
    }
}
