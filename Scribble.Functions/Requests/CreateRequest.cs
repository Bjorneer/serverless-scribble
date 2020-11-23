using Newtonsoft.Json;

namespace Scribble.Functions.Requests
{
    public class CreateRequest
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }
    }
}
