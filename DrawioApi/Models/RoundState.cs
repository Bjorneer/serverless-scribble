using System.Collections.Generic;

namespace Scribble.Functions.Models
{
    public class RoundState
    {
        public string PainterId { get; set; }
        public string Word { get; set; }
        public List<Player> Players { get; set; }
    }
}
