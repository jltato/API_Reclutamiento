using System.ComponentModel.DataAnnotations;

namespace API_Reclutamiento.Models
{
    public class Postulante
    {
        public int PostulanteId { get; set; } // Primary Key
        public required string Nombre{ get; set; }
        public required string Apellido { get; set; }
        public required int Dni { get; set; }
        public required DateOnly FechaNac { get; set; }
        [Required(ErrorMessage = "Debe ingresar un email válido.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public required string Mail { get; set; }
        public required int NacionalidadId { get; set; } // Foreign Key
        public required int SexoId { get; set; }    // Foreign Key
        public required DateTime FechaSoclicitud { get; set; } = DateTime.Now;
        public int? EstabSolicitudId { get; set; } // Foreign Key
        

       
        public Nacionalidad? Nacionalidad { get; set; }    
        public Sexo? Sexo { get; set; }
        public Establecimiento? Establecimiento { get; set; }


        // 
        public DatosPersonales? DatosPersonales { get; set; }
        public Seguimiento? Seguimiento { get; set; }
        public List<Domicilio>? Domicilios { get; set; } 
        public List<Contacto>? Contactos { get; set; }
        public List<Familiares>? Familiares { get; set; }        
        public List<Estudios>? Estudios { get; set; }
        public List<Trabajo>? Trabajos{ get; set; }
        public List<Documento>? Documentos { get; set; }
        



    }
}
