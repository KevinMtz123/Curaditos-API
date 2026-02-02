using Curaditos.Infraestructure.Data.Data.Entities;

namespace Curaditos.Data.Entities
{
    public class PromotionProduct
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int PromotionsId { get; set; }
        public Producto Producto { get; set; }
        public Promotion Promotions { get; set; }
    }
}
