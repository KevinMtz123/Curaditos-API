using AutoMapper;
using Curaditos.Data.Entities;
using Curaditos.Infraestructure.Data.Data;
using Curaditos.Infraestructure.Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Curaditos.Services
{
    public interface IPromotionService
    {
        Task<List<PromotionDto>> GetPromotion();
        Task<PromotionDto> AddPromotion(PromotionCreateDto dto);
        Task<PromotionDto?> GetPromotionById(int id);
        Task<PromotionDto?> UpdatePromotion(int id, PromotionCreateDto dto);
    }

    public class PromotionService(ApplicationDbContext context, IMapper mapper) : IPromotionService
    {
        public async Task<PromotionDto> AddPromotion(PromotionCreateDto dto)
        {
            var promotion = mapper.Map<Promotion>(dto);
            await context.AddAsync(promotion);
            await context.SaveChangesAsync();
            return mapper.Map<PromotionDto>(promotion);

        }

        public async Task<List<PromotionDto>> GetPromotion()
        {
            var Vacacioness = await context.Promotions.ToListAsync();

            return mapper.Map<List<PromotionDto>>(Vacacioness);
        }

        public async Task<PromotionDto?> GetPromotionById(int id)
        {
            var promotion = await context.Promotions.FirstOrDefaultAsync(i => i.Id == id);

            return promotion is null ? null : mapper.Map<PromotionDto>(promotion);
        }

        public async Task<PromotionDto?> UpdatePromotion(int id, PromotionCreateDto dto)
        {
            var promotion = await context.Promotions.FindAsync(id);
            if (promotion is null)
                throw new Exception("promotion no encontrado");
            mapper.Map(dto, promotion);
            await context.SaveChangesAsync();
            return mapper.Map<PromotionDto>(promotion);

        }
    }
    }
