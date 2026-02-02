using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public Guid Serie { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Image { get; set; } 

        public decimal Price { get; set; }

        public int CategoriaId { get; set; }

        public CategoriaDto Categoria { get; set; }

        public bool Active { get; set; }

        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

        public DateTime Date { get; set; }
    }
    public class ProductCreateDto
    {
      
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Image { get; set; } 

        public decimal? Price { get; set; }

        public int? CategoriaId { get; set; }

        public CategoriaCreateDto Categoria { get; set; }

        public bool Active { get; set; }

        public ICollection<OrderItemCreateDto> OrderItems { get; set; } = new List<OrderItemCreateDto>();
        public DateTime Date { get; set; }


    }
}
