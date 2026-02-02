using Curaditos.Infraestructure.Data.Data.Entities;

namespace Curaditos.Data.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrdersId { get; set; }

        public int ProductoId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Subtotal { get; set; }

        public Producto Producto { get; set; } = null!;

        public Order Orders { get; set; } = null!;
    }
}
