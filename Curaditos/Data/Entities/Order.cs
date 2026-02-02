using Shared.Enums;

namespace Curaditos.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime? FechaCompra { get; set; }
        public string? NombreConsumidor { get; set; }
        public string? Phone { get; set; }
        public decimal? Total { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
