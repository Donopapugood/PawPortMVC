namespace PawPortMVC.Models
{
    public class Clinica
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Pais { get; set; } = "";
        public string Ciudad { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string HorarioLunesViernes { get; set; } = "08:00 AM - 08:00 PM";
        public string HorarioSabado { get; set; } = "09:00 AM - 05:00 PM";
        public string HorarioDomingo { get; set; } = "Urgencias únicamente";
        public string Imagen { get; set; } = "";
        public string Especialidad { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public double Calificacion { get; set; }
        public int TotalReseñas { get; set; }
    }
}
