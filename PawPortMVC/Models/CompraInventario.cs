namespace PawPortMVC.Models
{
    public class CompraInventario
    {
        public int Id { get; set; }
        public string Fecha { get; set; } = "";
        public string Producto { get; set; } = "";
        public string Proveedor { get; set; } = "";
        public string Clinica { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Total => Cantidad * CostoUnitario;
    }
}