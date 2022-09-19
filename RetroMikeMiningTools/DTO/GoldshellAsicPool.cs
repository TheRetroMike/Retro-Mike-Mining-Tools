using Newtonsoft.Json;

namespace RetroMikeMiningTools.DTO
{
    public class GoldshellAsicPool
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        [JsonProperty(PropertyName = "pass")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "pool-priority")]
        public int PoolPriority { get; set; }

        [JsonProperty(PropertyName = "dragid")]
        public int DragId { get; set; }
    }
}
