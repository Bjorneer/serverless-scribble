using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrawioFunctions.Requests
{
    public class GuessRequest
    {
        [JsonProperty(PropertyName = "token")]
        public string PlayerID { get; set; }

        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode { get; set; }

        [JsonProperty(PropertyName ="guess")]
        public string Guess { get; set; }
    }
}
