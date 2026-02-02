using Shared.Enums;

namespace Curaditos.Data.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TypePromotion TypePromotion { get; set; }
        public int DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime Created { get; set; }
        public ICollection<PromotionProduct> PromotionProducts { get; set; }
       = new List<PromotionProduct>();
    }
}
