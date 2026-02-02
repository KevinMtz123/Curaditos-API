using AutoMapper;
using Curaditos.Data.Entities;
using Curaditos.Infraestructure.Data.Data;
using Curaditos.Infraestructure.Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Curaditos.Services
{
    public interface IPromotionProductService
    {
        Task<List<PromotionProductDto>> GetPromotionProduct();
        Task<PromotionProductDto> AddPromotionProduct(PromotionProductCreateDto dto);
        Task<PromotionProductDto?> GetPromotionProductById(int id);
        Task<PromotionProductDto?> UpdatePromotionProduct(int id, PromotionProductCreateDto dto);
    }

    public class PromotionProductService(ApplicationDbContext context, IMapper mapper) : IPromotionProductService
    {
        public async Task<PromotionProductDto> AddPromotionProduct(PromotionProductCreateDto dto)
        {
            var promotionProduct = mapper.Map<PromotionProduct>(dto);
            await context.AddAsync(promotionProduct);
            await context.SaveChangesAsync();
            return mapper.Map<PromotionProductDto>(promotionProduct);

        }

        public async Task<List<PromotionProductDto>> GetPromotionProduct()
        {
            var Vacacioness = await context.PromotionProducts.ToListAsync();

            return mapper.Map<List<PromotionProductDto>>(Vacacioness);
        }

        public async Task<PromotionProductDto?> GetPromotionProductById(int id)
        {
            var promotionProduct = await context.PromotionProducts.FirstOrDefaultAsync(i => i.Id == id);

            return promotionProduct is null ? null : mapper.Map<PromotionProductDto>(promotionProduct);
        }

        public async Task<PromotionProductDto?> UpdatePromotionProduct(int id, PromotionProductCreateDto dto)
        {
            var promotionProduct = await context.PromotionProducts.FindAsync(id);
            if (promotionProduct is null)
                throw new Exception("promotionProduct no encontrado");
            mapper.Map(dto, promotionProduct);
            await context.SaveChangesAsync();
            return mapper.Map<PromotionProductDto>(promotionProduct);

        }
    }
    }
