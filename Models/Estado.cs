using System.Text.Json.Serialization;

namespace API_Reclutamiento.Models
{
    public class Estado
    {
        public int EstadoId { get; set; }
        public required string EstadoNombre { get; set; }


        [JsonIgnore]
        public List<Seguimiento>? Seguimientos { get; set; } = [];
    }
}
