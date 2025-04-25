using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class NivelEstudios
    {
        public int NivelEstudiosId { get; set; }
        public required string NivelNombre { get; set; }


        [JsonIgnore]
        public List<Estudios>? Estudios { get; set; } = [];
    }
}
