using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Domicilio
    {
        public int DomicilioId { get; set; }  // Clave primaria autoincremental
        public int PostulanteId { get; set; } // Clave foránea
        public required string Calle { get; set; }
        public required string Numero { get; set; }
        public required int LocalidadId { get; set; } // Clave foránea
        public required string CodigoPostal { get; set; }
        

        
        [JsonIgnore]
        public Postulante? Postulante { get; set; } // Propiedad de navegación
      
        public Localidad? Localidad { get; set; }
    }
}
