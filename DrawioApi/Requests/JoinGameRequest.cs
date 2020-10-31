using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrawioFunctions.Requests
{
    public class JoinGameRequest
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "gamecode")]
        public string GameCode{ get; set; }
    }
}
