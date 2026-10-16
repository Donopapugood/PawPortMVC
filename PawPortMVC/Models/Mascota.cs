namespace PawPortMVC.Models
{
    public class Mascota
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Especie { get; set; } = "";
        public string Raza { get; set; } = "";
        public int Edad { get; set; }
        public double Peso { get; set; }
        public string Sexo { get; set; } = "Hembra";
        public string Propietario { get; set; } = "";
        public string Imagen { get; set; } = "";
        public string Vacunas { get; set; } = "";
        public string Estado { get; set; } = "Activo";
        public string UltimaVisita { get; set; } = "";
        public string Alergias { get; set; } = "Ninguna registrada";
    }
}
