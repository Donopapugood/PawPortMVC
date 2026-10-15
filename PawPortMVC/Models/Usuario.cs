namespace PawPortMVC.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public int? IdSucursal { get; set; }

        public string Nombre { get; set; } = "";

        public string Apellido { get; set; } = "";

        public string CorreoElectronico { get; set; } = "";

        public string Rol { get; set; } = "";

        public bool Estado { get; set; }
    }
}