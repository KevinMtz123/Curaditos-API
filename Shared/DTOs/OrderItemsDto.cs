using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; }

        public int OrdersId { get; set; }

        public int ProductoId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Subtotal { get; set; }

        public ProductDto Producto { get; set; } = null!;

        public OrderDto Orders { get; set; } = null!;
    }
    public class OrderItemCreateDto
    {

        public int OrdersId { get; set; }

        public int ProductoId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Subtotal { get; set; }

        public ProductCreateDto Producto { get; set; } = null!;

        public OrderCreateDto Orders { get; set; } = null!;
    }
}
