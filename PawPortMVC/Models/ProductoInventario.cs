namespace PawPortMVC.Models
{
    public class ProductoInventario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Categoria { get; set; } = "";
        public string Clinica { get; set; } = "";
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public decimal Precio { get; set; }
        public string Proveedor { get; set; } = "";
        public string FechaCaducidad { get; set; } = "";

        public string EstadoStock
        {
            get
            {
                if (StockActual <= 0) return "Agotado";
                if (StockActual <= StockMinimo) return "Stock Bajo";
                return "En stock";
            }
        }
    }
}
