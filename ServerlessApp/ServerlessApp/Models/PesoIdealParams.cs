using Newtonsoft.Json;

namespace ServerlessApp.Models
{
    public class PesoIdealParams
    {
        [JsonProperty(PropertyName = "nombre")]
        public string Nombre { get; set; }

        [JsonProperty(PropertyName = "peso")]
        public double PesoKg { get; set; }

        [JsonProperty(PropertyName = "altura")]
        public double AlturaMts { get; set; }
    }
}
