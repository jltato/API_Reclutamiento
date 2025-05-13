using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Seguimiento
    {
        public int SeguimientoId { get; set; } // Primary Key
        public required int PostulanteId { get; set; } // Foreign Key
        public required int EstadoId { get; set; } // Foreign Key
        public required int TipoInscripcionId { get; set; } // Foreign Key
        public int? EstadoSeguimientoActualId { get; set; }  // FK opcional
        public string Observaciones {  get; set; } = string.Empty;
        public DateTime Modify_At { get; set; } = DateTime.Now;
        public string Modify_By { get; set; } = string.Empty;


        [JsonIgnore]
        public  Postulante? Postulante { get; set; }
       
        public TipoInscripcion? TipoInscripcion { get; set; }
        public List<EstadoSeguimiento> EstadosSeguimiento { get; set; } = [];
        public EstadoSeguimiento? EstadoSeguimientoActual { get; set; }
        public Estado? Estado { get; set; }
    }
}
