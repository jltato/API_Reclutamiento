using System.Text.Json.Serialization;

namespace API_Reclutamiento.Models
{
    public class SectorSolicitud
    {
        public int SectorSolicitudId { get; set; }
        public required string NombreSector { get; set; }

        [JsonIgnore]
        public List<Seguimiento>? Seguimientos { get; set; } = [];
    }
}
