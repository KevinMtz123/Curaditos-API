using AutoMapper;
using Curaditos.Data.Entities;
using Curaditos.Infraestructure.Data.Data;
using Curaditos.Infraestructure.Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Curaditos.Services
{
    public interface IOrderItemService
    {
        Task<List<OrderItemDto>> GetOrderItem();
        Task<OrderItemDto> AddOrderItem(OrderItemCreateDto dto);
        Task<OrderItemDto?> GetOrderItemById(int id);
        Task<OrderItemDto?> UpdateOrderItem(int id, OrderItemCreateDto dto);
    }

    public class OrderItemService(ApplicationDbContext context, IMapper mapper) : IOrderItemService
    {
        public async Task<OrderItemDto> AddOrderItem(OrderItemCreateDto dto)
        {
            var orderItem = mapper.Map<OrderItem>(dto);
            await context.AddAsync(orderItem);
            await context.SaveChangesAsync();
            return mapper.Map<OrderItemDto>(orderItem);

        }

        public async Task<List<OrderItemDto>> GetOrderItem()
        {
            var Vacacioness = await context.OrderItems.ToListAsync();

            return mapper.Map<List<OrderItemDto>>(Vacacioness);
        }

        public async Task<OrderItemDto?> GetOrderItemById(int id)
        {
            var orderItem = await context.OrderItems.FirstOrDefaultAsync(i => i.Id == id);

            return orderItem is null ? null : mapper.Map<OrderItemDto>(orderItem);
        }

        public async Task<OrderItemDto?> UpdateOrderItem(int id, OrderItemCreateDto dto)
        {
            var orderItem = await context.OrderItems.FindAsync(id);
            if (orderItem is null)
                throw new Exception("orderItem no encontrado");
            mapper.Map(dto, orderItem);
            await context.SaveChangesAsync();
            return mapper.Map<OrderItemDto>(orderItem);

        }
    }
    }
