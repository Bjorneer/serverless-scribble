using System;
using System.Collections.Generic;
using System.Text;

namespace DrawioFunctions.Requests
{
    public class StartGameRequest
    {
        [JsonProperty(PropertyName = "token")]
        public string PlayerID { get; set; }

        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode { get; set; }
    }
}
