using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Sexo
    {
        public int SexoId { get; set; } 
        public required string SexoName { get; set; }

        [JsonIgnore]
        public List<Postulante>? Postulantes { get; set; } = []; // navegación inversa
    }
}
