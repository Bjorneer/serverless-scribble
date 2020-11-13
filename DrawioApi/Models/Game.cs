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
        public bool Started { get; set; }
        public int RoundsLeft { get; set; }
        public int SecondsPerRound { get; set; }
        public string PainterId { get; set; }
        public string CurretWord { get; set; }
        public DateTime EndOfRound { get; set; }
    }
}
