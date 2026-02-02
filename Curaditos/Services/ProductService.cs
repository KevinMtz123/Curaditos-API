using AutoMapper;
using Curaditos.Infraestructure.Data.Data;
using Curaditos.Infraestructure.Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;

namespace Curaditos.Services
{
    public interface IProductService
    {
        Task<int> CreateAsync(ProductCreateDto dto);
        Task UpdateAsync(int id, ProductCreateDto dto);
        Task<List<ProductDto>> GetProducts();
        Task<ProductDto?> GetProductById(int id);
    }
    public class ProductService(ApplicationDbContext context, IMapper mapper) : IProductService
    {
       
        public async Task<int> CreateAsync(ProductCreateDto dto)
        {
           
            var product = new Producto
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoriaId = dto.CategoriaId,
                Image = dto.Image,
                Active = true,
                Date = DateTime.UtcNow
            };

            context.Productos.Add(product);
            await context.SaveChangesAsync();

            return product.Id;
        }

        public async Task UpdateAsync(int id, ProductCreateDto dto)
        {
            var product = await context.Productos.FindAsync(id)
                ?? throw new Exception("Producto no encontrado");

            if (!string.IsNullOrWhiteSpace(dto.Image))
            {
                product.Image = dto.Image; 
            }

            product.Name = dto.Name ?? product.Name;
            product.Description = dto.Description ?? product.Description;
            product.Price = dto.Price ?? product.Price;
            product.CategoriaId = dto.CategoriaId ?? product.CategoriaId;

            await context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var products = await context.Productos.ToListAsync();

            return mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var product = await context.Productos.FirstOrDefaultAsync(i => i.Id == id);

            return product is null ? null : mapper.Map<ProductDto>(product);
        }

    }

}
