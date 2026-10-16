namespace PawPortMVC.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public string Fecha { get; set; } = "";
        public string Hora { get; set; } = "";
        public string Mascota { get; set; } = "";
        public string Especie { get; set; } = "";
        public string Propietario { get; set; } = "";
        public string Clinica { get; set; } = "";
        public string Veterinario { get; set; } = "";
        public string Especialidad { get; set; } = "";
        public string Motivo { get; set; } = "";
        public string Notas { get; set; } = "";
        public string Estado { get; set; } = "";
    }
}
