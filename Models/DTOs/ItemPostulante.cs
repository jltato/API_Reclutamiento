namespace API_Reclutamiento.Models.DTOs
{
    public class ItemPostulante
    {
        public int Id { get; set; }
        public string Apellido{ get; set; }
        public string Nombre { get; set; }
        public string estabSolicitud { get; set; }
        public int Dni { get; set; }
        public string EstadoNombre { get; set; }       
        public DateTime EstadoFecha { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Sexo { get; set; }


    }
}
