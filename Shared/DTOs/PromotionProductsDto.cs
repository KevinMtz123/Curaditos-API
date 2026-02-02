using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class PromotionProductDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int PromotionsId { get; set; }
        public ProductDto? Producto { get; set; }
        public PromotionDto? Promotions { get; set; }
    }
    public class PromotionProductCreateDto
    {
        public int ProductoId { get; set; }
        public int PromotionsId { get; set; }
        public ProductDto? Producto { get; set; }
        public PromotionDto? Promotions { get; set; }
    }
}
