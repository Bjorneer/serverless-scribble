using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrawioFunctions.Models
{
    class Game
    {
        [JsonProperty(PropertyName ="id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "customerid")]
        public int CustomerID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
    }
}
