namespace PawPortMVC.Models
{
    public class Veterinario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Especialidad { get; set; } = "";
        public string Clinica { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Horario { get; set; } = "";
        public string Estado { get; set; } = "Activo";
    }
}
