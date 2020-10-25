using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ScriblioClone.App.Models
{
    public class GameUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
