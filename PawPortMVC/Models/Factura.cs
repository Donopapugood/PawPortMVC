namespace PawPortMVC.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; } = "";
        public string Fecha { get; set; } = "";
        public string Propietario { get; set; } = "";
        public string Mascota { get; set; } = "";
        public string Clinica { get; set; } = "";
        public string Concepto { get; set; } = "";
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = "";
        public string Estado { get; set; } = "Pendiente";
    }
}
