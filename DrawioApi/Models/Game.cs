using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DrawioFunctions.Models
{
    public class Game
    {
        public string OwnerID { get; set; }
        public string GameCode { get; set; }
        public int RoundsLeft { get; set; }
        public int SecondsPerRound { get; set; }
        public List<Player> Players { get; set; }
    }
}
