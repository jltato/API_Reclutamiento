using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class EstadoCivil
    {
        public int EstadoCivilId { get; set; }
        public required string EstadoCivilNombre { get; set; }

        [JsonIgnore]
        public List<DatosPersonales>? DatosPersonales { get; set; }
    }
}
