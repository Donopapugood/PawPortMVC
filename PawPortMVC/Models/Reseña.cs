namespace PawPortMVC.Models
{
    public class Reseña
    {
        public int Id { get; set; }
        public string Autor { get; set; } = "";
        public string Pais { get; set; } = "";
        public string Clinica { get; set; } = "";
        public string Comentario { get; set; } = "";
        public int Calificacion { get; set; }
        public string Fecha { get; set; } = "";
    }
}
