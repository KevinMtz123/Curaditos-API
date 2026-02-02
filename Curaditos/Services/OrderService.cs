using AutoMapper;
using Curaditos.Data.Entities;
using Curaditos.Infraestructure.Data.Data;
using Curaditos.Infraestructure.Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Curaditos.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrder();
        Task<OrderDto> AddOrder(OrderCreateDto dto);
        Task<OrderDto?> GetOrderById(int id);
        Task<OrderDto?> UpdateOrder(int id, OrderCreateDto dto);
    }

    public class OrderService(ApplicationDbContext context, IMapper mapper) : IOrderService
    {
        public async Task<OrderDto> AddOrder(OrderCreateDto dto)
        {
            var order = mapper.Map<Order>(dto);
            await context.AddAsync(order);
            await context.SaveChangesAsync();
            return mapper.Map<OrderDto>(order);

        }

        public async Task<List<OrderDto>> GetOrder()
        {
            var Vacacioness = await context.Orders.ToListAsync();

            return mapper.Map<List<OrderDto>>(Vacacioness);
        }

        public async Task<OrderDto?> GetOrderById(int id)
        {
            var order = await context.Orders.FirstOrDefaultAsync(i => i.Id == id);

            return order is null ? null : mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto?> UpdateOrder(int id, OrderCreateDto dto)
        {
            var order = await context.Orders.FindAsync(id);
            if (order is null)
                throw new Exception("order no encontrado");
            mapper.Map(dto, order);
            await context.SaveChangesAsync();
            return mapper.Map<OrderDto>(order);

        }
    }
    }
