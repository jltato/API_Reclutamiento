
using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class TipoInscripcion
    {
        public int TipoInscripcionId { get; set; } // Primary key
        public required string TipoInscripcionNombre { get; set; } 


        [JsonIgnore]
        public List<Seguimiento>? Seguimientos { get; set; } = [];
        
        
    }
}
