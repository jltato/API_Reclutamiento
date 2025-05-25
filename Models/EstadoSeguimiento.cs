using System.Text.Json.Serialization;

namespace API_Reclutamiento.Models
{
    public class EstadoSeguimiento
    {
        public int EstadoSeguimientoId { get; set; }
        public required int SeguimientoId { get; set; }
        public required int EtapaSeguimientoId { get; set; }
        public DateTime FechaTurno { get; set; }
        public bool Asistencia { get; set; }
        public bool Apto { get; set; }
        public bool NoApto { get; set; }
        public bool Notificado { get; set; }

        //propiedades de navegación 
        [JsonIgnore]
        public Seguimiento? Seguimiento { get; set; }
        public EtapaSeguimiento? EtapaSeguimiento { get; set; }
        

    }
}
