using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class PromotionDto
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
        public ICollection<PromotionProductDto> PromotionProducts { get; set; }
       = new List<PromotionProductDto>();
    }
    public class PromotionCreateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TypePromotion TypePromotion { get; set; }
        public int DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime Created { get; set; }
        public ICollection<PromotionProductCreateDto> PromotionProducts { get; set; }
       = new List<PromotionProductCreateDto>();
    }
}
