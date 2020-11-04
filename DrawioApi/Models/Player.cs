using System;
using System.Collections.Generic;
using System.Text;

namespace DrawioFunctions.Models
{
    public class Player
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public DateTime LastRequestAt { get; set; }
    }
}
