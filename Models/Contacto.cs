using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Contacto
    {
        public int ContactoId { get; set; }
        public required int PostulanteId { get; set; }
        public required string Telefono { get; set; }
        public required string Perteneciente { get; set; }

        [JsonIgnore]
        public Postulante? Postulante { get; set; }
    }
}
