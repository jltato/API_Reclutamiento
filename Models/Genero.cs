using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Genero
    {
        public int GeneroId { get; set; }
        public required string GeneroNombre { get; set; }

        [JsonIgnore]
        public List<DatosPersonales>? DatosPersonales { get; set; } = [];
    }
}
