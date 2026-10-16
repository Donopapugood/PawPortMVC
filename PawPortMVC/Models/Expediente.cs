namespace PawPortMVC.Models
{
    public class Expediente
    {
        public int Id { get; set; }
        public int IdMascota { get; set; }
        public string Mascota { get; set; } = "";
        public string Propietario { get; set; } = "";
        public string Clinica { get; set; } = "";
        public string Veterinario { get; set; } = "";
        public string Fecha { get; set; } = "";
        public string Diagnostico { get; set; } = "";
        public string Tratamiento { get; set; } = "";
        public string MedicamentoRecetado { get; set; } = "";
        public string VacunaAplicada { get; set; } = "";
        public double PesoRegistrado { get; set; }
        public string Notas { get; set; } = "";
    }
}
