using Newtonsoft.Json;

namespace ServerlessApp.Models
{
    public class PesoIdealResult
    {
        [JsonProperty(PropertyName = "result")]
        public double Result { get; set; }
    }
}
