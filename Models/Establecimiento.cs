using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Establecimiento
    {
        public int EstablecimientoId { get; set; }
        public required string EstablecimientoNombre { get; set; }
        public required string EstablecimientoCiudad { get; set; }


        [JsonIgnore]
        public List<Postulante>? Postulantes { get; set; } = [];
    }
}
