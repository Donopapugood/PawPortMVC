namespace PawPortMVC.Models
{
    public class Recordatorio
    {
        public int Id { get; set; }
        public string Mascota { get; set; } = "";
        public string Propietario { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Mensaje { get; set; } = "";
        public string FechaLimite { get; set; } = "";
        public bool Completado { get; set; }
    }
}
