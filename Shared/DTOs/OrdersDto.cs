using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime? FechaCompra { get; set; }
        public string? NombreConsumidor { get; set; }
        public string? Phone { get; set; }
        public decimal? Total { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
    public class OrderCreateDto
    {
        public DateTime? FechaCompra { get; set; }
        public string? NombreConsumidor { get; set; }
        public string? Phone { get; set; }
        public decimal? Total { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<OrderItemCreateDto> OrderItems { get; set; } = new List<OrderItemCreateDto>();
    }
}
