using Newtonsoft.Json;

namespace Scribble.Functions.Models
{
    public class DrawObject
    {
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }
        [JsonProperty(PropertyName = "fromX")]
        public double FromX { get; set; }
        [JsonProperty(PropertyName = "toX")]
        public double ToX { get; set; }
        [JsonProperty(PropertyName = "fromY")]
        public double FromY { get; set; }
        [JsonProperty(PropertyName = "toY")]
        public double ToY { get; set; }
        [JsonProperty(PropertyName = "clear")]
        public bool Clear { get; set; }
    }
}
