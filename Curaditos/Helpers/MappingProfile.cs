using AutoMapper;
using Curaditos.Data.Entities;
using Curaditos.Infraestructure.Data.Data.Entities;
using Shared.DTOs;

namespace Curaditos.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Categoria, CategoriaDto>();
            CreateMap<CategoriaDto, Categoria>();
            CreateMap<CategoriaCreateDto, Categoria>();

            CreateMap<Producto, ProductDto>();
            CreateMap<ProductDto, Producto>();
            CreateMap<ProductCreateDto, Producto>();

            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderItemCreateDto, OrderItem>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderCreateDto, Order>();

            CreateMap<Promotion, PromotionDto>();
            CreateMap<PromotionDto, Promotion>();
            CreateMap<PromotionCreateDto, Promotion>();

            CreateMap<PromotionProduct, PromotionProductDto>();
            CreateMap<PromotionProductDto, PromotionProduct>();
            CreateMap<PromotionProductCreateDto, PromotionProduct>();
        }
    }
}
