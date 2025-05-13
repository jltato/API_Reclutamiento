using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace API_Reclutamiento.Models
{
    public class Trabajo
    {
        public int TrabajoId { get; set; }
        public required int PostulanteId { get; set; }
        public required string ActividadLaboral { get; set; }
        public DateOnly Desde { get; set; }
        [AllowNull]
        public DateOnly Hasta { get; set; }
        public bool IntentoAnterior  { get; set; } = false;
        public string EtapaAlzanzada { get; set; } = string.Empty;
        public bool OtraFuerza { get; set; } = false;
        public string MotivoBaja { get; set; } = string.Empty;


        [JsonIgnore]
        public Postulante? Postulante { get; set; }  
    }
}
