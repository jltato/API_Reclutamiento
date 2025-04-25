using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Seguimiento
    {
        public int SeguimientoId { get; set; } // Primary Key
        public required int PostulanteId { get; set; } // Foreign Key
        public required int EstadoId { get; set; } // Foreign Key
        public required int TipoInscripcionId { get; set; } // Foreign Key
        public DateOnly EntrevistaGrupal { get; set; }
        public bool? AptoGrupal { get; set; }
        public DateOnly EntrevistaPsicologica {  get; set; }
        public bool? AptoPsicologico {  get; set; }
        public DateOnly TurnoMedico {  get; set; }
        public bool? AptoMedico { get; set; }   
        public DateOnly ReconocimientoMed {  get; set; }
        public bool? AptoReconocMed { get; set; }
        public string Observaciones {  get; set; } = string.Empty;
        public bool? Notificado {  get; set; }
        public DateTime Modify_At { get; set; }
        public string Modify_By { get; set; } = string.Empty;



        [JsonIgnore]
        public  Postulante? Postulante { get; set; }
        [JsonIgnore]
        public TipoInscripcion? TipoInscripcion { get; set; }
        [JsonIgnore]
        public Estado? Estado { get; set; }
    }
}
