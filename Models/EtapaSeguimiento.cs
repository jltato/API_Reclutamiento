using System.Text.Json.Serialization;

namespace API_Reclutamiento.Models
{
    public class EtapaSeguimiento
    {
        public int EtapaSeguimientoId { get; set; }
        public required string NombreEtapa { get; set; }

        //propiedades de navegacion
        [JsonIgnore]
        public List<EstadoSeguimiento> EstadoSeguimiento { get; set; } = [];
    }
}
