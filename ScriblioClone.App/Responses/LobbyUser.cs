using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScriblioClone.App.Responses
{
    public class LobbyUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string UserName { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
