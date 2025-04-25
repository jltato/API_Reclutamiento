using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Localidad
    {
        public int LocalidadId { get; set; }
        public required string LocalidadName { get; set; }

        [JsonIgnore]
        public List<Domicilio>? Domicilio { get; set; }

    }
}
