using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScriblioClone.App.Requests
{
    public class CreateGame
    {
        [JsonPropertyName("ownerUserName")]
        public string OwnerUserName { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
