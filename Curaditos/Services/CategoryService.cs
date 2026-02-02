using AutoMapper;
using Curaditos.Infraestructure.Data.Data;
using Curaditos.Infraestructure.Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Curaditos.Services
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> GetCategoria();
        Task<CategoriaDto> AddCategoria(CategoriaCreateDto dto);
        Task<CategoriaDto?> GetCategoriaById(int id);
        Task<CategoriaDto?> UpdateCategoria(int id, CategoriaCreateDto dto);
    }

    public class CategoriaService(ApplicationDbContext context, IMapper mapper) : ICategoriaService
    {
        public async Task<CategoriaDto> AddCategoria(CategoriaCreateDto dto)
        {
            var categoria = mapper.Map<Categoria>(dto);
            categoria.Serie = Guid.NewGuid();
            await context.AddAsync(categoria);
            await context.SaveChangesAsync();
            return mapper.Map<CategoriaDto>(categoria);

        }

        public async Task<List<CategoriaDto>> GetCategoria()
        {
            var Vacacioness = await context.Categorias.ToListAsync();

            return mapper.Map<List<CategoriaDto>>(Vacacioness);
        }

        public async Task<CategoriaDto?> GetCategoriaById(int id)
        {
            var Categoria = await context.Categorias.FirstOrDefaultAsync(i => i.Id == id);

            return Categoria is null ? null : mapper.Map<CategoriaDto>(Categoria);
        }

        public async Task<CategoriaDto?> UpdateCategoria(int id, CategoriaCreateDto dto)
        {
            var Categoria = await context.Categorias.FindAsync(id);
            if (Categoria is null)
                throw new Exception("Categoria no encontrado");
            mapper.Map(dto, Categoria);
            await context.SaveChangesAsync();
            return mapper.Map<CategoriaDto>(Categoria);

        }
    }
    }
