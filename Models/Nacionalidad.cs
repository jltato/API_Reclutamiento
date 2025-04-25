

using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Nacionalidad
    {
        public int NacionalidadId { get; set; }
        public required string NombreNacionalidad { get; set; }

        [JsonIgnore]
        public List<Postulante>? Postulantes { get; set; } = []; // navegación inversa
    }
}
